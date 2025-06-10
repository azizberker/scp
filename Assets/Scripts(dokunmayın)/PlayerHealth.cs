using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.PostProcessing;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
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

    void Start()
    {
        // Oyunu normal hızda başlat
        Time.timeScale = 1f;

        currentHealth = maxHealth;
        currentTempHealth = maxTempHealth;  // Geçici canı başlangıçta maksimum yap

        if (tempHealthText != null)
        {
            UpdateTempHealthText();
        }

        // Kamera ve kontrol bileşenlerini al
        playerCamera = GetComponentInChildren<Camera>();
        playerController = GetComponent<FirstPersonControllerCustom>();
        mouseLook = GetComponentInChildren<MouseLook>();
        characterController = GetComponent<CharacterController>();

        SetupPostProcess();
        ResetPostProcess();
    }

    void Update()
    {
        if (tempHealthText != null)
        {
            UpdateTempHealthText();
        }

        // Ölüm animasyonunu güncelle
        if (isDeathAnimationStarted && !isDead)
        {
            deathTimer += Time.deltaTime;
            float progress = deathTimer / deathRotationDuration;

            if (playerCamera != null)
            {
                // Kamerayı yavaşça döndür
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
        // Eğer volume yoksa, runtime'da oluştur
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

        // ColorGrading ayarla
        if (!profile.TryGetSettings(out colorGrading))
        {
            colorGrading = profile.AddSettings<ColorGrading>();
        }
        colorGrading.enabled.Override(true);
        colorGrading.postExposure.overrideState = true;
        colorGrading.tint.overrideState = true;
        colorGrading.colorFilter.overrideState = true;

        // Vignette ayarla
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

        // Önce geçici cana hasar ver
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

        // Kalan hasar varsa normal cana ver
        if (damage > 0)
        {
            currentHealth -= damage;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        }

        Debug.Log("Hasar alındı! Can: " + currentHealth + " Geçici Can: " + currentTempHealth);

        // Can sıfırsa öl
        if (currentHealth <= 0 && !isDead)
        {
            Die();
        }
    }

    void SetupRagdoll()
    {
        // CharacterController'ı devre dışı bırak
        if (characterController != null)
        {
            characterController.enabled = false;
        }

        // Rigidbody ekle
        if (playerRigidbody == null)
        {
            playerRigidbody = gameObject.AddComponent<Rigidbody>();
            playerRigidbody.mass = 70f; // Gerçekçi bir kütle
            playerRigidbody.drag = 1f; // Hava direnci
            playerRigidbody.angularDrag = 5f; // Dönme direnci
            playerRigidbody.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX;
        }

        // CapsuleCollider ekle (CharacterController yerine)
        if (playerCollider == null)
        {
            playerCollider = gameObject.AddComponent<CapsuleCollider>();
            // CharacterController'ın boyutlarını kullan
            if (characterController != null)
            {
                playerCollider.height = characterController.height;
                playerCollider.radius = characterController.radius;
                playerCollider.center = characterController.center;
            }
        }

        // Düşme kuvveti uygula
        if (playerRigidbody != null)
        {
            // İleri ve aşağı doğru kuvvet uygula
            Vector3 fallDirection = transform.forward + Vector3.down;
            playerRigidbody.AddForce(fallDirection.normalized * fallForce, ForceMode.Impulse);
        }
    }

    void Die()
    {
        if (isDead || isDeathAnimationStarted) return;
        Debug.Log("Oyuncu öldü!");

        // Ölüm animasyonunu başlat
        isDeathAnimationStarted = true;
        deathTimer = 0f;

        // Başlangıç rotasyonunu kaydet
        if (playerCamera != null)
        {
            initialRotation = playerCamera.transform.localRotation;
        }

        // Tüm hareket kontrollerini devre dışı bırak
        if (playerController != null)
        {
            playerController.enabled = false;
        }
        if (mouseLook != null)
        {
            mouseLook.enabled = false;
        }

        // PlayerController'ı bul ve devre dışı bırak
        var playerCtrl = GetComponent<PlayerController>();
        if (playerCtrl != null)
        {
            playerCtrl.enabled = false;
        }

        // FirstPersonController'ı bul ve devre dışı bırak
        var firstPersonCtrl = GetComponent<FirstPersonControllerCustom>();
        if (firstPersonCtrl != null)
        {
            firstPersonCtrl.enabled = false;
        }

        // Ragdoll fiziğini ayarla
        SetupRagdoll();

        // Post-process efektlerini başlat
        StartCoroutine(DeathSequence());
    }

    void CompleteDeathSequence()
    {
        isDead = true;
        isDeathAnimationStarted = false;
    }

    System.Collections.IEnumerator DeathSequence()
    {
        if (isDead) yield break;

        SetupPostProcess();

        // Ses efektini çal
        if (deathSound != null)
        {
            deathSound.Play();
        }

        // Oyunu dondur ama coroutine'ler çalışsın
        Time.timeScale = 0f;

        // Ekranı karart ve kan efekti ekle
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

        // 5 saniye bekle (gerçek zaman)
        yield return new WaitForSecondsRealtime(5f);

        // Oyunu yeniden başlatmadan önce timeScale'i geri al
        Time.timeScale = 1f;

        // Post process'i sıfırla (yüklenen sahnede de güvenlik için)
        ResetPostProcess();

        // Sahneyi yeniden yükle
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
}