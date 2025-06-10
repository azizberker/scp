using UnityEngine;

public class Item : Collectable
{
    public ItemDataSO data;

    void Start()
    {
        // Eğer item data'sı yoksa ve parent'ta Item varsa, ondan al
        if (data == null && transform.parent != null)
        {
            var parentItem = transform.parent.GetComponent<Item>();
            if (parentItem != null && parentItem.data != null)
            {
                data = parentItem.data;
            }
        }
    }

    public override void Collect()
    {
        if (data == null)
        {
            Debug.LogError("Item data is missing!");
            return;
        }

        // Hotbar'a eklemeyi dene
        int currentItemCount = 0;
        var hotbar = HotbarManager.Instance;
        
        if (hotbar != null)
        {
            // Mevcut item sayısını kontrol et
            for (int i = 0; i < 4; i++)
            {
                if (hotbar.items[i] != null)
                {
                    currentItemCount++;
                }
            }

            // Eğer yer varsa ekle ve yok et
            if (currentItemCount < 4)
            {
                Debug.Log($"{data.itemName} toplandı!");
                hotbar.AddItemToHotbar(data);
                
                // Eğer bu bir child item ise, parent'ı yok et
                if (transform.parent != null && transform.parent.GetComponent<Item>() != null)
                {
                    Destroy(transform.parent.gameObject);
                }
                else
                {
                    Destroy(gameObject);
                }
            }
            else
            {
                Debug.Log("❌ Hotbar dolu olduğu için item alınamadı!");
            }
        }
    }
}
