using UnityEngine;

public class RaycastAction : MonoBehaviour
{
    private Camera cam;
    private Ray ray;
    private RaycastHit hit;
    public LayerMask interactMask; // Inspector'dan hem "Item" hem "Door" i�aretli olacak

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
                // �nce kap� kontrol�
                if (hit.collider.TryGetComponent<DoorController>(out var door))
                {
                    Debug.Log("Kap� bulundu! A�ma/Kapama �al��t�.");
                    door.ToggleDoor();
                }
                // Sonra Collectable (item) kontrol�
                if (hit.collider.TryGetComponent<Collectable>(out var collectable))
                {
                    Debug.Log("Collectable bulundu! Toplama �al��t�.");
                    collectable.Collect();
                }
            }
        }
    }
}
