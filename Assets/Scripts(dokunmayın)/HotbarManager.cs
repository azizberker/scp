using UnityEngine;
using UnityEngine.UI;

public class HotbarManager : MonoBehaviour
{
    public static HotbarManager Instance; // Singleton

    public Image[] slotImages;
    public Image[] selectionBorders;
    public ItemDataSO[] items = new ItemDataSO[4]; // 🔸 Dizi boyutu 4 oldu

    public Transform handTransform; // FPS karakterindeki "Hand" objesi
    private GameObject currentEquippedObj;
    private int selectedIndex = 0;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        // Slot image'larını otomatik olarak bul
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
                        // Ana slot image'ı değilse (yani yeni eklenen image ise)
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
        if (Input.GetKeyDown(KeyCode.Alpha4)) { SelectSlot(3); EquipItem(3); } // 🔸 Burayı Alpha4 olarak düzelttik
    }

    // Slot image'larını değiştirmek için yeni fonksiyonlar
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
                    // Eğer item yoksa, slotImage'ın kendi sprite'ını koru
                    if (slotImages[i].sprite == null)
                    {
                        slotImages[i].color = new Color(1, 1, 1, 0.2f);
                    }
                }
            }
            if (i < selectionBorders.Length && selectionBorders[i] != null)
            {
                selectionBorders[i].color = (i == selectedIndex) ? new Color(1, 1, 0, 1) : new Color(1, 1, 0, 0);
            }
        }
    }

    public void AddItemToHotbar(ItemDataSO newItem)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == null)
            {
                items[i] = newItem;
                UpdateHotbarUI();
                EquipItem(i);
                return;
            }
        }
    }

    void EquipItem(int index)
    {
        if (currentEquippedObj != null)
            Destroy(currentEquippedObj);

        var itemData = items[index];
        if (itemData != null && itemData.prefab != null && handTransform != null)
        {
            currentEquippedObj = Instantiate(itemData.prefab, handTransform);
            currentEquippedObj.transform.localPosition = Vector3.zero;
            currentEquippedObj.transform.localRotation = Quaternion.identity;
        }
    }
}
