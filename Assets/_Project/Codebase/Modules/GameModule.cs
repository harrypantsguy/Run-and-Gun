using _Project.Codebase.Gameplay;
using Cysharp.Threading.Tasks;
using DanonFramework.Runtime.Core.ModuleLayer;
using DanonFramework.Runtime.Core.Utilities;
using UnityEngine;

namespace _Project.Codebase.Modules
{
    public class GameModule : IAsyncModule
    {
        private const string c_scene_name = "GameScene";
        
        public Building Building { get; private set; }
        
        public async UniTask LoadAsync()
        {
            SceneUtilities.CreateScene(c_scene_name);
            await SceneUtilities.SetActiveSceneAsync(c_scene_name);

            GameObject building = 
                Object.Instantiate(ContentUtilities.GetCachedAsset<GameObject>(PrefabAssetGroup.BUILDING));
            Building = building.GetComponent<Building>();
            
            Object.Instantiate(ContentUtilities.GetCachedAsset<GameObject>(PrefabAssetGroup.SHOOTER));
        }

        public async UniTask UnloadAsync()
        {
            await SceneUtilities.UnloadSceneAsync(c_scene_name);
        }
    }
}