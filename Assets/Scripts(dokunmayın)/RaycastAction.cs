using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RaycastAction : MonoBehaviour
{
    private Camera cam;
    private Ray ray;
    private RaycastHit hit;

    [Tooltip("Kapı ve item layer'larını işaretleyin.")]
    public LayerMask interactMask;

    [Header("UI Elemanları")]
    public TextMeshProUGUI itemPriceText;
    public Image collectPrompt; // Canvas>CollectPrompt Image'ını buraya ata

    [Header("Item Bırakma Ayarları")]
    public float dropForce = 5f; // İtem bırakma kuvveti
    public float dropUpwardForce = 2f; // Yukarı doğru kuvvet
    public float dropDistance = 2f; // Karakterin önünde bırakma mesafesi

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
        // Item bırakma kontrolü
        if (Input.GetKeyDown(KeyCode.G))
        {
            DropCurrentItem();
            return;
        }

        ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        if (Physics.Raycast(ray, out hit, 3f, interactMask))
        {
            // === ITEM kontrolü ===
            Item item = hit.collider.GetComponent<Item>();
            
            // Eğer vurulan objede Item yoksa, parent'ında ara
            if (item == null && hit.collider.transform.parent != null)
            {
                item = hit.collider.transform.parent.GetComponent<Item>();
            }

            if (item != null && item.data != null)
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

    private void DropCurrentItem()
    {
        var hotbar = HotbarManager.Instance;
        if (hotbar == null) return;

        var currentItem = hotbar.GetSelectedItem();
        if (currentItem == null)
        {
            Debug.Log("❌ Bırakılacak item yok!");
            return;
        }

        // Boş bir GameObject oluştur
        GameObject droppedItem = new GameObject($"DroppedItem_{currentItem.itemName}");
        droppedItem.transform.position = cam.transform.position + cam.transform.forward * dropDistance;

        // Prefab'ı child olarak ekle
        GameObject itemModel = Instantiate(currentItem.prefab, droppedItem.transform);
        itemModel.transform.localPosition = Vector3.zero;
        itemModel.transform.localRotation = Quaternion.identity;

        // Item bileşenini ana objeye ekle
        Item itemComponent = droppedItem.AddComponent<Item>();
        itemComponent.data = currentItem;

        // BoxCollider ekle
        BoxCollider collider = droppedItem.AddComponent<BoxCollider>();
        collider.size = new Vector3(0.5f, 0.5f, 0.5f);
        collider.isTrigger = false;

        // Rigidbody ekle
        Rigidbody rb = droppedItem.AddComponent<Rigidbody>();
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

        // Layer'ları ayarla (6 numaralı layer'ı kullan)
        droppedItem.layer = 6;
        // Child objenin ve tüm alt objelerinin layer'ını ayarla
        foreach (Transform child in droppedItem.GetComponentsInChildren<Transform>(true))
        {
            child.gameObject.layer = 6;
        }
        
        // İleri ve yukarı doğru kuvvet uygula
        Vector3 dropDirection = cam.transform.forward + Vector3.up * dropUpwardForce;
        rb.AddForce(dropDirection.normalized * dropForce, ForceMode.Impulse);

        // Hotbar'dan item'ı kaldır
        hotbar.RemoveSelectedItem();
        Debug.Log($"✅ {currentItem.itemName} yere bırakıldı!");
    }
}
