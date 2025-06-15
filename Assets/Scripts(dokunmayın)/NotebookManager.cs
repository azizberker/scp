using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using TMPro; // TMP InputField kullanacaksan

public class NotebookManager : MonoBehaviour
{
    public GameObject notebookPanel; // Paneli Inspector�dan atayacaks�n
    public TMP_InputField notebookInput; // Input Field

    private bool isNotebookOpen = false;

    void Start()
    {
        if (notebookInput != null)
            notebookInput.text = PlayerPrefs.GetString("NotebookContent", "");
    }

    void OnDisable()
    {
        if (notebookInput != null)
            PlayerPrefs.SetString("NotebookContent", notebookInput.text);
    }

    void Update()
    {
        // N tu�una bas�nca a�/kapat
        if (Input.GetKeyDown(KeyCode.N))
        {
            isNotebookOpen = !isNotebookOpen;
            notebookPanel.SetActive(isNotebookOpen);

            // A��ld���nda otomatik imleci al
            if (isNotebookOpen && notebookInput != null)
                notebookInput.ActivateInputField();

            // Not defteri a��kken mouse ve imle� serbest olsun!
            if (isNotebookOpen)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                // Oyun duracaksa: Time.timeScale = 0f;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                // Time.timeScale = 1f;
            }
        }
    }
    public void CloseNotebook()
    {
        isNotebookOpen = false;
        notebookPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        // Time.timeScale = 1f;
    }
}
