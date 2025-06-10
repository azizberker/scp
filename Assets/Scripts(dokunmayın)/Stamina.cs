using UnityEngine;
using UnityEngine.UI;

public class Stamina : MonoBehaviour
{
    public float stamina; // Başlangı ve maksimum stamina değeri
    public Slider StaminaBar;
    public Image staminabarImage;

    // Koşma, zıplama ve stamina yenilenme değerleri
    public float staminaDrainRate = 10f;  // Koşma sırasında harcanan stamina miktarı
    public float staminaJumpCost = 5f;    // Zıplama sırasında harcanan stamina miktarı
    public float staminaRegenRate = 5f;   // Koşmadaki harcamadan sonra dolanma hızı

    private float maxStamina;
    private bool canRegenerate = true;
    private float emptyStaminaDelay = 5f;    // Stamina 0 olduğundaki bekleme süresi
    private float partialStaminaDelay = 3f;  // Stamina 0'dan büyük olduğundaki bekleme süresi
    private float regenerationTimer = 0f;
    private bool isWaiting = false;
    private bool startedWaiting = false;

    void Start()
    {
        maxStamina = stamina;
        if (StaminaBar != null)
            StaminaBar.value = maxStamina;
        
        // Image'in type'ını Filled olarak ayarlıyoruz
        if (staminabarImage != null)
        {
            staminabarImage.type = Image.Type.Filled;
            staminabarImage.fillMethod = Image.FillMethod.Radial360;
            staminabarImage.fillOrigin = (int)Image.Origin360.Top;
            staminabarImage.fillClockwise = true;
        }
    }

    void Update()
    {
        if (!Input.GetKey(KeyCode.LeftShift))
        {
            // Koşmayı yeni bıraktıysak
            if (!startedWaiting)
            {
                isWaiting = true;
                startedWaiting = true;
                regenerationTimer = 0f;
                canRegenerate = false;
            }

            if (isWaiting)
            {
                float currentDelay = (stamina <= 0) ? emptyStaminaDelay : partialStaminaDelay;
                regenerationTimer += Time.deltaTime;
                
                if (regenerationTimer >= currentDelay)
                {
                    canRegenerate = true;
                    isWaiting = false;
                }
            }

            if (canRegenerate)
            {
                RegenerateStamina();
            }
        }
        else
        {
            regenerationTimer = 0f;
            isWaiting = false;
            canRegenerate = false;
            startedWaiting = false;
        }

        // Hem Slider hem de Image'i güncelliyoruz
        if (StaminaBar != null)
            StaminaBar.value = stamina;
            
        if (staminabarImage != null)
            staminabarImage.fillAmount = stamina / maxStamina;

        // Stamina değeri 0 ile maksimum arasında tutuluyor
        if (stamina > maxStamina)
            stamina = maxStamina;
        if (stamina < 0)
            stamina = 0;
    }

    // Stamina yenileme fonksiyonu
    private void RegenerateStamina()
    {
        stamina += staminaRegenRate * Time.deltaTime;
    }

    // Koşma için gerekli stamina kontrolü
    public bool CanSprint()
    {
        return stamina > 0;
    }

    // Zıplama için gerekli stamina kontrolü
    public bool CanJump()
    {
        return stamina > 0;
    }

    // Belirtilen miktarda stamina harcaması
    public void UseStamina(float amount)
    {
        stamina -= amount;
        if (stamina < 0)
            stamina = 0;
    }
}
