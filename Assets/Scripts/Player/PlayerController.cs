using Arena.Components;
using Arena.Player.Components;
using Arena.Player.Data;
using Arena.Strategies.Movement;
using UnityEngine;
using Zenject;

namespace Arena.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private CharacterModel model; // temp

        [SerializeField] private StatsComponent statsComponent;
        [SerializeField] private AnimatorComponent animatorComponent;

        [SerializeField] private Transform body;
        [SerializeField] private Rigidbody _rigidbody;

        private Transform _transform;

        private IMovementStrategy _movement;

        private void OnValidate()
        {
            statsComponent ??= GetComponent<StatsComponent>();
            animatorComponent ??= GetComponent<AnimatorComponent>();
            _rigidbody ??= GetComponent<Rigidbody>();
        }

        private void Awake()
        {
            _transform = transform;

            statsComponent.SetBaseStats(model.Stats);
            statsComponent.RecalculateStats();
        }

        private void OnDestroy()
        {
            if (_movement != null)
                _movement.OnMoveStateChanged -= OnMoveStateChanged;
        }

        [Inject]
        private void Construct(CustomJoystick joystick)
        {
            _movement ??= new JoystickMovementStrategy(joystick, _rigidbody, statsComponent, body);
            _movement.OnMoveStateChanged += OnMoveStateChanged;
        }

        private void FixedUpdate()
        {
            _movement?.Execute();
        }

        private void OnMoveStateChanged(bool isMove) => animatorComponent.ToggleMovement(isMove);
    }
}
