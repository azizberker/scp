using UnityEngine;

public class InteractionIconController : MonoBehaviour
{
    [Tooltip("Kapýnýn child’ý olarak yerleþtirdiðiniz World Space Canvas > Image (E PNG) objesi")]
    public GameObject interactionIcon;

    [Tooltip("Etkileþim mesafesi")]
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
        // Ekran ortasýndan (0.5,0.5) ileri doðru ray at
        ray = mainCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        if (Physics.Raycast(ray, out hit, interactDistance))
        {
            // Raycast hangi kapýya çarptý?
            var door = hit.collider.GetComponentInParent<DoorController>();
            // Eðer bu kapýysa (script’in takýlý olduðu GameObject ile ayný root ise)
            if (door != null && door.gameObject == this.gameObject)
            {
                interactionIcon.SetActive(true);
                return;
            }
        }

        // Hiçbir koþul saðlanmadýysa gizle
        if (interactionIcon.activeSelf)
            interactionIcon.SetActive(false);
    }
}
