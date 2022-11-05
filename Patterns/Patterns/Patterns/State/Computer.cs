namespace Patterns.State
{
    public partial class Computer
    {
        private IState _state = new Off();
        private int stateOldVersion = 0;
        private bool chargingOldVersion = true;

        private void SetState(IState state)
        {
            _state = state;
        }

        public void PressPowerButton()
        {
            _state.PressPowerButton(this);

            // What we try to resolve?

            //// off
            //if (stateOldVersion == 0)
            //{
            //    // do work
            //    stateOldVersion = 1;
            //    return;
            //}

            //// on
            //if (stateOldVersion == 1)
            //{
            //    // do work
            //    if (chargingOldVersion)
            //    {
            //        stateOldVersion = 2;
            //        return;
            //    }

            //    stateOldVersion = 0;
            //    return;
            //}

            //// standby
            //// do work
            //stateOldVersion = 1;
        }
    }

    public partial class Computer
    {
        private interface IState
        {
            void PressPowerButton(Computer computer);
        }

        private class Off : IState
        {
            public void PressPowerButton(Computer computer)
            {
                computer.SetState(new On());
            }
        }

        private class On : IState
        {
            private bool charging;
            
            public void PressPowerButton(Computer computer)
            {
                // perfom some work                
                if (charging)
                {
                    computer.SetState(new Standby());
                }
                else
                {
                    computer.SetState(new Off());
                }
            }
        }

        private class Standby : IState
        {
            public void PressPowerButton(Computer computer)
            {
                // perfom some work 
                computer.SetState(new On());
            }
        }
    }
}
