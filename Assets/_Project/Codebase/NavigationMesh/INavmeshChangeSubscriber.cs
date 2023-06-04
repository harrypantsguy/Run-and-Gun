namespace _Project.Codebase.NavigationMesh
{
    public interface INavmeshChangeSubscriber
    {
        public bool NavmeshReferenceDirty { get; set; }
    }
}