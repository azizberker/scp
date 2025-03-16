using UnityEngine;
using UnityEngine.UI;

public class Stamina : MonoBehaviour
{
    public float stamina; // Ba�lang�� ve maksimum stamina de�eri
    public Slider StaminaBar;

    // Ko�ma, z�plama ve stamina yenilenme de�erleri
    public float staminaDrainRate = 10f;  // Ko�ma s�ras�nda harcanan stamina miktar�
    public float staminaJumpCost = 5f;    // Z�plama s�ras�nda harcanan stamina miktar�
    public float staminaRegenRate = 5f;   // Ko�madaki harcamadan sonra dolanma h�z�

    private float maxStamina;

    void Start()
    {
        maxStamina = stamina;
        if (StaminaBar != null)
            StaminaBar.value = maxStamina;
    }

    void Update()
    {
        // LeftShift tu�una bas�lmad���nda (yani ko�ulmad���nda) stamina yeniden dolar
        if (!Input.GetKey(KeyCode.LeftShift))
        {
            RegenerateStamina();
        }

        if (StaminaBar != null)
            StaminaBar.value = stamina;

        // Stamina de�eri 0 ile maksimum aras�nda tutuluyor
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

    // Ko�ma i�in gerekli stamina kontrol�
    public bool CanSprint()
    {
        return stamina > 0;
    }

    // Z�plama i�in gerekli stamina kontrol�
    public bool CanJump()
    {
        return stamina > 0;
    }

    // Belirtilen miktarda stamina harcamas�
    public void UseStamina(float amount)
    {
        stamina -= amount;
        if (stamina < 0)
            stamina = 0;
    }
}
