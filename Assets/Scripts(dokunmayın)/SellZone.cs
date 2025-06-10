using UnityEngine;

public class SellZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Oyuncunun elindeki item'i sat
            HotbarManager hotbar = HotbarManager.Instance;
            if (hotbar != null && hotbar.GetSelectedItem() != null)
            {
                var item = hotbar.GetSelectedItem();
                Debug.Log($"Satıldı: {item.itemName} → {item.price}₺");

                // Parayı artır
                MoneyBar.Instance.AddMoney(item.price);

                // Hotbar'dan kaldır
                hotbar.RemoveSelectedItem();
            }
        }
    }
}
