using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    [HideInInspector] public PlayerInputManager playerInputManager;
    [HideInInspector] public PlayerLocomotionManager playerLocomotionManager;

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

        DontDestroyOnLoad(gameObject);
    }

    private void FixedUpdate()
    {
        PlayerCamera.instance.HandleAllCameraActions();
    }

}
