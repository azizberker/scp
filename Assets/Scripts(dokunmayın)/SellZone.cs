using UnityEngine;

public class SellZone : MonoBehaviour
{
    private bool playerInRange = false;
    private float holdTime = 0f;
    private float requiredHoldTime = 0.5f;
    public float detectionRange = 5f; // Algılama mesafesi (Unity'de ayarlanabilir)
    private Transform playerTransform;

    void Start()
    {
        // Oyun başladığında Player tag'ine sahip objeyi bul
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    void Update()
    {
        if (playerTransform == null) return;

        // Oyuncu ile SellZone arasındaki mesafeyi hesapla
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
        
        // Oyuncu menzil içinde mi kontrol et
        playerInRange = distanceToPlayer <= detectionRange;

        if (!playerInRange)
        {
            if (holdTime > 0f)
                Debug.Log("◀ Oyuncu satış alanından ÇIKTI");
            holdTime = 0f;
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

    // Gizmos ile Unity editöründe algılama mesafesini görselleştir
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
