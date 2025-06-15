using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    public GameObject agreementPanel;            // Tiksiz panel
    public GameObject agreementAcceptedPanel;    // Tikli panel
    public float fadeDuration = 1.5f;            // Fade-out süresi
    public float waitDuration = 3f;              // Tikli panelin bekleme süresi

    private bool agreementAccepted = false;
    private CanvasGroup acceptedPanelCanvasGroup;

    public GameObject mainMenuPanel; // Inspector’da ana menü panelini buraya ata
    public GameObject mainMenuGroup; // Inspector’dan yeni grubu ata

    void Start()
    {
        if (agreementPanel != null)
            agreementPanel.SetActive(false);

        if (agreementAcceptedPanel != null)
        {
            agreementAcceptedPanel.SetActive(false);

            acceptedPanelCanvasGroup = agreementAcceptedPanel.GetComponent<CanvasGroup>();
            if (acceptedPanelCanvasGroup == null)
                acceptedPanelCanvasGroup = agreementAcceptedPanel.AddComponent<CanvasGroup>();

            acceptedPanelCanvasGroup.alpha = 1f;
        }
    }

    public void StartGame()
    {
        if (!agreementAccepted)
        {
            if (agreementPanel != null)
                agreementPanel.SetActive(true);
            return;
        }
        SceneManager.LoadScene(1);
    }

    public void OnAcceptAgreement()
    {
        agreementAccepted = true;

        if (agreementPanel != null)
            agreementPanel.SetActive(false);

        if (agreementAcceptedPanel != null)
        {
            agreementAcceptedPanel.SetActive(true);

            if (acceptedPanelCanvasGroup != null)
            {
                acceptedPanelCanvasGroup.alpha = 1f;
                StartCoroutine(WaitAndFadeOutPanel());
            }
            else
            {
                // CanvasGroup yoksa hemen geç
                SceneManager.LoadScene(1);
            }
        }
    }

    private IEnumerator WaitAndFadeOutPanel()
    {
        // Ana menü panelini hemen gizle!
        if (mainMenuGroup != null)
            mainMenuGroup.SetActive(false);


        // Sonra 3 saniye bekle
        yield return new WaitForSecondsRealtime(waitDuration);

        // Sonra tikli panelin fade-out'u devam etsin
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / fadeDuration);
            acceptedPanelCanvasGroup.alpha = 1f - t;
            yield return null;
        }
        acceptedPanelCanvasGroup.alpha = 0f;

        // Sonunda sahneyi değiştir
        SceneManager.LoadScene(1);
    }

    public void Options()
    {
        Debug.Log("Options tıklandı");
    }

    public void Credits()
    {
        Debug.Log("Credits tıklandı");
        SceneManager.LoadScene("Credits");
    }

    public void QuitGame()
    {
        Debug.Log("Oyun kapatılıyor...");
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
