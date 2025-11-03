using UnityEngine;

namespace StateMachine
{
    public class StateMachineRunner : MonoBehaviour
    {
        protected StateMachine FiniteStateMachine;
        
        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        private void OnDestroy()
        {
            if(FiniteStateMachine != null)
                FiniteStateMachine.OnStateMachineEnd -= RunFSM;
        }

        protected void StartFSM(StateMachine fsm)
        {
            FiniteStateMachine = fsm;
            FiniteStateMachine.OnStateMachineEnd += RunFSM;
            RunFSM(FiniteStateMachine);
        }

        protected void RunFSM(StateMachine fsm)
        {
            fsm.ForceQuit = false;
            StartCoroutine(fsm.Execute().GetEnumerator());
        }
    }
}