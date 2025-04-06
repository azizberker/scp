using UnityEngine;

[CreateAssetMenu(fileName = "New ItemData", menuName = "Item Data")]
public class ItemDataSO : ScriptableObject
{
    public string itemName;
    public int itemValue;
}

