using UnityEngine;

public class NoteTrigger : MonoBehaviour
{
    [TextArea(3, 10)]
    public string noteContent;
    
    private bool hasBeenRead = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!hasBeenRead && other.CompareTag("Player"))
        {
            ShowNote();
        }
    }

    private void ShowNote()
    {
        if (NoteUIController.Instance != null)
        {
            NoteUIController.Instance.ShowNote(noteContent, this);
        }
    }

    public void OnNoteClosed()
    {
        hasBeenRead = true;
    }
} 