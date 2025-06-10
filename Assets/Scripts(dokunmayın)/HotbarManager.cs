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
            slotImages[i].enabled = true;
            if (items[i] != null && items[i].icon != null)
            {
                slotImages[i].sprite = items[i].icon;
                slotImages[i].color = Color.white;
            }
            else
            {
                slotImages[i].sprite = null;
                slotImages[i].color = new Color(1, 1, 1, 0.2f);
            }
            selectionBorders[i].color = (i == selectedIndex) ? new Color(1, 1, 0, 1) : new Color(1, 1, 0, 0);
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
