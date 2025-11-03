using System.Collections;

namespace StateMachine
{
    public interface IStateMachine
    {
        IEnumerable Execute();
    }
}