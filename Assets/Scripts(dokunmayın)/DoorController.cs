using UnityEngine;

public class DoorController : MonoBehaviour
{
    public GameObject interactionIcon; // World Space Canvas veya sadece Image objesi

    private bool isOpen = false;
    private float openAngle = 90f;
    private float closedAngle = 0f;
    private float speed = 5f;

    private void Update()
    {
        float targetAngle = isOpen ? openAngle : closedAngle;
        transform.localRotation = Quaternion.Lerp(
            transform.localRotation,
            Quaternion.Euler(0, targetAngle, 0),
            Time.deltaTime * speed
        );
    }

    public void ToggleDoor()
    {
        isOpen = !isOpen;
    }

    public void ShowIcon(bool show)
    {
        if (interactionIcon != null)
            interactionIcon.SetActive(show);
    }
}

