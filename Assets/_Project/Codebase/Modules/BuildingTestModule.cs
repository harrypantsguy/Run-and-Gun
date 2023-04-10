using Cysharp.Threading.Tasks;
using DanonFramework.Runtime.Core.ModuleLayer;
using DanonFramework.Runtime.Core.Utilities;

namespace _Project.Codebase.Modules
{
    public class BuildingTestModule : IAsyncModule
    {
        private const string c_scene_name = "BuildingTestScene";
        public async UniTask LoadAsync()
        {
            await SceneUtilities.LoadSceneAsync(c_scene_name);
            //await SceneUtilities.SetActiveSceneAsync(c_scene_name);
        }

        public async UniTask UnloadAsync()
        {
            await SceneUtilities.UnloadSceneAsync(c_scene_name);
        }
    }
}