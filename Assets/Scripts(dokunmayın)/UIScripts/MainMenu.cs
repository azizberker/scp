using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene(1); 
    }

    public void Options()
    {
        Debug.Log("Options tıklandı");
    }

    public void Credits()
    {
        Debug.Log("Credits tıklandı");
        SceneManager.LoadScene("Credits"); // ← Sahne ismini buraya yazdık
    }

    public void QuitGame()
    {
        Debug.Log("Oyun kapatılıyor...");
        Application.Quit(); // ✅ Derlenmiş oyunda çalışır

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // 🎯 Editör içindeysen burası oyunu durdurur
#endif
    }
}

