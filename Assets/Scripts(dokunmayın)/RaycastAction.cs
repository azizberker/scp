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

    [Header("UI Elemanları")]
    public TextMeshProUGUI itemPriceText;
    public Image collectPrompt; // Canvas>CollectPrompt Image’ını buraya ata

    void Start()
    {
        cam = Camera.main;

        if (itemPriceText != null)
            itemPriceText.enabled = false;

        if (collectPrompt != null)
            collectPrompt.enabled = false;
    }

    void Update()
    {
        ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        if (Physics.Raycast(ray, out hit, 3f, interactMask))
        {
            // === ITEM kontrolü ===
            if (hit.collider.TryGetComponent<Item>(out var item))
            {
                if (item.data != null)
                {
                    // Fiyat yazısını göster
                    if (itemPriceText != null)
                    {
                        itemPriceText.text = $"{item.data.itemName} - {item.data.price}$";
                        itemPriceText.enabled = true;
                    }

                    // Collect prompt'u göster
                    if (collectPrompt != null)
                        collectPrompt.enabled = true;

                    // E tuşuna basılırsa item alınır
                    if (Input.GetKeyDown(KeyCode.E))
                        item.Collect();
                }

                return; // item varsa başka kontrol gerekmez
            }

            // === COLLECTABLE kontrolü (Item olmayan toplanabilirler için) ===
            if (hit.collider.TryGetComponent<Collectable>(out var collectable))
            {
                if (collectPrompt != null)
                    collectPrompt.enabled = true;

                if (itemPriceText != null)
                    itemPriceText.enabled = false;

                if (Input.GetKeyDown(KeyCode.E))
                    collectable.Collect();

                return;
            }

            // === DOOR kontrolü ===
            if (hit.collider.TryGetComponent<DoorController>(out var door))
            {
                if (itemPriceText != null)
                    itemPriceText.enabled = false;

                if (collectPrompt != null)
                    collectPrompt.enabled = false;

                if (Input.GetKeyDown(KeyCode.E))
                    door.ToggleDoor();

                return;
            }
        }

        // === Hiçbir şey vurulmadıysa UI'ları kapat ===
        if (itemPriceText != null)
            itemPriceText.enabled = false;

        if (collectPrompt != null)
            collectPrompt.enabled = false;
    }
}
