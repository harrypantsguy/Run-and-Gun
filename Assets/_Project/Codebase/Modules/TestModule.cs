using Cysharp.Threading.Tasks;
using DanonFramework.Core.ModuleLayer;
using DanonFramework.Core.Utilities;

namespace _Project.Codebase.Modules
{
    public class TestModule : IAsyncModule
    {
        private const string c_scene_name = "TestScene";
        
        public async UniTask LoadAsync()
        {
            await SceneUtilities.LoadAddressableSceneAsync(c_scene_name);
            await SceneUtilities.SetActiveSceneAsync(c_scene_name);
        }

        public async UniTask UnloadAsync()
        {
            await SceneUtilities.UnloadSceneAsync(c_scene_name);
        }
    }
}