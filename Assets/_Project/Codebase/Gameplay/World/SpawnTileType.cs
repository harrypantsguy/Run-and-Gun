using System;

namespace _Project.Codebase.Gameplay.World
{
    [Flags]
    public enum SpawnTileType
    {
        None =     0,
        Item =     1 << 0,
        KeyItem =  1 << 1,
        Enemy =    1 << 2,
    }
}