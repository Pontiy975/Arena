using System;
using System.Collections;

namespace StateMachine
{
    public interface IState 
    {
        IEnumerable Execute();
        event EventHandler<StateBeginExitEventArgs> OnBeginExit;    
        void EndExit();
    }

    public interface IStateWithParams : IState
    {
	    void SetArgs(params object[] args);
    }
}