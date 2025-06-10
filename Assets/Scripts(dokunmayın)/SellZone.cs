using UnityEngine;

public class SellZone : MonoBehaviour
{
    private bool playerInZone = false;
    private float holdTime = 0f;
    private float requiredHoldTime = 1.5f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = true;
            Debug.Log("▶ Oyuncu satış alanına GİRDİ");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = false;
            holdTime = 0f;
            Debug.Log("◀ Oyuncu satış alanından ÇIKTI");
        }
    }

    void Update()
    {
        if (!playerInZone)
        {
            return;
        }

        var hotbar = HotbarManager.Instance;
        var item = hotbar != null ? hotbar.GetSelectedItem() : null;

        if (item == null)
        {
            Debug.Log("❌ Hotbar'da seçili item yok → Satış yapılmaz");
            return;
        }

        if (Input.GetKey(KeyCode.E))
        {
            holdTime += Time.deltaTime;
            Debug.Log($"🕐 E tuşuna basılıyor... {holdTime:F2} / {requiredHoldTime}s");

            if (holdTime >= requiredHoldTime)
            {
                Debug.Log($"💰 Satıldı: {item.itemName} +{item.price}₺");

                MoneyBar.Instance?.AddMoney(item.price);
                hotbar.RemoveSelectedItem();

                holdTime = 0f;
            }
        }
        else
        {
            if (holdTime > 0f)
                Debug.Log("🛑 E tuşu bırakıldı → Sayaç sıfırlandı");

            holdTime = 0f;
        }
    }
}
