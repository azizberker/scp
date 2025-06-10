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

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

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
                            break;
                        }
                    }
                }
            }
        }
    }

    void Start()
    {
        UpdateHotbarUI();
        EquipItem(selectedIndex);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) { SelectSlot(0); EquipItem(0); }
        if (Input.GetKeyDown(KeyCode.Alpha2)) { SelectSlot(1); EquipItem(1); }
        if (Input.GetKeyDown(KeyCode.Alpha3)) { SelectSlot(2); EquipItem(2); }
        if (Input.GetKeyDown(KeyCode.Alpha4)) { SelectSlot(3); EquipItem(3); }
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
            slotImages[slotIndex].sprite = null;
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
                    if (slotImages[i].sprite == null)
                    {
                        slotImages[i].color = new Color(1, 1, 1, 0.2f);
                    }
                }
            }
            if (i < selectionBorders.Length && selectionBorders[i] != null)
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
            Debug.Log($"Destroying current equipped object: {currentEquippedObj.name}");
            Destroy(currentEquippedObj);
        }

        var itemData = items[index];
        if (itemData == null)
        {
            Debug.Log($"No item data found for slot {index}");
            return;
        }

        if (itemData.prefab == null)
        {
            Debug.LogError($"Item {itemData.name} has no prefab assigned!");
            return;
        }

        if (handTransform == null)
        {
            Debug.LogError("Hand Transform is not assigned! Please assign it in the Unity Inspector.");
            return;
        }

        Debug.Log($"Attempting to equip item: {itemData.name}");
        currentEquippedObj = Instantiate(itemData.prefab, handTransform);
        currentEquippedObj.transform.localPosition = Vector3.zero;
        currentEquippedObj.transform.localRotation = Quaternion.identity;
        Debug.Log($"Successfully equipped item: {currentEquippedObj.name} at position {currentEquippedObj.transform.position}");
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
            Destroy(currentEquippedObj);

        items[selectedIndex] = null;
        UpdateHotbarUI();
    }
}
