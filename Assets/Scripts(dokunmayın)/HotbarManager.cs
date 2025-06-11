using UnityEngine;
using UnityEngine.UI;

public class HotbarManager : MonoBehaviour
{
    public static HotbarManager Instance;

    public Image[] slotImages;
    public Image[] selectionBorders;
    public ItemDataSO[] items = new ItemDataSO[4];

    public Transform handTransform;
    private GameObject currentEquippedObj;
    private int selectedIndex = 0;

    [SerializeField] private Sprite defaultSlotSprite; // Rectangle 193 için

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        // Önce default sprite'ı kontrol et
        if (defaultSlotSprite == null)
        {
            Debug.LogError("Default Slot Sprite atanmamış! Lütfen Unity Inspector'da atayın!");
            return;
        }

        Transform hotbarPanel = transform.Find("HotbarPanel");
        if (hotbarPanel != null)
        {
            slotImages = new Image[4];
            for (int i = 0; i < 4; i++)
            {
                Transform slotTransform = hotbarPanel.Find($"Slot{i + 1}");
                if (slotTransform != null)
                {
                    Image[] images = slotTransform.GetComponentsInChildren<Image>();
                    foreach (Image img in images)
                    {
                        if (img.transform != slotTransform)
                        {
                            slotImages[i] = img;
                            EnsureSlotHasSprite(i); // Her slot için sprite kontrolü
                            break;
                        }
                    }
                }
            }
        }
    }

    // Slot'un sprite'ını kontrol eden ve düzelten fonksiyon
    private void EnsureSlotHasSprite(int index)
    {
        if (slotImages[index] != null)
        {
            if (slotImages[index].sprite == null)
            {
                slotImages[index].sprite = defaultSlotSprite;
            }
            slotImages[index].color = new Color(1, 1, 1, 0.2f);
        }
    }

    void Start()
    {
        UpdateHotbarUI();
        EquipItem(selectedIndex);
    }

    void Update()
    {
        // Hotbar slot seçimleri
        if (Input.GetKeyDown(KeyCode.Alpha1)) { SelectSlot(0); EquipItem(0); }
        if (Input.GetKeyDown(KeyCode.Alpha2)) { SelectSlot(1); EquipItem(1); }
        if (Input.GetKeyDown(KeyCode.Alpha3)) { SelectSlot(2); EquipItem(2); }
        if (Input.GetKeyDown(KeyCode.Alpha4)) { SelectSlot(3); EquipItem(3); }

        // Flashlight kontrolü - sadece elimizde flashlight varsa çalışsın
        if (Input.GetKeyDown(KeyCode.F) && items[selectedIndex] != null)
        {
            if (items[selectedIndex].itemName.ToLower().Contains("flashlight"))
            {
                // Flashlight'ı bul ve aç/kapa
                Light flashlightComponent = currentEquippedObj?.GetComponentInChildren<Light>();
                if (flashlightComponent != null)
                {
                    flashlightComponent.enabled = !flashlightComponent.enabled;
                }
            }
        }
    }

    public void ChangeSlotImage(int slotIndex, Sprite newSprite)
    {
        if (slotIndex >= 0 && slotIndex < slotImages.Length && slotImages[slotIndex] != null)
        {
            slotImages[slotIndex].sprite = newSprite;
            slotImages[slotIndex].color = Color.white;
        }
    }

    public void ChangeAllSlotImages(Sprite newSprite)
    {
        for (int i = 0; i < slotImages.Length; i++)
        {
            if (slotImages[i] != null)
            {
                slotImages[i].sprite = newSprite;
                slotImages[i].color = Color.white;
            }
        }
    }

    public void ResetSlotImage(int slotIndex)
    {
        if (slotIndex >= 0 && slotIndex < slotImages.Length && slotImages[slotIndex] != null)
        {
            slotImages[slotIndex].sprite = defaultSlotSprite; // Rectangle 193'e geri dön
            slotImages[slotIndex].color = new Color(1, 1, 1, 0.2f);
        }
    }

    void SelectSlot(int index)
    {
        if (index >= 0 && index < items.Length)
        {
            selectedIndex = index;
            UpdateHotbarUI();
        }
    }

    void UpdateHotbarUI()
    {
        for (int i = 0; i < slotImages.Length; i++)
        {
            if (slotImages[i] != null)
            {
                slotImages[i].enabled = true;
                if (items[i] != null && items[i].icon != null)
                {
                    slotImages[i].sprite = items[i].icon;
                    slotImages[i].color = Color.white;
                }
                else
                {
                    slotImages[i].sprite = defaultSlotSprite;
                    slotImages[i].color = new Color(1, 1, 1, 0.2f);
                }
            }
        }

        // Seçili slot için border rengini güncelle
        for (int i = 0; i < selectionBorders.Length; i++)
        {
            if (selectionBorders[i] != null)
            {
                selectionBorders[i].color = (i == selectedIndex)
                    ? new Color(1, 1, 0, 1)
                    : new Color(1, 1, 0, 0);
            }
        }
    }

    public void AddItemToHotbar(ItemDataSO newItem)
    {
        // Önce boş slot var mı kontrol et
        bool hasEmptySlot = false;
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == null)
            {
                hasEmptySlot = true;
                break;
            }
        }

        // Eğer boş slot yoksa, item'ı alma
        if (!hasEmptySlot)
        {
            Debug.Log("❌ Hotbar dolu! Daha fazla item alınamaz.");
            return;
        }

        // Boş slot varsa item'ı ekle
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == null)
            {
                items[i] = newItem;
                UpdateHotbarUI();
                EquipItem(i);
                Debug.Log($"✅ {newItem.itemName} hotbar'a eklendi. Slot: {i + 1}");
                return;
            }
        }
    }

    void EquipItem(int index)
    {
        if (currentEquippedObj != null)
        {
            Destroy(currentEquippedObj);
        }

        var itemData = items[index];
        if (itemData == null || itemData.prefab == null || handTransform == null)
            return;

        currentEquippedObj = Instantiate(itemData.prefab, handTransform);
        currentEquippedObj.transform.localPosition = Vector3.zero;
        currentEquippedObj.transform.localRotation = Quaternion.identity;

        // 🔦 Flashlight ise UI göster
        if (itemData.itemName.ToLower().Contains("flashlight"))
            ItemTipsUI.Instance?.ShowFlashlightTip(true);
        else
            ItemTipsUI.Instance?.ShowFlashlightTip(false);

        // 📦 Her item için "Drop Item" göster
        ItemTipsUI.Instance?.ShowDropTip(true);
    }

    // 🔻 Satış alanı için eklendi:
    public ItemDataSO GetSelectedItem()
    {
        if (selectedIndex >= 0 && selectedIndex < items.Length)
            return items[selectedIndex];
        return null;
    }

    public void RemoveSelectedItem()
    {
        if (currentEquippedObj != null)
        {
            Destroy(currentEquippedObj);
        }

        // UI ipuçlarını gizle
        ItemTipsUI.Instance?.ShowDropTip(false);
        
        // Eğer flashlight drop edildiyse onun ipucunu da gizle
        if (items[selectedIndex] != null && items[selectedIndex].itemName.ToLower().Contains("flashlight"))
        {
            ItemTipsUI.Instance?.ShowFlashlightTip(false);
        }

        items[selectedIndex] = null;
        EnsureSlotHasSprite(selectedIndex);
        UpdateHotbarUI();
    }
}
