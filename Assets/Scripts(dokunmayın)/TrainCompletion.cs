using UnityEngine;
using TMPro;

public class TrainCompletion : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI promptText;
    public GameObject ePrompt;

    [Header("Settings")]
    public float interactionDistance = 3f;
    public string menuSceneName = "pist"; // Ana sahneye dönüş yapacak

    private Transform playerCam;

    void Start()
    {
        playerCam = Camera.main.transform;
        HidePrompts();
    }

    void Update()
    {
        if (playerCam == null) return;

        // Para kontrolü
        bool hasSufficientMoney = MoneyBar.Instance != null && MoneyBar.Instance.currentMoney >= 8000;
        
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
        Debug.Log("🎮 Oyun tamamlandı! Ana sahneye dönülüyor...");
        UnityEngine.SceneManagement.SceneManager.LoadScene(menuSceneName);
    }
} 