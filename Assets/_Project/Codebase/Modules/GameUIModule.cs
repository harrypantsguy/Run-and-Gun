using Cysharp.Threading.Tasks;
using DanonFramework.Runtime.Core.ModuleLayer;
using DanonFramework.Runtime.Core.Utilities;
using UnityEngine;

namespace _Project.Codebase.Modules
{
    public class GameUIModule : IModule
    {
        private const string c_scene_name = "GameUIScene";

        public void Load()
        {
            SceneUtilities.CreateScene(c_scene_name);
            SceneUtilities.SetActiveSceneAsync(c_scene_name).Forget();

            ContentUtilities.Instantiate<GameObject>(PrefabAssetGroup.GAME_UI_CANVAS);
        }

        public void Unload()
        {
            SceneUtilities.UnloadSceneAsync(c_scene_name).Forget();
        }
    }
}