namespace Patterns.State.PlayerBehavior
{
    public interface IPlayerBehavior
    {
        void Enter(Player player);
        void Exit(Player player);
        void Update(Player player);
    }
}
