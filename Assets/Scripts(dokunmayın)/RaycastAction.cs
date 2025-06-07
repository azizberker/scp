using UnityEngine;
using UnityEngine.UI;

public class RaycastAction : MonoBehaviour
{
    private Camera cam;
    private Ray ray;
    private RaycastHit hit;

    [Tooltip("Kapı ve item layer’larını işaretleyin.")]
    public LayerMask interactMask;

    [Header("E Prompt (Screen UI)")]
    public Image collectPrompt; // Canvas>CollectPrompt Image’ını buraya ata

    void Start()
    {
        cam = Camera.main;
        if (collectPrompt != null)
            collectPrompt.enabled = false;
    }

    void Update()
    {
        ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        if (Physics.Raycast(ray, out hit, 3f, interactMask))
        {
            // 1) Kapı açma
            if (hit.collider.TryGetComponent<DoorController>(out var door))
            {
                // (World-space icon için zaten ayrı script çalışıyor)
                if (Input.GetKeyDown(KeyCode.E))
                    door.ToggleDoor();
            }

            // 2) Collectable (fener) kontrolü
            if (hit.collider.TryGetComponent<Collectable>(out var collectable))
            {
                // ekranda prompt göster
                if (collectPrompt != null)
                    collectPrompt.enabled = true;

                if (Input.GetKeyDown(KeyCode.E))
                    collectable.Collect();

                return; // item için bitti
            }
        }

        // Ne kapıya ne fener’e bakıyorsan prompt gizle
        if (collectPrompt != null && collectPrompt.enabled)
            collectPrompt.enabled = false;
    }
}
