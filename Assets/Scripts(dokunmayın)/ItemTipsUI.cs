using UnityEngine;
using TMPro;

public class ItemTipsUI : MonoBehaviour
{
    public static ItemTipsUI Instance;

    public TextMeshProUGUI flashlightTip;
    public TextMeshProUGUI dropTip;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        if (flashlightTip != null)
            flashlightTip.enabled = false;

        if (dropTip != null)
            dropTip.enabled = false;
    }

    public void ShowFlashlightTip(bool show)
    {
        if (flashlightTip != null)
            flashlightTip.enabled = show;
    }

    public void ShowDropTip(bool show)
    {
        if (dropTip != null)
            dropTip.enabled = show;
    }
}
