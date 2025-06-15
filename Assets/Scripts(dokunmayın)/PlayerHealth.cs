using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.PostProcessing;
using TMPro;
using System.Collections;


public class PlayerHealth : MonoBehaviour
{
    [Header("Death Screen UI")]
    public GameObject deathPanel; // DeathPanel objesi
    public Button restartButton;  // Restart butonu

    [Header("Health Settings")]
    public int maxHealth = 100;
    public int currentHealth;
    public int maxTempHealth = 100;  // Maksimum geçici can
    public int currentTempHealth;    // Mevcut geçici can
    public TextMeshProUGUI tempHealthText;  // Geçici can için text

    [Header("Death Effect")]
    public PostProcessVolume postProcessVolume;
    public AudioSource deathSound;
    public float deathEffectDuration = 1f;
    private ColorGrading colorGrading;
    private Vignette vignette;
    private bool isDead = false;

    [Header("Death Animation")]
    public float deathRotationDuration = 1.5f; // Kamera dönüş süresi
    public float finalCameraAngle = 90f; // Son kamera açısı
    public float fallForce = 300f; // Düşme kuvveti
    private Camera playerCamera;
    private FirstPersonControllerCustom playerController;
    private MouseLook mouseLook;
    private float deathTimer = 0f;
    private Quaternion initialRotation;
    private bool isDeathAnimationStarted = false;
    private CharacterController characterController;
    private CapsuleCollider playerCollider;
    private Rigidbody playerRigidbody;

    //Ses
    public AudioClip jumpscareClip; // Inspector’dan atacaksın
    private AudioSource jumpscareSource; // Kodda bulacağız
    private bool jumpscared = false; // Sadece 1 kere çalması için

    // FADE için ekler
    private CanvasGroup deathPanelCanvasGroup;
    public float deathPanelFadeDuration = 1.5f; // Fade süresi

    void Start()
    {
        Time.timeScale = 1f;

        currentHealth = maxHealth;
        currentTempHealth = maxTempHealth;

        if (tempHealthText != null)
            UpdateTempHealthText();

        playerCamera = GetComponentInChildren<Camera>();
        playerController = GetComponent<FirstPersonControllerCustom>();
        mouseLook = GetComponentInChildren<MouseLook>();
        characterController = GetComponent<CharacterController>();

        SetupPostProcess();
        ResetPostProcess();

        jumpscareSource = GetComponent<AudioSource>();

        if (deathPanel != null)
        {
            deathPanel.SetActive(false);
            deathPanelCanvasGroup = deathPanel.GetComponent<CanvasGroup>();
            if (deathPanelCanvasGroup != null)
                deathPanelCanvasGroup.alpha = 0f;
        }

        if (restartButton != null)
            restartButton.onClick.AddListener(RestartGame);
    }

    void Update()
    {
        if (tempHealthText != null)
            UpdateTempHealthText();

        if (isDeathAnimationStarted && !isDead)
        {
            deathTimer += Time.deltaTime;
            float progress = deathTimer / deathRotationDuration;

            if (playerCamera != null)
            {
                float currentAngle = Mathf.Lerp(0, finalCameraAngle, progress);
                playerCamera.transform.localRotation = initialRotation * Quaternion.Euler(currentAngle, 0, 0);
            }

            if (progress >= 1f)
            {
                CompleteDeathSequence();
            }
        }
    }

    void UpdateTempHealthText()
    {
        tempHealthText.text = currentTempHealth.ToString();
    }

    void SetupPostProcess()
    {
        if (postProcessVolume == null)
        {
            GameObject ppObj = new GameObject("RuntimePostProcessVolume");
            ppObj.transform.SetParent(transform);
            postProcessVolume = ppObj.AddComponent<PostProcessVolume>();
            postProcessVolume.isGlobal = true;
            postProcessVolume.priority = 100f;
            postProcessVolume.sharedProfile = ScriptableObject.CreateInstance<PostProcessProfile>();
        }

        var profile = postProcessVolume.sharedProfile;

        if (!profile.TryGetSettings(out colorGrading))
        {
            colorGrading = profile.AddSettings<ColorGrading>();
        }
        colorGrading.enabled.Override(true);
        colorGrading.postExposure.overrideState = true;
        colorGrading.tint.overrideState = true;
        colorGrading.colorFilter.overrideState = true;

        if (!profile.TryGetSettings(out vignette))
        {
            vignette = profile.AddSettings<Vignette>();
        }
        vignette.enabled.Override(true);
        vignette.intensity.overrideState = true;
        vignette.smoothness.overrideState = true;
        vignette.smoothness.value = 0.8f;
    }

    void ResetPostProcess()
    {
        if (colorGrading != null)
        {
            colorGrading.postExposure.value = 0f;
            colorGrading.colorFilter.value = Color.white;
            colorGrading.tint.value = 0f;
        }
        if (vignette != null)
        {
            vignette.intensity.value = 0f;
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        if (currentTempHealth > 0)
        {
            if (damage <= currentTempHealth)
            {
                currentTempHealth -= damage;
                damage = 0;
            }
            else
            {
                damage -= currentTempHealth;
                currentTempHealth = 0;
            }
            UpdateTempHealthText();
        }

        if (damage > 0)
        {
            currentHealth -= damage;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        }

        Debug.Log("Hasar alındı! Can: " + currentHealth + " Geçici Can: " + currentTempHealth);

        if (currentHealth <= 0 && !isDead)
        {
            Die();
        }

        if (currentHealth <= 0 && !jumpscared)
        {
            PlayJumpscare();
            jumpscared = true;
        }
    }

    void PlayJumpscare()
    {
        if (jumpscareSource != null && jumpscareClip != null)
            jumpscareSource.PlayOneShot(jumpscareClip);
    }

    void SetupRagdoll()
    {
        if (characterController != null)
            characterController.enabled = false;

        if (playerRigidbody == null)
        {
            playerRigidbody = gameObject.AddComponent<Rigidbody>();
            playerRigidbody.mass = 70f;
            playerRigidbody.drag = 1f;
            playerRigidbody.angularDrag = 5f;
            playerRigidbody.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX;
        }

        if (playerCollider == null)
        {
            playerCollider = gameObject.AddComponent<CapsuleCollider>();
            if (characterController != null)
            {
                playerCollider.height = characterController.height;
                playerCollider.radius = characterController.radius;
                playerCollider.center = characterController.center;
            }
        }

        if (playerRigidbody != null)
        {
            Vector3 fallDirection = transform.forward + Vector3.down;
            playerRigidbody.AddForce(fallDirection.normalized * fallForce, ForceMode.Impulse);
        }
    }

    void Die()
    {
        if (isDead || isDeathAnimationStarted) return;
        Debug.Log("Oyuncu öldü!");

        isDeathAnimationStarted = true;
        deathTimer = 0f;

        if (playerCamera != null)
            initialRotation = playerCamera.transform.localRotation;

        if (playerController != null)
            playerController.enabled = false;
        if (mouseLook != null)
            mouseLook.enabled = false;

        var playerCtrl = GetComponent<PlayerController>();
        if (playerCtrl != null)
            playerCtrl.enabled = false;

        var firstPersonCtrl = GetComponent<FirstPersonControllerCustom>();
        if (firstPersonCtrl != null)
            firstPersonCtrl.enabled = false;

        SetupRagdoll();

        StartCoroutine(DeathSequence());
    }

    void CompleteDeathSequence()
    {
        isDead = true;
        isDeathAnimationStarted = false;

        // Fade ile paneli göster
        if (deathPanel != null && deathPanelCanvasGroup != null)
            StartCoroutine(FadeInDeathPanel());

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 0f;
    }

    private IEnumerator FadeInDeathPanel()
    {
        deathPanel.SetActive(true);
        deathPanelCanvasGroup.alpha = 0f;

        float elapsed = 0f;
        while (elapsed < deathPanelFadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / deathPanelFadeDuration);
            deathPanelCanvasGroup.alpha = t;
            yield return null;
        }
        deathPanelCanvasGroup.alpha = 1f;
    }

    System.Collections.IEnumerator DeathSequence()
    {
        if (isDead) yield break;

        SetupPostProcess();

        if (deathSound != null)
            deathSound.Play();

        float elapsedTime = 0f;
        float startExposure = 0f;
        float targetExposure = -5f;
        float startVignette = 0f;
        float targetVignette = 0.55f;
        Color startColor = Color.white;
        Color endColor = new Color(0.7f, 0f, 0f);

        while (elapsedTime < deathEffectDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float t = elapsedTime / deathEffectDuration;
            colorGrading.postExposure.value = Mathf.Lerp(startExposure, targetExposure, t);
            colorGrading.colorFilter.value = Color.Lerp(startColor, endColor, t);
            vignette.intensity.value = Mathf.Lerp(startVignette, targetVignette, t);
            yield return null;
        }

        // Paneli ve mouse'u aç, oyunu durdur
        CompleteDeathSequence();
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }

    public void HealTemp(int amount)
    {
        currentTempHealth += amount;
        currentTempHealth = Mathf.Clamp(currentTempHealth, 0, maxTempHealth);
        UpdateTempHealthText();
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
