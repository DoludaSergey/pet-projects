namespace Patterns.State.PlayerBehavior
{
    internal class PlayerBehaviorIdle : IPlayerBehavior
    {
        public void Enter(Player player)
        {
            Console.WriteLine("Enter IDLE behavior");
        }

        public void Exit(Player player)
        {
            Console.WriteLine("Exit IDLE behavior");
        }

        public void Update(Player player)
        {
            Console.WriteLine("Update IDLE behavior");
        }
    }
}
