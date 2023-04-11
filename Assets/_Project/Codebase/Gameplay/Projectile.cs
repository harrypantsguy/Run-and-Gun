using DanonFramework.Runtime.Core.Utilities;
using UnityEngine;

namespace _Project.Codebase.Gameplay
{
    public class Projectile : MonoBehaviour
    {
        private float m_speed;
        private bool m_queueToDestroy;
        private float m_distTraveled;
        private Vector2 m_nextPosition;
        private Vector2 m_oldPosition;
        private float m_lastFixedTimeCall;
        private float m_interpolationTime;
        private int m_remainingPierces;
        private int m_remainingRicochets;
        private Cell m_cellInsideOf;
        private float m_equivalentUnitDistance;
        private float m_tilePierceDistSum;

        public static float DEFAULT_SPEED = 75f;
        public static float MAX_TRAVEL_DIST = 120f;
        public static float IMPACT_OFFSET_DIST = .025f;

        private void Start()
        {
            Debug.Break();
            m_remainingPierces = 2;
            m_remainingRicochets = 1;
            
            m_lastFixedTimeCall = Time.time;
            m_oldPosition = transform.position;
            float tickTravelDist = m_speed * Time.fixedDeltaTime;
            m_nextPosition = transform.position + transform.right * tickTravelDist;
            m_interpolationTime = Time.fixedDeltaTime;

            m_equivalentUnitDistance = CalculateEquivalentTravelDistanceFromDirection(transform.right);
            //transform.localScale = new Vector3(tickTravelDist, transform.localScale.y, transform.localPosition.z);
        }

        private void LateUpdate()
        {
            Debug.Log($"{m_tilePierceDistSum} {m_remainingPierces}");
            transform.position = Vector2.Lerp(m_oldPosition, m_nextPosition, 
                (Time.time - m_lastFixedTimeCall) / m_interpolationTime);
        }

        private void FixedUpdate()
        {
            m_lastFixedTimeCall = Time.fixedTime;
            m_oldPosition = transform.position;
            
            float dist = m_speed * Time.fixedDeltaTime;
            Vector2 newPosition = transform.position + transform.right * dist;

            Debug.DrawLine(transform.position, newPosition, new Color(0f, 1f, 0f, .5f));
            
            RaycastHit2D hit = Physics2D.Linecast(transform.position, newPosition);
            if (hit.collider != null)
            {
                Vector2 testPos = hit.point - hit.normal * .1f;
                Cell cell = Building.building.GetWallAtPos(testPos);

                if (m_cellInsideOf == null)
                    m_tilePierceDistSum += (newPosition - hit.point).magnitude;

                if (m_tilePierceDistSum > m_equivalentUnitDistance)
                {
                    m_tilePierceDistSum -= m_equivalentUnitDistance;
                    m_remainingPierces--;
                }

                Debug.DrawLine(hit.point, Vector3.zero, Color.red, 1f);
                
                if (m_cellInsideOf != cell)
                {
                    m_cellInsideOf = cell;

                    bool reachedExactlyZero = false;
                    if (m_remainingPierces > 0)
                    {
                        m_remainingPierces -= cell.pierceInfluence; // REDETERMINE PIERCE INFLUENCE CONSTANTLY WHILE INSIDE
                        if (m_remainingPierces == 0) 
                            reachedExactlyZero = true;
                    }

                    if (!reachedExactlyZero && m_remainingPierces <= 0)
                    {
                        m_nextPosition = (Vector3)hit.point - transform.right * IMPACT_OFFSET_DIST;
                        m_interpolationTime = (m_nextPosition - (Vector2)transform.position).magnitude / m_speed;
                        Destroy(gameObject, m_interpolationTime);
                        return;
                    }
                }
            } 
            else if (m_cellInsideOf != null)
            {
                RaycastHit2D reverseCast = Physics2D.Raycast(newPosition, -transform.right, dist);
                
                if (reverseCast.collider == null)
                {
                    Debug.DrawRay(newPosition, -transform.right, Color.yellow);
                    m_tilePierceDistSum += dist;
                }
                else
                {
                    Debug.DrawLine(newPosition, reverseCast.point, Color.red);
                    m_cellInsideOf = null;
                    float additionalDistTraveledInside = dist - reverseCast.distance;
                
                    m_tilePierceDistSum += additionalDistTraveledInside;
                }

                if (m_tilePierceDistSum > m_equivalentUnitDistance)
                {
                    m_remainingPierces--; // ACCOUNT FOR PIERCE INFLUENCE
                    m_tilePierceDistSum -= m_equivalentUnitDistance;
                }

                if (m_remainingPierces <= 0)
                {
                    m_nextPosition = newPosition;
                    m_interpolationTime = (m_nextPosition - (Vector2)transform.position).magnitude / m_speed;
                    Destroy(gameObject, m_interpolationTime);
                    return;
                }
            }

            m_distTraveled += dist;
            if (m_distTraveled > MAX_TRAVEL_DIST)
                Destroy(gameObject);
            
            m_nextPosition = newPosition;
        }

        private static float CalculateEquivalentTravelDistanceFromDirection(Vector2 direction)
        {
            float x = direction.x;
            float y = direction.y;

            float slope = Mathf.Abs(x) > Mathf.Abs(y) ? y / x : x / y;
            return Mathf.Sqrt(slope * slope + 1);
        }

        public static void SpawnProjectile(Vector2 position, Vector2 direction, float speed)
        {
            GameObject newProj = Instantiate(ContentUtilities.GetCachedAsset<GameObject>(PrefabAssetGroup.BASIC_PROJECTILE));
            newProj.transform.right = direction;
            newProj.transform.position = position;
            newProj.GetComponent<Projectile>().m_speed = speed;
        }
    }
}