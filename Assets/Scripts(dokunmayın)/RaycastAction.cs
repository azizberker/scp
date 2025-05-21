using UnityEngine;

public class RaycastAction : MonoBehaviour
{
    private Camera cam;
    private Ray ray;
    private RaycastHit hit;
    public LayerMask interactMask; // Inspector'dan hem "Item" hem "Door" iþaretli olacak

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        if (Physics.Raycast(ray, out hit, 3f, interactMask))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                // Önce kapý kontrolü
                if (hit.collider.TryGetComponent<DoorController>(out var door))
                {
                    Debug.Log("Kapý bulundu! Açma/Kapama çalýþtý.");
                    door.ToggleDoor();
                }
                // Sonra Collectable (item) kontrolü
                if (hit.collider.TryGetComponent<Collectable>(out var collectable))
                {
                    Debug.Log("Collectable bulundu! Toplama çalýþtý.");
                    collectable.Collect();
                }
            }
        }
    }
}
