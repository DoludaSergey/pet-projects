namespace Patterns.State.PlayerBehavior
{
    internal class PlayerBehaviorAggressive : IPlayerBehavior
    {
        public void Enter(Player player)
        {
            Console.WriteLine("Enter AGGRESSIVE behavior");
        }

        public void Exit(Player player)
        {
            Console.WriteLine("Exit AGGRESSIVE behavior");
        }

        public void Update(Player player)
        {
            Console.WriteLine("Update AGGRESSIVE behavior");
        }
    }
}
