using UnityEngine;

public class InteractionIconController : MonoBehaviour
{
    [Tooltip("Kap�n�n child�� olarak yerle�tirdi�iniz World Space Canvas > Image (E PNG) objesi")]
    public GameObject interactionIcon;

    [Tooltip("Etkile�im mesafesi")]
    public float interactDistance = 3f;

    private Camera mainCam;
    private Ray ray;
    private RaycastHit hit;

    void Start()
    {
        mainCam = Camera.main;
        if (interactionIcon != null)
            interactionIcon.SetActive(false);
    }

    void Update()
    {
        // Ekran ortas�ndan (0.5,0.5) ileri do�ru ray at
        ray = mainCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        if (Physics.Raycast(ray, out hit, interactDistance))
        {
            // Raycast hangi kap�ya �arpt�?
            var door = hit.collider.GetComponentInParent<DoorController>();
            // E�er bu kap�ysa (script�in tak�l� oldu�u GameObject ile ayn� root ise)
            if (door != null && door.gameObject == this.gameObject)
            {
                interactionIcon.SetActive(true);
                return;
            }
        }

        // Hi�bir ko�ul sa�lanmad�ysa gizle
        if (interactionIcon.activeSelf)
            interactionIcon.SetActive(false);
    }
}
