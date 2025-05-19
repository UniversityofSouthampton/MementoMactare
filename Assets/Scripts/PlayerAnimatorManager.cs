using UnityEngine;

public class PlayerAnimatorManager : MonoBehaviour
{
    
    public static PlayerAnimatorManager instance;
    //Set up PlayerHealth as a singleton
    //(https://techhub.wsagames.com/guides//1_Unity/1_Coding/1_Advanced-CSharp#programming-design-patterns)
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

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
