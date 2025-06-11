using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneDoorRaycast : MonoBehaviour
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
                if (Input.GetKey(KeyCode.E))
                {
                    holdTimer += Time.deltaTime;
                    if (holdTimer >= holdDuration)
                    {
                        LoadScene();
                        holdTimer = 0f;
                    }
                }
                else
                {
                    holdTimer = 0f;
                }

                return;
            }
        }

        // Ray bu objeye bakmıyorsa timer sıfırla
        holdTimer = 0f;
    }

    void LoadScene()
    {
        if (!string.IsNullOrEmpty(targetSceneName))
        {
            Debug.Log("🚪 Sahne geçişi: " + targetSceneName);
            Time.timeScale = 1f;
            SceneManager.LoadScene(targetSceneName);
        }
    }
}
