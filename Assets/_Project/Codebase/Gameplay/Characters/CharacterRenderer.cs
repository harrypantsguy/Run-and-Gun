using _Project.Codebase.AssetGroups;
using DanonFramework.Core.Utilities;
using UnityEngine;

namespace _Project.Codebase.Gameplay.Characters
{
    public class CharacterRenderer : MonoBehaviour
    {
        [field: SerializeField] public CharacterAnimator Animator { get; private set; }
        public Transform GraphicsTransform => transform;
        public SelectionRenderer SelectionRenderer { get; private set; }
        public NavmeshRangeRenderer RangeRenderer { get; private set; }
        public HealthRenderer HealthRenderer { get; private set; }

        public void Initialize(Character character)
        {
            SelectionRenderer = Instantiate<SelectionRenderer>(PrefabAssetGroup.CHARACTER_SELECTION_RENDERER);
            SelectionRenderer.transform.localPosition = Vector3.zero;
            RangeRenderer = Instantiate<NavmeshRangeRenderer>(PrefabAssetGroup.CHARACTER_RANGE_RENDERER);
            RangeRenderer.character = character;
            HealthRenderer = Instantiate<HealthRenderer>(PrefabAssetGroup.CHARACTER_HEALTH_RENDERER);
            HealthRenderer.transform.localPosition = new Vector3(0f, .6f);
        }

        private T Instantiate<T>(string address)
        {
            GameObject obj = ContentUtilities.Instantiate<GameObject>(address);
            obj.transform.SetParent(transform.parent);
            return obj.GetComponent<T>();
        }
    }
}