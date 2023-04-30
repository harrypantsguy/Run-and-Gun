using _Project.Codebase.AssetGroups;
using _Project.Codebase.Gameplay;
using _Project.Codebase.Gameplay.Characters;
using _Project.Codebase.Gameplay.Player;
using _Project.Codebase.Gameplay.World;
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
        public TurnController TurnController { get; private set; }
        public CharacterManager CharacterManager { get; private set; }
        public WorldRegions WorldRegions { get; private set; }
        public WorldScreenshot World { get; private set; }
        public PlayerManager PlayerManager { get; private set; }

        public async UniTask LoadAsync()
        {
            SceneUtilities.CreateScene(c_scene_name);
            await SceneUtilities.SetActiveSceneAsync(c_scene_name);

            WorldRegions = new WorldRegions();
            
            GameObject building = 
                Object.Instantiate(ContentUtilities.GetCachedAsset<GameObject>(PrefabAssetGroup.BUILDING));
            Building = building.GetComponent<BuildingAuthoring>().Initialize();
            
            TurnController = new TurnController();
            
            CharacterManager = new CharacterManager(TurnController, Building);

            World = new WorldScreenshot(Building);
            
            PlayerManager = new GameObject("PlayerManager").AddComponent<PlayerManager>();

            TurnController.StartGame();
        }
        
        public async UniTask UnloadAsync()
        {
            await SceneUtilities.UnloadSceneAsync(c_scene_name);
        }

        public async UniTask SetSceneActive()
        {
            await SceneUtilities.SetActiveSceneAsync(c_scene_name);
        }
    }
}