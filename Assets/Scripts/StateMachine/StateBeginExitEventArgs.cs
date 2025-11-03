using System;

namespace StateMachine
{
    public class StateBeginExitEventArgs : EventArgs
    {
        public Type NextState { get; }
        public object[] Args { get; }
        
        public StateBeginExitEventArgs(Type nextState, params object[] args)
        {
            NextState = nextState;
            Args = args;
        }
    }
}