namespace Patterns.State
{
    public partial class Pen
    {
        private IState _state = new Idls();

        private void SetState(IState state) => _state = state;

        public void OnClick(Pen pen) => _state.OnClick(this);

        public void OnClickFinish(Pen pen) => _state.OnClickFinish(this);

        public void OnMove(Pen pen) => _state.OnMove(this);
    }

    public partial class Pen
    {
        private interface IState
        {
            void OnMove(Pen pen);
            void OnClick(Pen pen);
            void OnClickFinish(Pen pen);
        }

        private class Idls : IState
        {
            public void OnClick(Pen pen)
            {
                pen.SetState(new Writing());
            }

            public void OnClickFinish(Pen pen)
            {
                // do nothing
            }

            public void OnMove(Pen pen)
            {
                // do nothing
            }
        }

        private class Writing : IState
        {
            public void OnClick(Pen pen)
            {
                // DRAW HARDER
            }

            public void OnClickFinish(Pen pen)
            {
                pen.SetState(new Idls());
            }

            public void OnMove(Pen pen)
            {
                // draw on canvas
            }
        }
    }
}
