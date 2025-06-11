using UnityEngine;
using TMPro;

public class VagonMessageTrigger : MonoBehaviour
{
    public GameObject messageTextObject; // ReturnTheVagonText objesi
    public int targetMoney = 8000;

    private bool shown = false;

    void Update()
    {
        if (shown || PlayerStats.Instance == null) return;

        if (PlayerStats.Instance.Money >= targetMoney)
        {
            shown = true;
            if (messageTextObject != null)
                messageTextObject.SetActive(true);

            Debug.Log("🚨 Vagon mesajı gösterildi!");
        }
    }
}
