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

        public static float DEFAULT_SPEED = 75f;
        public static float MAX_TRAVEL_DIST = 120f;
        public static float IMPACT_OFFSET_DIST = .025f;

        private void Start()
        {
            m_remainingPierces = 1;
            m_remainingRicochets = 1;
            
            m_lastFixedTimeCall = Time.time;
            m_oldPosition = transform.position;
            float tickTravelDist = m_speed * Time.fixedDeltaTime;
            m_nextPosition = transform.position + transform.right * tickTravelDist;
            m_interpolationTime = Time.fixedDeltaTime;
            //transform.localScale = new Vector3(tickTravelDist, transform.localScale.y, transform.localPosition.z);
        }

        private void LateUpdate()
        {
            transform.position = Vector2.Lerp(m_oldPosition, m_nextPosition, 
                (Time.time - m_lastFixedTimeCall) / m_interpolationTime);
        }

        private void FixedUpdate()
        {
            m_lastFixedTimeCall = Time.fixedTime;
            m_oldPosition = transform.position;
            
            float dist = m_speed * Time.fixedDeltaTime;
            Vector2 newPosition = transform.position + transform.right * dist;

            RaycastHit2D hit = Physics2D.Linecast(transform.position, newPosition);
            if (hit.collider != null)
            {
                Vector2 testPos = hit.point - hit.normal * .1f;
                Cell cell = Building.building.GetWallAtPos(testPos);
                
                Debug.DrawLine(hit.point, Vector3.zero, Color.red, 1f);
                
                if (m_cellInsideOf != cell)
                {
                    m_cellInsideOf = cell;

                    bool reachedExactlyZero = false;
                    if (m_remainingPierces > 0)
                    {
                        m_remainingPierces -= cell.pierceInfluence;
                        if (m_remainingPierces == 0) 
                            reachedExactlyZero = true;
                    }

                    if (!reachedExactlyZero && m_remainingPierces <= 0)
                    {
                        m_nextPosition = (Vector3)hit.point - (transform.right * IMPACT_OFFSET_DIST);
                        m_interpolationTime = (m_nextPosition - (Vector2)transform.position).magnitude / m_speed;
                        Destroy(gameObject, m_interpolationTime);
                        return;
                    }
                }
            }

            m_distTraveled += dist;
            if (m_distTraveled > MAX_TRAVEL_DIST)
                Destroy(gameObject);
            
            m_nextPosition = newPosition;
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