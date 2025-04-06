using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;

    private List<ItemDataSO> collectedItems = new List<ItemDataSO>();

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void AddItem(ItemDataSO item)
    {
        collectedItems.Add(item);
    }

    public int SellAllItems()
    {
        int total = 0;
        foreach (var item in collectedItems)
        {
            total += item.itemValue;
            Debug.Log($"Sold {item.itemName} for {item.itemValue}");
        }

        PlayerStats.Instance.AddMoney(total);
        collectedItems.Clear();
        return total;
    }

    public List<ItemDataSO> GetItems()
    {
        return collectedItems;
    }
}
