using System.Collections.Generic;
using UnityEngine;


public class Item : Collectable
{
    public ItemDataSO data;

    public override void Collect()
    {
        Inventory.Instance.AddItem(data);
        Debug.Log($"{data.itemName} collected! Value: {data.itemValue}");
        base.Collect();
    }
}


