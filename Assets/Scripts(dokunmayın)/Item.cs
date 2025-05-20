using UnityEngine;

public class Item : Collectable
{
    public ItemDataSO data;

    public override void Collect()
    {
        Debug.Log($"{data.itemName} toplandý!");
        HotbarManager.Instance.AddItemToHotbar(data); // Hotbara ekle
        base.Collect();
    }
}
