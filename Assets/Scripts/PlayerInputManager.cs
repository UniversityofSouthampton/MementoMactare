using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    PlayerControls playerControls;

    [Header("Movement Input")]
    [SerializeField] Vector2 movement_Input;
    public float vertical_Input;
    public float horizontal_Input;

    [Header("Action Input")]
    public bool fightButton01_Input;

    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();

            //Locomotion inputs
            playerControls.Player.Movement.performed += i => movement_Input = i.ReadValue<Vector2>();

            //Action inputs
            playerControls.Player.Fight01.performed += i => fightButton01_Input = true;
        }

        playerControls.Enable();
    }

    private void Update()
    {
        HandleAllInputs();
    }

    private void HandleAllInputs()
    {
        HandlePlayerMovementInput();
    }

    private void HandlePlayerMovementInput()
    {
        vertical_Input = movement_Input.y;
        horizontal_Input = movement_Input.x;
    }
}
