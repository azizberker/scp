using UnityEngine;
using TMPro;

public class NoteUIController : MonoBehaviour
{
    public static NoteUIController Instance;
    public GameObject notePanel;
    public TextMeshProUGUI noteText;
    private NoteTrigger currentNote;

    void Awake()
    {
        Instance = this;
        if (notePanel != null)
            notePanel.SetActive(false);
    }

    public void ShowNote(string content, NoteTrigger trigger)
    {
        Time.timeScale = 0f;
        notePanel.SetActive(true);
        noteText.text = content;
        currentNote = trigger;
    }

    public void HideNote()
    {
        Time.timeScale = 1f;
        notePanel.SetActive(false);
        currentNote?.OnNoteClosed();
    }
}
