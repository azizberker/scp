using UnityEngine;

public class DoorController : MonoBehaviour
{
    private bool isOpen = false;
    private Quaternion closedRotation;
    private Quaternion openRotation;

    [SerializeField] private float openAngle = 90f;
    [SerializeField] private float rotationSpeed = 2f;

    void Start()
    {
        // D�n��leri ba�lang��ta hesapla
        closedRotation = transform.rotation;
        openRotation = Quaternion.Euler(0, transform.eulerAngles.y + openAngle, 0);
    }

    public void ToggleDoor()
    {
        isOpen = !isOpen;
    }

    void Update()
    {
        // Hedef d�n��� belirle
        Quaternion targetRotation = isOpen ? openRotation : closedRotation;
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }
}
