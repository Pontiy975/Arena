using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachine
{
    public class StateMachine : IStateMachine
    {
        public event Action<StateMachine> OnStateMachineEnd;
        public event Action<IState> OnStateChanged; 
        public bool Running { get; private set; }
        public bool ForceQuit { get; set; }

        private IState _state;
        private IState _nextState;
        protected Dictionary<object, IState> States;
        protected bool _debug = false;

        public IEnumerable Execute()
        {
            Log($"Start of FSM");
            Running = true;
            while (true)
            {
                // Execute the current state until stops executing
                for ( var e = _state.Execute().GetEnumerator(); e.MoveNext() && _nextState == null && !ForceQuit; )
                    yield return e.Current;

                _state.OnBeginExit -= HandleStateBeginExit;
                _state.EndExit();
                
                // There is no next state to transition to
                // This means the state machine is finished executing
                if (_nextState == null)
                {
                    Log($"End of FSM");
                    Running = false;
                    OnStateMachineEnd?.Invoke(this);
                    break;
                }

                // Switch state
                State = _nextState;
                _nextState = null;
            }
        }

        public IState State
        {
            get => _state;
            protected set
            {
                _state = value;
                _state.OnBeginExit += HandleStateBeginExit;
                Log($"New state: {_state.GetType().Name}");
                OnStateChanged?.Invoke(_state);
            }
        }

        protected void HandleStateBeginExit(object sender, StateBeginExitEventArgs e)
        {
            _nextState = States[e.NextState];
            if(_nextState is IStateWithParams nextStateWithParams)
	            nextStateWithParams.SetArgs(e.Args);
        }

        private void Log(string message)
        {
            if(_debug)
                Debug.Log($"{message}".LogFormat(this, Color.green));
        }
    }
}