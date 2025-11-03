using System;

namespace Arena.Strategies.Movement
{
    public interface IMovementStrategy
    {
        public event Action<bool> OnMoveStateChanged;

        public bool IsMove { get; }

        public void Execute();
    }
}
