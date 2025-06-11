using UnityEngine;

public class SellZone : MonoBehaviour
{
    private bool playerInRange = false;
    private float holdTime = 0f;
    private float requiredHoldTime = 0.5f;
    public float detectionRange = 5f;
    private Transform playerTransform;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    void Update()
    {
        if (playerTransform == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
        playerInRange = distanceToPlayer <= detectionRange;

        var hotbar = HotbarManager.Instance;
        var item = hotbar != null ? hotbar.GetSelectedItem() : null;

        // Her karede önce UI'yi kapat
        SellZoneUI.Instance?.HideUI();

        // Menzilde değilsen veya item yoksa hiçbir şey yapma
        if (!playerInRange || item == null)
        {
            if (holdTime > 0f)
                Debug.Log("🛑 Satış iptal → Uzaklaşıldı veya item yok");
            holdTime = 0f;
            return;
        }

        // Artık uygun → UI’yi göster
        SellZoneUI.Instance?.ShowUI();

        if (Input.GetKey(KeyCode.E))
        {
            holdTime += Time.deltaTime;
            Debug.Log($"🕐 E tuşuna basılıyor... {holdTime:F2} / {requiredHoldTime}s");

            float progress = 1f - (holdTime / requiredHoldTime);
            SellZoneUI.Instance?.UpdateProgress(progress);

            if (holdTime >= requiredHoldTime)
            {
                Debug.Log($"💰 Satıldı: {item.itemName} +{item.price}₺");

                MoneyBar.Instance?.AddMoney(item.price);
                hotbar.RemoveSelectedItem();

                holdTime = 0f;
                SellZoneUI.Instance?.HideUI();
            }
        }
        else
        {
            if (holdTime > 0f)
                Debug.Log("🛑 E tuşu bırakıldı → Sayaç sıfırlandı");

            holdTime = 0f;
            SellZoneUI.Instance?.UpdateProgress(1f);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
