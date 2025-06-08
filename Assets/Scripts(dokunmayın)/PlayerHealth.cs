using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.PostProcessing;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    [Header("UI")]
    public Slider healthSlider;

    [Header("Death Effect")]
    public PostProcessVolume postProcessVolume;
    public AudioSource deathSound;
    public float deathEffectDuration = 1f;
    private ColorGrading colorGrading;
    private Vignette vignette;
    private bool isDead = false;

    void Start()
    {
        // Oyunu normal hızda başlat
        Time.timeScale = 1f;

        currentHealth = maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;

        SetupPostProcess();

        // Başlangıçta post process değerlerini sıfırla
        ResetPostProcess();
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
        if (isDead) return; // ölüyse hasar alma

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        healthSlider.value = currentHealth;

        Debug.Log("Hasar alındı! Can: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        healthSlider.value = currentHealth;
    }

    void Die()
    {
        Debug.Log("Oyuncu öldü!");
        StartCoroutine(DeathSequence());
    }

    System.Collections.IEnumerator DeathSequence()
    {
        if (isDead) yield break;
        isDead = true;

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

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}