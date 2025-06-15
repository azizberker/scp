using UnityEngine;

public class MouseLook : MonoBehaviour
{
    

    [Header("Mouse Settings")]
    [Range(0.1f, 10f)]
    public float mouseSensitivity = 2.0f;
    
    [Range(0.1f, 1f)]
    public float smoothing = 0.2f;

    [Header("Rotation Limits")]
    public float minVerticalAngle = -89f;
    public float maxVerticalAngle = 89f;

    [Header("References")]
    public Transform playerBody;

    // Private variables
    private Vector2 smoothedMouseDelta;
    private Vector2 currentMouseDelta;
    private float currentVerticalRotation = 0f;
    private bool cursorLocked = true;

    private void Start()
    {
        SetCursorState(true);
    }

    private void Update()
    {
        

        HandleMouseInput();
        HandleCursorLock();
    }

    private void HandleMouseInput()
    {
        // Get raw mouse input
        Vector2 mouseDelta = new Vector2(
            Input.GetAxisRaw("Mouse X"),
            Input.GetAxisRaw("Mouse Y")
        );

        // Scale input by sensitivity
        mouseDelta *= mouseSensitivity;

        // Smooth the mouse movement
        currentMouseDelta = Vector2.Lerp(currentMouseDelta, mouseDelta, 1f / smoothing * Time.deltaTime);
        smoothedMouseDelta = currentMouseDelta;

        // Rotate camera up/down
        currentVerticalRotation -= smoothedMouseDelta.y;
        currentVerticalRotation = Mathf.Clamp(currentVerticalRotation, minVerticalAngle, maxVerticalAngle);
        transform.localRotation = Quaternion.Euler(currentVerticalRotation, 0f, 0f);

        // Rotate player body left/right
        if (playerBody != null)
        {
            playerBody.Rotate(Vector3.up * smoothedMouseDelta.x);
        }
    }

    private void HandleCursorLock()
    {
        // Toggle cursor lock with Escape key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SetCursorState(!cursorLocked);
        }
    }

    private void SetCursorState(bool locked)
    {
        cursorLocked = locked;
        Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !locked;
    }
}