namespace Patterns.State
{
    abstract class State
    {
        protected TrafficLight trafficLight;

        public TrafficLight TrafficLight { set => trafficLight = value; }

        public abstract void NextState();
        public abstract void PreviousState();
    }

    class TrafficLight
    {
        private State _state;

        public TrafficLight(State state)
        {
            this._state = state;
        }

        public void SetState(State state)
        {
            _state = state;
            _state.TrafficLight = this;
        }

        public void NextState()
        {
            _state?.NextState();
        }

        public void PreviousState()
        {
            _state?.PreviousState();
        }
    }

    class GreenState : State
    {
        public override void NextState()
        {
            Console.WriteLine("From green to yellow");

            trafficLight.SetState(new YellowState());
        }

        public override void PreviousState()
        {
            Console.WriteLine("Green collor");
        }
    }

    class YellowState : State
    {
        public override void NextState()
        {
            Console.WriteLine("From yellow to red");

            trafficLight.SetState(new RedState());
        }

        public override void PreviousState()
        {
            Console.WriteLine("From yellow to green");

            trafficLight.SetState(new GreenState());
        }
    }

    class RedState : State
    {
        public override void NextState()
        {
            Console.WriteLine("Red collor");
        }

        public override void PreviousState()
        {
            Console.WriteLine("From red to yellow");

            trafficLight.SetState(new YellowState());
        }
    }
}
