using UnityEngine;

public class RaycastAction : MonoBehaviour
{
    private Camera cam;
    private Ray ray;
    private RaycastHit hit;
    public LayerMask interactMask; // Inspector’dan sadece Item layer’ý seçili olacak

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
                if (hit.collider.TryGetComponent<Collectable>(out var collectable))
                {
                    collectable.Collect();
                }
            }
        }
    }
}
