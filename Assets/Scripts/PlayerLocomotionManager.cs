using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerLocomotionManager : MonoBehaviour
{
    public bool canMove;

    [Header("Input")]
    private float verticalInput = 0;
    private float horizontalInput = 0;
    private Vector3 moveDirection;

    [Header("Settings")]
    [SerializeField] private float moveSpeed;


    private void FixedUpdate()
    {
        HandlePlayerMovement();
    }

    private void GetMovementValues()
    {
        verticalInput = PlayerManager.instance.playerInputManager.vertical_Input;
        horizontalInput = PlayerManager.instance.playerInputManager.horizontal_Input;
    }

    private void HandlePlayerMovement()
    {
        if (!canMove)
            return;

        GetMovementValues();

        moveDirection = PlayerManager.instance.transform.up * verticalInput;
        moveDirection += PlayerManager.instance.transform.right * horizontalInput;
        moveDirection.Normalize();
        moveDirection.y = 0;

        PlayerManager.instance.transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }
}
