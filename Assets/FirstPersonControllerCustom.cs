using System.Collections;
using UnityEngine;
using DG.Tweening;

public class FirstPersonControllerCustom : MonoBehaviour
{
    // Sprint
    private Camera cam;
    private bool isRunning;
    private float defaultFOV;
    private float sprintFOV = 80f; // Sprint sırasında FOV
    private float fovTransitionTime = 0.25f; // FOV değişim süresi

    // Movement
    private CharacterController characterController;
    public float walkSpeed = 3f;
    public float sprintSpeed = 6f;
    private float speed;

    // Gravity & Jump
    private float gravity = -30f;
    private Vector3 velocity;
    [SerializeField] private float jumpHeight = 1.2f;

    private bool isGrounded;

    // Stamina Sistemi
    private Stamina staminaSystem;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        cam = Camera.main;
        defaultFOV = cam.fieldOfView;
        staminaSystem = GetComponent<Stamina>();
        speed = walkSpeed;
    }

    void Update()
    {
        DoMove();
        DoGravityAndJump();
        DoSprint();
    }

    private void DoSprint()
    {
        if (Input.GetKey(KeyCode.LeftShift) && staminaSystem.CanSprint())
        {
            speed = sprintSpeed;
            if (!isRunning)
            {
                isRunning = true;
                cam.DOFieldOfView(sprintFOV, fovTransitionTime);
            }
            // Koşarken stamina düşüşü
            staminaSystem.UseStamina(staminaSystem.staminaDrainRate * Time.deltaTime);
        }
        else
        {
            speed = walkSpeed;
            isRunning = false;
            cam.DOFieldOfView(defaultFOV, fovTransitionTime);
        }

        // Stamina 0 iken hareketi tamamen durdur
        if (staminaSystem.stamina <= 0)
        {
            speed = 0f;
        }
    }

    private void DoGravityAndJump()
    {
        isGrounded = characterController.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -5f;
        }

        // Zıplama sırasında stamina kontrolü ve düşüşü
        if (Input.GetButtonDown("Jump") && isGrounded && staminaSystem.CanJump())
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            staminaSystem.UseStamina(staminaSystem.staminaJumpCost);
        }

        velocity.y += gravity * Time.deltaTime * 1.2f;
        Vector3 moveVector = new Vector3(0, velocity.y, 0);
        characterController.Move(moveVector * Time.deltaTime);
    }

    private void DoMove()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Stamina bittiğinde hareketi devre dışı bırak
        if (staminaSystem.stamina <= 0)
        {
            horizontal = 0;
            vertical = 0;
        }

        Vector3 dir = transform.right * horizontal + transform.forward * vertical;
        characterController.Move((dir * speed + new Vector3(0, velocity.y, 0)) * Time.deltaTime);
    }

    // Dışarıya açık property'ler
    public bool IsRunning { get { return isRunning; } }
    public float CurrentSpeed { get { return speed; } }

    // Gravity etkisini hariç tutarak karakterin yatay hızını hesaplar
    public float HorizontalSpeed
    {
        get
        {
            Vector3 horizontalVel = characterController.velocity;
            horizontalVel.y = 0;
            return horizontalVel.magnitude;
        }
    }
}
