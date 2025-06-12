using UnityEngine;

public class Flashlight : MonoBehaviour
{
    [SerializeField] private KeyCode toggleKey = KeyCode.F;
    [SerializeField] private Light flashlight;
    [SerializeField] private AudioSource toggleSound;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip onSound;
    [SerializeField] private AudioClip offSound;

    private bool isOn = true;
    private bool isEquipped = false;

    void Start()
    {
        if (flashlight == null)
            flashlight = GetComponentInChildren<Light>();

        if (flashlight != null)
            flashlight.enabled = isOn;
    }

    void Update()
    {
        if (isEquipped && Input.GetKeyDown(toggleKey))
        {
            ToggleFlashlight();
        }
    }

    public void ToggleFlashlight()
    {
        isOn = !isOn;
        if (flashlight != null)
            flashlight.enabled = isOn;

        if (audioSource != null)
        {
            if (isOn && onSound != null)
                audioSource.PlayOneShot(onSound);
            else if (!isOn && offSound != null)
                audioSource.PlayOneShot(offSound);
        }
    }

    public bool IsOn()
    {
        return isOn;
    }

    public void SetEquipped(bool equipped)
    {
        isEquipped = equipped;
        if (!isEquipped && flashlight != null)
        {
            isOn = false;
            flashlight.enabled = false;
        }
    }
}
