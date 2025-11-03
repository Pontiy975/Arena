using UnityEngine;

namespace Arena.Player.Components
{
    public class AnimatorComponent : MonoBehaviour
    {
        [SerializeField] private Animator animator;

        private readonly int MovementID = Animator.StringToHash("IsMove");

        private void OnValidate()
        {
            animator ??= GetComponentInChildren<Animator>();
        }

        public void ToggleMovement(bool isMove)
        {
            if (animator.GetBool(MovementID) != isMove)
                animator.SetBool(MovementID, isMove);
        }
    }
}
