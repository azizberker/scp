using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SellZoneUI : MonoBehaviour
{
    public static SellZoneUI Instance;

    public GameObject root;
    public Image eIcon;
    public TextMeshProUGUI holdText;
    public Image progressCircle;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        HideUI();
    }

    public void ShowUI()
    {
        if (root != null)
            root.SetActive(true);

        if (progressCircle != null)
            progressCircle.fillAmount = 1f;
    }

    public void UpdateProgress(float fill)
    {
        if (progressCircle != null)
            progressCircle.fillAmount = Mathf.Clamp01(fill);
    }

    public void HideUI()
    {
        if (root != null)
            root.SetActive(false);
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
}
