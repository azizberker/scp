using UnityEngine;

public class MoveEffect : MonoBehaviour
{
    // Y�r�me ve ko�ma i�in bobbing ayarlar�
    public float walkBobSpeed = 14f;
    public float runBobSpeed = 18f;
    public float walkBobAmount = 0.05f;
    public float runBobAmount = 0.1f;

    private float bobTimer = 0f;
    private Vector3 initialLocalPos;

    void Start()
    {
        // Kameran�n ba�lang�� lokal pozisyonunu sakla
        initialLocalPos = transform.localPosition;
    }

    void Update()
    {
        // Klavye giri�lerini al
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        float inputMagnitude = new Vector2(horizontal, vertical).magnitude;

        if (inputMagnitude > 0.1f) // Hareket varsa
        {
            // E�er LeftShift tu�una bas�l�ysa ko�ma, de�ilse y�r�me
            bool isRunning = Input.GetKey(KeyCode.LeftShift);
            float bobSpeed = isRunning ? runBobSpeed : walkBobSpeed;
            float bobAmount = isRunning ? runBobAmount : walkBobAmount;

            bobTimer += Time.deltaTime * bobSpeed;
            float offsetY = Mathf.Sin(bobTimer) * bobAmount;
            transform.localPosition = initialLocalPos + new Vector3(0, offsetY, 0);
        }
        else
        {
            // Hareket yoksa efekt s�f�rlans�n
            bobTimer = 0f;
            transform.localPosition = initialLocalPos;
        }
    }
}
