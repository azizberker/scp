using UnityEngine;

[CreateAssetMenu(fileName = "NewItemData", menuName = "Inventory/Item Data")]
public class ItemDataSO : ScriptableObject
{
    public int price = 10; // Bu item ka� para eder
    public string itemName;
    public int itemValue;
    public Sprite icon;
    public GameObject prefab; // Elde g�sterilecek prefab
}
