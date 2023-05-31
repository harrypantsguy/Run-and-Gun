using DanonFramework.Core.ContentLayer;
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
        public const string ENEMY = "Enemy";
        public const string RUNNER = "Runner";
        public const string GAME_UI_CANVAS = "GameUICanvas";
        public const string NAV_PATH_RENDERER = "NavPathRenderer";
        public const string CHARACTER_SELECTION_RENDERER = "CharacterSelectionRenderer"; 
        public const string CHARACTER_RANGE_RENDERER = "CharacterRangeRenderer"; 
        public const string CHARACTER_HEALTH_RENDERER = "CharacterHealthRenderer";
        public const string DOOR = "Door";
        public const string TILE_BOX_OUTLINE = "TileBoxOutline";
    }
}