using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RaycastAction : MonoBehaviour
{
    private Camera cam;
    private Ray ray;
    private RaycastHit hit;

    [Tooltip("Kapı ve item layer’larını işaretleyin.")]
    public LayerMask interactMask;

    public TextMeshProUGUI itemPriceText;

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

        if (Physics.Raycast(ray, out hit, 3f, interactMask))
        {
            // Item'a bakılıyorsa fiyatı yaz
            if (hit.collider.TryGetComponent<Item>(out var item))
            {
                if (item.data != null && itemPriceText != null)
                {
                    itemPriceText.text = $"{item.data.itemName} - {item.data.price}₺";
                    itemPriceText.enabled = true;
                }

                // E tuşuna basıldıysa toplama işlemi vs.
                if (Input.GetKeyDown(KeyCode.E))
                {
                    item.Collect();
                }
                return;
            }

            // Kapıya bakılıyorsa gizle fiyatı
            if (itemPriceText != null)
                itemPriceText.enabled = false;

            // Diğer raycast işlemleri...
        }
        else
        {
            if (itemPriceText != null)
                itemPriceText.enabled = false;
        }


        // Ne kapıya ne fener’e bakıyorsan prompt gizle
        if (collectPrompt != null && collectPrompt.enabled)
            collectPrompt.enabled = false;

        Debug.DrawRay(ray.origin, ray.direction * 3f, Color.green);

        if (hit.collider.TryGetComponent<Item>(out var item))
        {
            Debug.Log($"Item bulundu: {item.name}");

            if (item.data != null && itemPriceText != null)
            {
                itemPriceText.text = $"{item.data.itemName} - {item.data.price}₺";
                itemPriceText.enabled = true;
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                item.Collect();
            }

            return;
        }
    }
}

