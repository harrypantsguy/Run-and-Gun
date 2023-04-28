namespace _Project.Codebase.NavigationMesh
{
    public struct PathResults
    {
        public readonly PathResultType type;
        public readonly float distance;

        public PathResults(PathResultType type, float distance)
        {
            this.type = type;
            this.distance = distance;
        }
    }
}