using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyBar : MonoBehaviour
{
    public static MoneyBar Instance;
    public TextMeshProUGUI moneyText;
    private const int MONEY_LIMIT = 8000;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        UpdateMoneyText();
    }

    void Update()
    {
        // Şu an kullanılmıyor ama istenirse animasyon vs. buraya eklenebilir
    }

    public void UpdateMoneyText()
    {
        if (moneyText != null && PlayerStats.Instance != null)
        {
            moneyText.text = $"{PlayerStats.Instance.Money}/{MONEY_LIMIT}$";
        }
    }

    public bool CanAddMoney(int amount)
    {
        return PlayerStats.Instance.Money + amount <= MONEY_LIMIT;
    }

    public void AddMoney(int amount)
    {
        if (PlayerStats.Instance != null)
        {
            if (CanAddMoney(amount))
            {
                PlayerStats.Instance.Money += amount;
                UpdateMoneyText();
                Debug.Log($"Para eklendi: +{amount}$ → Yeni bakiye: {PlayerStats.Instance.Money}");
            }
            else
            {
                Debug.Log("Para limiti aşılamaz!");
            }
        }
    }
}
