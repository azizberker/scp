using UnityEngine;
using UnityEngine.UI;

public class Stamina : MonoBehaviour
{
    public float stamina; // Baþlangýç ve maksimum stamina deðeri
    public Slider StaminaBar;

    // Koþma, zýplama ve stamina yenilenme deðerleri
    public float staminaDrainRate = 10f;  // Koþma sýrasýnda harcanan stamina miktarý
    public float staminaJumpCost = 5f;    // Zýplama sýrasýnda harcanan stamina miktarý
    public float staminaRegenRate = 5f;   // Koþmadaki harcamadan sonra dolanma hýzý

    private float maxStamina;

    void Start()
    {
        maxStamina = stamina;
        if (StaminaBar != null)
            StaminaBar.value = maxStamina;
    }

    void Update()
    {
        // LeftShift tuþuna basýlmadýðýnda (yani koþulmadýðýnda) stamina yeniden dolar
        if (!Input.GetKey(KeyCode.LeftShift))
        {
            RegenerateStamina();
        }

        if (StaminaBar != null)
            StaminaBar.value = stamina;

        // Stamina deðeri 0 ile maksimum arasýnda tutuluyor
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

    // Koþma için gerekli stamina kontrolü
    public bool CanSprint()
    {
        return stamina > 0;
    }

    // Zýplama için gerekli stamina kontrolü
    public bool CanJump()
    {
        return stamina > 0;
    }

    // Belirtilen miktarda stamina harcamasý
    public void UseStamina(float amount)
    {
        stamina -= amount;
        if (stamina < 0)
            stamina = 0;
    }
}
