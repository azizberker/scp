using UnityEngine;

[CreateAssetMenu(fileName = "NewItemData", menuName = "Inventory/Item Data")]
public class ItemDataSO : ScriptableObject
{
    public string itemName;
    public int itemValue;
    public Sprite icon;
    public GameObject prefab; // Elde gösterilecek prefab
}
