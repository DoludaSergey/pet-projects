namespace Patterns.State.PlayerBehavior
{
    public class Player
    {
        private Dictionary<Type, IPlayerBehavior> _behaviorsMap;

        private IPlayerBehavior _playerBehaviorCurrent;

        private void Start()
        {
            InitBehaviors();
            SetBehaviorByDefault();
        }

        private void InitBehaviors()
        {
            _behaviorsMap = new Dictionary<Type, IPlayerBehavior>();

            _behaviorsMap.Add(typeof(PlayerBehaviorActive), new PlayerBehaviorActive());
            _behaviorsMap.Add(typeof(PlayerBehaviorAggressive), new PlayerBehaviorAggressive());
            _behaviorsMap.Add(typeof(PlayerBehaviorIdle), new PlayerBehaviorIdle());
        }

        private void SetBehavior(IPlayerBehavior newPlayerBehavior)
        {
            if (_playerBehaviorCurrent != null)
            {
                _playerBehaviorCurrent.Exit(this);
            }
            
            _playerBehaviorCurrent = newPlayerBehavior;
            _playerBehaviorCurrent.Enter(this);
        }

        private void SetBehaviorByDefault()
        {
            SetBehaviorIdle();
        }

        private IPlayerBehavior GetBehavior<T>() where T : IPlayerBehavior
        {
            return _behaviorsMap[typeof(T)];
        }

        private void Update()
        {
            if (_playerBehaviorCurrent != null)
            {
                _playerBehaviorCurrent.Update(this);
            }
        }

        public void SetBehaviorIdle()
        {
            var behavior = GetBehavior<PlayerBehaviorIdle>();
            
            SetBehavior(behavior);
        }

        public void SetBehaviorAggressive()
        {
            var behavior = GetBehavior<PlayerBehaviorAggressive>();

            SetBehavior(behavior);
        }

        public void SetBehaviorActive()
        {
            var behavior = GetBehavior<PlayerBehaviorActive>();

            SetBehavior(behavior);
        }
    }
}
