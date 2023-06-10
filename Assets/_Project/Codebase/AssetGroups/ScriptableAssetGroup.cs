using DanonFramework.Core;
using JetBrains.Annotations;

namespace _Project.Codebase.AssetGroups
{
    [UsedImplicitly]
    public class ScriptableAssetGroup : IAssetGroup
    {
        public const string WALL_COLLECTION = "WallCollection";
        public const string BUILDING_COLLECTION = "BuildingCollection";
        public const string KEY_ITEM_COLLECTION = "KeyItemCollection";
    }
}