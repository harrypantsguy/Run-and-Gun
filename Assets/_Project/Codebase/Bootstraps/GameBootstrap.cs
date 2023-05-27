using _Project.Codebase.AssetGroups;
using _Project.Codebase.Modules;
using _Project.Codebase.Services;
using Cysharp.Threading.Tasks;
using DanonFramework.Core.BootstrapLayer;
using DanonFramework.Core.ContentLayer;
using DanonFramework.Core.ModuleLayer;
using DanonFramework.Core.ServiceLayer;
using UnityEngine;

namespace _Project.Codebase.Bootstraps
{
    [CreateAssetMenu(fileName = nameof(GameBootstrap), menuName = "Bootstraps/" + nameof(GameBootstrap))]
    public class GameBootstrap : ScriptableBootstrap
    {
        public override async UniTask Boot(ServiceContainer services, ModuleContainer modules)
        {
            var contentService = services.Get<ContentService>();
            await contentService.LoadAssetGroupAsync<PrefabAssetGroup, GameObject>();
            await contentService.LoadAssetGroupAsync<ScriptableAssetGroup, ScriptableObject>();
            services.Add(new UIKeycodeOverrideService());
            
            GameModule game = new GameModule();
            await modules.LoadAsync(game);
            await modules.LoadAsync(new GameUIModule());
            await game.SetSceneActive();
        }
    }
}