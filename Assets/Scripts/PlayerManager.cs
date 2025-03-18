using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    public GameObject currentEnemy;
    public Animator enemyAnimator;

    [HideInInspector] public PlayerInputManager playerInputManager;
    [HideInInspector] public PlayerLocomotionManager playerLocomotionManager;
    [HideInInspector] public PlayerAnimatorManager playerAnimatorManager;

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
    void Start()
    {
        playerInputManager = GetComponent<PlayerInputManager>();
        playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        playerAnimatorManager = GetComponent<PlayerAnimatorManager>();

        //DontDestroyOnLoad(gameObject);
    }

    private void FixedUpdate()
    {
        PlayerCamera.instance.HandleAllCameraActions();
    }

}
