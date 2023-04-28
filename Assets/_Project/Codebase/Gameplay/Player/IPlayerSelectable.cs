namespace _Project.Codebase.Gameplay.Player
{
    public interface IPlayerSelectable
    {
        public void SetPlayerSelectState(bool state);
        public PlayerSelectableType SelectableType { get; }
    }
}