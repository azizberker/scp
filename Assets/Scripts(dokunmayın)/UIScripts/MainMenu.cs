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
        Debug.Log("Options týklandý");
    }

    public void Credits()
    {
        Debug.Log("Credits týklandý");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Oyun kapatýlýyor..."); // Editör için
    }

    public void TestButton()
    {
        Debug.Log("Buton gerçekten çalýþýyor!");
    }

}
