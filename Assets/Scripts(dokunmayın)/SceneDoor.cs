using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneDoor : MonoBehaviour
{
    public string targetSceneName;
    public float holdDuration = 2f;
    public float triggerDistance = 3f;

    private float holdTimer = 0f;
    private Transform playerCam;

    void Start()
    {
        playerCam = Camera.main.transform;
    }

    void Update()
    {
        Ray ray = new Ray(playerCam.position, playerCam.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, triggerDistance))
        {
            if (hit.collider.gameObject == gameObject)
            {
                // UI Göster
                SceneDoorUI.Instance?.ShowUI();

                if (Input.GetKey(KeyCode.E))
                {
                    holdTimer += Time.deltaTime;

                    float progress = 1f - (holdTimer / holdDuration);
                    SceneDoorUI.Instance?.UpdateProgress(progress);

                    if (holdTimer >= holdDuration)
                    {
                        holdTimer = 0f;
                        SceneDoorUI.Instance?.HideUI();
                        LoadScene();
                    }
                }
                else
                {
                    holdTimer = 0f;
                    SceneDoorUI.Instance?.UpdateProgress(1f);
                }

                return;
            }
        }

        // Kapıya bakmıyorsan UI kapat ve timer sıfırla
        holdTimer = 0f;
        if (SceneDoorUI.Instance == null) return;

        SceneDoorUI.Instance?.HideUI();
    }

    void LoadScene()
{
    if (!string.IsNullOrEmpty(targetSceneName))
    {
        Debug.Log("🚪 Sahne geçişi: " + targetSceneName);
        Time.timeScale = 1f;
        SceneManager.LoadScene(targetSceneName);

        enabled = false; // 🔥 scripti devre dışı bırak, sahne geçince eski obje yok olacak
    }
}

}
