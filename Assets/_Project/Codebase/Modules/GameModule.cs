using _Project.Codebase.Gameplay;
using _Project.Codebase.Gameplay.World;
using _Project.Codebase.NavigationMesh;
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
        public Navmesh Navmesh { get; private set; }
        public TurnController TurnController { get; private set; }
        public EnemyController EnemyController { get; private set; }
        
        public async UniTask LoadAsync()
        {
            SceneUtilities.CreateScene(c_scene_name);
            await SceneUtilities.SetActiveSceneAsync(c_scene_name);

            GameObject building = 
                Object.Instantiate(ContentUtilities.GetCachedAsset<GameObject>(PrefabAssetGroup.BUILDING));
            Building = building.GetComponent<BuildingAuthoring>().Initialize();

            TurnController = new TurnController();
            
            Object.Instantiate(ContentUtilities.GetCachedAsset<GameObject>(PrefabAssetGroup.SHOOTER));
            EnemyController = new EnemyController(TurnController);
            
            TurnController.StartGame();
        }

        public void SetNavmesh(Navmesh navmesh) => Navmesh = navmesh;

        public async UniTask UnloadAsync()
        {
            await SceneUtilities.UnloadSceneAsync(c_scene_name);
        }
    }
}