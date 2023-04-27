using DanonFramework.Runtime.Core.ContentLayer;
using JetBrains.Annotations;

namespace _Project.Codebase.AssetGroups
{
    [UsedImplicitly]
    public class PrefabAssetGroup : IAssetGroup
    {
        public const string SHOOTER = "Shooter";
        public const string BASIC_PROJECTILE = "BasicProjectile";
        public const string GLASS_PIERCE_PARTICLE_SYSTEM = "GlassPierceParticleSystem";
        public const string FLESH_PIERCE_PARTICLE_SYSTEM = "FleshPierceParticleSystem";
        public const string BUILDING = "Building";
        public const string ENEMY = "Enemy";
        public const string RUNNER = "Runner";
        public const string GAME_UI_CANVAS = "GameUICanvas";
    }
}