using UnityEngine;
using TMPro;

public class VagonMessageTrigger : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI messageText;

    [Header("Settings")]
    [SerializeField] private int targetMoney = 8000;

    private bool shown = false;

    void Start()
    {
        // Text component'ini kontrol et
        if (messageText == null)
        {
            Debug.LogError("Message Text atanmamış! Lütfen inspector'dan atayın!");
            return;
        }

        // Başlangıçta mesajı gizle
        messageText.gameObject.SetActive(false);
    }

    void Update()
    {
        // Eğer mesaj zaten gösterildiyse çık
        if (shown) 
            return;

        // MoneyBar veya PlayerStats üzerinden para kontrolü
        int currentMoney = 0;
        if (MoneyBar.Instance != null)
        {
            currentMoney = MoneyBar.Instance.currentMoney;
        }
        else if (PlayerStats.Instance != null)
        {
            currentMoney = PlayerStats.Instance.Money;
        }

        // Para miktarını kontrol et
        if (currentMoney >= targetMoney)
        {
            ShowMessage();
        }
    }

    private void ShowMessage()
    {
        shown = true;
        messageText.gameObject.SetActive(true);
        Debug.Log($"Para {targetMoney}'e ulaştı! 'Return the VAGON!' mesajı gösteriliyor.");
    }
}
