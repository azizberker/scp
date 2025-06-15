using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TrainCompletion : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI promptText;
    public GameObject ePrompt;

    [Header("Settings")]
    public float interactionDistance = 3f;
    public string menuSceneName = "pist"; // Ana sahneye dönüş yapacak

    private Transform playerCam;

    public GameObject winPanel;   // Inspector’dan paneli ata
    public float winScreenDuration = 6f;


    void Start()
    {
        playerCam = Camera.main.transform;
        HidePrompts();
    }

    void Update()
    {
        if (playerCam == null) return;

        bool isPist2 = SceneManager.GetActiveScene().name == "pist2";

        bool hasSufficientMoney = isPist2 || (MoneyBar.Instance != null && MoneyBar.Instance.currentMoney >= 8000);
    

        Ray ray = new Ray(playerCam.position, playerCam.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, interactionDistance))
        {
            if (hit.collider.gameObject == gameObject) // Trene bakıyorsa
            {
                // Sadece yeterli para varsa UI'ı göster
                if (hasSufficientMoney)
                {
                    ShowPrompts();

                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        CompleteGame();
                    }
                }
                else
                {
                    HidePrompts();
                }
            }
            else
            {
                HidePrompts();
            }
        }
        else
        {
            HidePrompts();
        }
    }

    void ShowPrompts()
    {
        if (promptText != null)
        {
            promptText.gameObject.SetActive(true);
            promptText.text = "Tamamla";
        }
        
        if (ePrompt != null)
            ePrompt.SetActive(true);
    }

    void HidePrompts()
    {
        if (promptText != null)
            promptText.gameObject.SetActive(false);
            
        if (ePrompt != null)
            ePrompt.SetActive(false);
    }

    

    void CompleteGame()
    {
        Debug.Log("🎮 Oyun tamamlandı! Kazandınız ekranı gösterilecek.");
        HidePrompts();

        // Kazandınız panelini göster
        if (winPanel != null)
            winPanel.SetActive(true);

        // Mouse'u serbest bırak, oyunu dondur (opsiyonel)
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;

        // 10 saniye bekleyip ana menüye dön
        StartCoroutine(WinScreenCoroutine());
    }

    System.Collections.IEnumerator WinScreenCoroutine()
    {
        // Time.timeScale = 0 olduğu için gerçek zamanlı bekle!
        yield return new WaitForSecondsRealtime(winScreenDuration);

        Time.timeScale = 1f;
        SceneManager.LoadScene(menuSceneName);
    }

}