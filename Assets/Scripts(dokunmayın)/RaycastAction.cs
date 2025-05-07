using UnityEngine;

public class RaycastAction : MonoBehaviour
{
    private Camera cam;
    private Ray ray;
    private RaycastHit hit;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        if (Physics.Raycast(ray, out hit, 3f)) // mesafe: 3 metre
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (hit.collider.TryGetComponent<DoorController>(out var door))
                {
                    door.ToggleDoor();
                }
            }
        }
    }
}
