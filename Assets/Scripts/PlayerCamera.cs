using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public static PlayerCamera instance;

    [Header("Camera Settings")]
    [SerializeField] float cameraSmoothSpeed = 10f;

    [Header("Camera Values")]
    private Vector3 cameraVelocity;
    private Vector3 cameraOffset;
    [SerializeField] private float minX;
    [SerializeField] private float maxX;

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

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        cameraOffset = transform.position - PlayerManager.instance.transform.position;
        cameraOffset.z = 0;
    }

    public void HandleAllCameraActions()
    {
        HandleFollowTarget();
    }

    private void HandleFollowTarget()
    {
        Vector3 targetCameraPosition = Vector3.SmoothDamp(transform.position, PlayerManager.instance.transform.position+cameraOffset, ref cameraVelocity, cameraSmoothSpeed * Time.deltaTime);
        targetCameraPosition = new Vector3(Mathf.Clamp(targetCameraPosition.x, minX, maxX), targetCameraPosition.y, targetCameraPosition.z);
        transform.position = targetCameraPosition;
    }
}
