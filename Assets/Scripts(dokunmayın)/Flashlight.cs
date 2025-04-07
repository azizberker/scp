using UnityEngine;

public class Flashlight : MonoBehaviour
{
    [SerializeField] private KeyCode toggleKey = KeyCode.F;
    [SerializeField] private Light flashlight;
    [SerializeField] private AudioSource toggleSound;

    private bool isOn = true;

    void Start()
    {
        if (flashlight == null)
            flashlight = GetComponentInChildren<Light>();

        if (flashlight != null)
            flashlight.enabled = isOn;
    }

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            ToggleFlashlight();
        }
    }

    public void ToggleFlashlight()
    {
        isOn = !isOn;
        if (flashlight != null)
            flashlight.enabled = isOn;

        if (toggleSound != null)
            toggleSound.Play();
    }

    public bool IsOn()
    {
        return isOn;
    }
}
