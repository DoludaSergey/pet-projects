using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patterns.State.PlayerBehavior
{
    internal class PlayerBehaviorActive : IPlayerBehavior
    {
        public void Enter(Player player)
        {
            Console.WriteLine("Enter ACTIVE behavior");
        }

        public void Exit(Player player)
        {
            Console.WriteLine("Exit ACTIVE behavior");
        }

        public void Update(Player player)
        {
            Console.WriteLine("Update ACTIVE behavior");
        }
    }
}
