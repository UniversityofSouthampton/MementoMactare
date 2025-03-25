using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public static PlayerCamera instance;

    [Header("Camera Settings")]
    [SerializeField] float cameraSmoothSpeed = 10f;

    [SerializeField] private SpriteRenderer backgroundSprite;

    [Header("Camera Values")]
    private Vector3 cameraVelocity;
    private Vector3 cameraOffset;
    private float minX;
    private float maxX;

    private Camera cam;

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
        cam = Camera.main;
        
        cameraOffset = transform.position - PlayerManager.instance.transform.position;
        cameraOffset.z = 0;

        float camHorzExtent = cam.orthographicSize * Screen.width / Screen.height;
        minX = (backgroundSprite.bounds.min.x) + camHorzExtent + 0.1f;
        maxX = (backgroundSprite.bounds.max.x) - camHorzExtent - 0.1f;
        Debug.Log(minX);
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
