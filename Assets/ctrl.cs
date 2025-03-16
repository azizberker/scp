using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private float originalHeight;
    private Vector3 originalCenter;

    public float reducedHeight = 0.1f;
    public float crouchSpeed = 5f;
    public float speed = 5.0f;
    public float transitionDuration = 0.1f; // Geçiþ süresi

    void Start()
    {
        controller = GetComponent<CharacterController>();
        originalHeight = controller.height;
        originalCenter = controller.center;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            StartCoroutine(CrouchTransition(true));
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            StartCoroutine(CrouchTransition(false));
        }
        Move();
    }

    IEnumerator CrouchTransition(bool crouching)
    {
        float startHeight = controller.height;
        Vector3 startCenter = controller.center;
        float targetHeight = crouching ? reducedHeight : originalHeight;
        Vector3 targetCenter = crouching ? new Vector3(originalCenter.x, reducedHeight / 2f, originalCenter.z) : originalCenter;
        float elapsed = 0f;

        while (elapsed < transitionDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / transitionDuration;
            controller.height = Mathf.Lerp(startHeight, targetHeight, t);
            controller.center = Vector3.Lerp(startCenter, targetCenter, t);
            yield return null;
        }
        controller.height = targetHeight;
        controller.center = targetCenter;
        // Ýsteðe baðlý: Hýzý da güncelleyebilirsiniz
        speed = crouching ? crouchSpeed : 5.0f;
    }

    void Move()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 direction = transform.right * horizontal + transform.forward * vertical;
        controller.Move(direction * speed * Time.deltaTime);
    }
}
