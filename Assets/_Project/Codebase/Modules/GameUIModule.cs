using _Project.Codebase.AssetGroups;
using Cysharp.Threading.Tasks;
using DanonFramework.Core.ModuleLayer;
using DanonFramework.Core.Utilities;
using UnityEngine;

namespace _Project.Codebase.Modules
{
    public class GameUIModule : IAsyncModule
    {
        private const string c_scene_name = "GameUIScene";

        public async UniTask LoadAsync()
        {
            SceneUtilities.CreateScene(c_scene_name);
            await SceneUtilities.SetActiveSceneAsync(c_scene_name);

            ContentUtilities.Instantiate<GameObject>(PrefabAssetGroup.GAME_UI_CANVAS);
        }

        public async UniTask UnloadAsync()
        {
            await SceneUtilities.UnloadSceneAsync(c_scene_name);
        }
    }
}