using Arena.Components;
using System;
using UnityEngine;

namespace Arena.Strategies.Movement
{
    public class JoystickMovementStrategy : IMovementStrategy
    {
        public event Action<bool> OnMoveStateChanged;

        private readonly CustomJoystick _joystick;
        private readonly Rigidbody _rigidbody;
        private readonly StatsComponent _statsComponent;
        private readonly Transform _body;

        private static readonly float ANGULAR_SPEED = 15f;

        private bool _isMove;
        public bool IsMove
        {
            get => _isMove;
            private set
            {
                if (_isMove == value)
                    return;

                _isMove = value;
                OnMoveStateChanged?.Invoke(value);
            }
        }

        public JoystickMovementStrategy(CustomJoystick joystick, Rigidbody rigidbody, StatsComponent statsComponent, Transform body)
        {
            _joystick = joystick;
            _rigidbody = rigidbody;
            _statsComponent = statsComponent;
            _body = body;
        }

        public void Execute()
        {
            IsMove = _joystick.HasInput;

            if (IsMove)
            {
                Quaternion targetRotation = GetRotation();
                if (targetRotation != Quaternion.identity)
                    _body.rotation = Quaternion.Slerp(_body.rotation, targetRotation, Time.fixedDeltaTime * ANGULAR_SPEED);

                _rigidbody.velocity = _statsComponent[StatsSystem.StatType.Speed].Value * _joystick.DirectionXZ;
            }
            else
            {
                _rigidbody.velocity = Vector3.zero;
            }
        }

        private Quaternion GetRotation()
        {
            if (_joystick.DirectionXZ == Vector3.zero)
                return Quaternion.identity;

            return Quaternion.LookRotation(new Vector3(_joystick.DirectionXZ.x, 0f, _joystick.DirectionXZ.z).normalized);
        }
    }
}
