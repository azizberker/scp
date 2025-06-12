using UnityEngine;
using System.Collections;

public class DoorController : MonoBehaviour
{
    public GameObject interactionIcon; // World Space Canvas veya sadece Image objesi

    private bool isOpen = true;
    private float openAngle = 90f;
    private float closedAngle = 0f;
    private float speed = 5f;
    public AudioClip openSound;
    public AudioClip closeSound;
    private AudioSource audioSource;

    public float openSoundStartTime = 0.05f;  // Açýlma sesinde kaçýncý saniyeden baþlasýn
    public float closeSoundStartTime = 0.05f; // Kapanma sesinde kaçýncý saniyeden baþlasýn
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        float targetAngle = isOpen ? openAngle : closedAngle;
        transform.localRotation = Quaternion.Lerp(
            transform.localRotation,
            Quaternion.Euler(0, targetAngle, 0),
            Time.deltaTime * speed
        );
    }
    private void PlayOpenSound()
    {
        audioSource.clip = openSound;
        audioSource.time = openSoundStartTime;
        audioSource.Play();
    }

    private void PlayCloseSound()
    {
        audioSource.clip = closeSound;
        audioSource.time = closeSoundStartTime;
        audioSource.Play();
    }

    public void ToggleDoor()
    {
        isOpen = !isOpen;

        if (audioSource != null)
        {
            if (isOpen && openSound != null)
                PlayOpenSound(); // Direkt çal, gerekirse gecikmeli olarak Invoke ile de çaðýrabilirsin
            else if (!isOpen && closeSound != null)
                PlayCloseSound();
        }
    }

    public void ShowIcon(bool show)
    {
        if (interactionIcon != null)
            interactionIcon.SetActive(show);
    }
}

