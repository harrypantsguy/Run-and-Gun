using _Project.Codebase.Modules;
using Cysharp.Threading.Tasks;
using DanonFramework.Runtime.Core.BootstrapLayer;
using DanonFramework.Runtime.Core.ModuleLayer;
using DanonFramework.Runtime.Core.ServiceLayer;
using UnityEngine;

namespace _Project.Codebase.Bootstraps
{
    [CreateAssetMenu(fileName = nameof(TestBootstrap), menuName = "Bootstraps/" + nameof(TestBootstrap))]
    public class TestBootstrap : ScriptableBootstrap
    {
        public override async UniTask Boot(ServiceContainer services, ModuleContainer modules)
        {
            await modules.LoadAsync(new TestModule());
        }
    }
}