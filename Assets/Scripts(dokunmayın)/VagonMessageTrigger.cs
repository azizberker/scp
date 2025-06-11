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
        // Eğer mesaj zaten gösterildiyse veya PlayerStats yoksa çık
        if (shown || PlayerStats.Instance == null) 
            return;

        // Para miktarını kontrol et
        if (PlayerStats.Instance.Money >= targetMoney)
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
