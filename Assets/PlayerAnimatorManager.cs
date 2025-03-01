using UnityEngine;

public class PlayerAnimatorManager : MonoBehaviour
{
    Animator playerAnimator;

    private void Start()
    {
        playerAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        HandleWalkingAnimation();
    }
    private void HandleWalkingAnimation()
    {
        if (!PlayerManager.instance.playerLocomotionManager.canMove)
        {
            playerAnimator.SetFloat("MovementValue", 0);
            return;
        }
        playerAnimator.SetFloat("MovementValue", Mathf.Abs(PlayerManager.instance.playerInputManager.horizontal_Input));
    }
    public void PlayAttackAnimation(string attackAnimation)
    {
        playerAnimator.Play(attackAnimation);
    }
}
