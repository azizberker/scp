using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour
{
    public GameObject panel; // InventoryPanel
    public GameObject itemButtonPrefab; // Buton prefabı
    public Transform contentParent; // Butonları içine koyacağımız yer

    void Start()
    {
        panel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            panel.SetActive(!panel.activeSelf);
            Debug.Log("Panel Active: " + panel.activeSelf); // 👈 Eklendi
            if (panel.activeSelf)
                RefreshUI();
        }
    }


    void RefreshUI()
    {
        foreach (Transform child in contentParent)
            Destroy(child.gameObject);

        List<ItemDataSO> items = Inventory.Instance.GetItems();

        foreach (ItemDataSO item in items)
        {
            GameObject buttonGO = Instantiate(itemButtonPrefab, contentParent);
            buttonGO.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = item.itemName + " ($" + item.itemValue + ")";
        }
    }
}
