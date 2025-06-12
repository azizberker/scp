using System.Collections;
using UnityEngine;
using DG.Tweening;

public class FirstPersonControllerCustom : MonoBehaviour
{
    // Sprint
    private Camera cam;
    private bool isRunning;
    private float defaultFOV;
    private float sprintFOV = 67f; // Sprint sırasında FOV
    private float fovTransitionTime = 0.35f; // FOV değişim süresi

    // Movement
    private CharacterController characterController;
    public float walkSpeed = 0.5f;
    public float sprintSpeed = 3f;
    private float speed;

    // Gravity & Jump
    private float gravity = -30f;
    private Vector3 velocity;
    [SerializeField] private float jumpHeight = 0.6f;

    private bool isGrounded;

    // Stamina Sistemi
    private Stamina staminaSystem;

    // Ses 
    public AudioClip[] footstepSounds; // Birden fazla ses için dizi, istersen tek ses de olabilir
    private AudioSource audioSource;
    private float footstepTimer = 0f;
    public float footstepInterval = 0.4f; // Adım aralığı (koşarken azaltabilirsin)
    private int footstepIndex = 0;
    public AudioClip jumpSound;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        cam = Camera.main;
        defaultFOV = cam.fieldOfView;
        staminaSystem = GetComponent<Stamina>();
        speed = walkSpeed;
        audioSource = GetComponent<AudioSource>();
        
}

    void Update()
    {
        DoMove();
        DoGravityAndJump();
        DoSprint();

        // Ayakta ve hareket ediyorsa
        if (characterController.isGrounded && (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f || Mathf.Abs(Input.GetAxis("Vertical")) > 0.1f))
        {
            footstepTimer -= Time.deltaTime;
            if (footstepTimer <= 0f)
            {
                PlayFootstepSound();
                footstepTimer = isRunning ? footstepInterval * 0.6f : footstepInterval; // Koşarken daha sık çal
            }
        }
        else
        {
            footstepTimer = 0f; // Durdurunca sıfırla
        }
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
            PlayJumpSound();
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
    private void PlayFootstepSound()
    {
        if (footstepSounds.Length > 0)
        {
            audioSource.clip = footstepSounds[footstepIndex];
            audioSource.Play();
            footstepIndex = (footstepIndex + 1) % footstepSounds.Length; // sıradaki sesi al
        }
    }

    private void PlayJumpSound()
    {
        if (audioSource != null && jumpSound != null)
            audioSource.PlayOneShot(jumpSound);
    }
}
