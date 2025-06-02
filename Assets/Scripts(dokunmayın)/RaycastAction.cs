using UnityEngine;

public class RaycastAction : MonoBehaviour
{
    private Camera cam;
    private Ray ray;
    private RaycastHit hit;
    public LayerMask interactMask; // Door ve/veya Item layer'ları seçili olmalı

    private DoorController lastDoor = null;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        if (Physics.Raycast(ray, out hit, 3f, interactMask))
        {
            // Sadece kapıya bakıyorsan:
            if (hit.collider.TryGetComponent<DoorController>(out var door))
            {
                door.ShowIcon(true);

                if (lastDoor != null && lastDoor != door)
                    lastDoor.ShowIcon(false);
                lastDoor = door;

                if (Input.GetKeyDown(KeyCode.E))
                {
                    door.ToggleDoor();
                }
            }
            else
            {
                if (lastDoor != null)
                {
                    lastDoor.ShowIcon(false);
                    lastDoor = null;
                }
            }
        }
        else
        {
            if (lastDoor != null)
            {
                lastDoor.ShowIcon(false);
                lastDoor = null;
            }
        }
    }
}
