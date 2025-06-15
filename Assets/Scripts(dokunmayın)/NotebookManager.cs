using UnityEngine;

public class NotebookManager : MonoBehaviour
{
    public GameObject notebookPanel;         // Tüm not paneli objesi
    public GameObject[] noteImages;          // Inspector’dan sýralý not imgeleri
    public MonoBehaviour playerMovementScript; // Oyuncu hareket scripti (örn. FirstPersonController)
    public MonoBehaviour mouseLookScript;      // Mouse bakýþ scripti (ayrýysa)

    private int currentNoteIndex = 0;
    private bool isNotebookOpen = false;

    void Start()
    {
        notebookPanel.SetActive(false);
        foreach (var img in noteImages)
            img.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            if (!isNotebookOpen)
            {
                OpenNotebook();
            }
            else
            {
                CloseNotebook();
            }
        }
    }

    public void OpenNotebook()
    {
        isNotebookOpen = true;
        notebookPanel.SetActive(true);
        ShowNote(currentNoteIndex);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f; // Oyun dursun

        if (playerMovementScript != null)
            playerMovementScript.enabled = false;
        if (mouseLookScript != null)
            mouseLookScript.enabled = false;
    }

    public void CloseNotebook()
    {
        isNotebookOpen = false;
        notebookPanel.SetActive(false);
        foreach (var img in noteImages)
            img.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f; // Oyun devam etsin

        if (playerMovementScript != null)
            playerMovementScript.enabled = true;
        if (mouseLookScript != null)
            mouseLookScript.enabled = true;
    }

    public void NextNote()
    {
        currentNoteIndex++;
        if (currentNoteIndex >= noteImages.Length)
            currentNoteIndex = 0;
        ShowNote(currentNoteIndex);
    }

    public void PrevNote()
    {
        currentNoteIndex--;
        if (currentNoteIndex < 0)
            currentNoteIndex = noteImages.Length - 1;
        ShowNote(currentNoteIndex);
    }

    void ShowNote(int index)
    {
        for (int i = 0; i < noteImages.Length; i++)
            noteImages[i].SetActive(i == index);
    }
}
