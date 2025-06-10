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
        Debug.Log("Options t�kland�");
    }

    public void Credits()
    {
        Debug.Log("Credits t�kland�");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Oyun kapat�l�yor..."); // Edit�r i�in
    }

    public void TestButton()
    {
        Debug.Log("Buton ger�ekten �al���yor!");
    }

}
