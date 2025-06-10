using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyBar : MonoBehaviour
{
    public static MoneyBar Instance;
    public TextMeshProUGUI moneyText;
    private const int MONEY_LIMIT = 8000;
    private int currentMoney = 0;

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
        if (moneyText != null)
        {
            moneyText.text = $"{currentMoney}/{MONEY_LIMIT}$";
        }
    }

    public bool CanAddMoney(int amount)
    {
        return currentMoney + amount <= MONEY_LIMIT;
    }

    public void AddMoney(int amount)
    {
        if (CanAddMoney(amount))
        {
            currentMoney += amount;
            UpdateMoneyText();
            Debug.Log($"Para eklendi: +{amount}$ → Yeni bakiye: {currentMoney}");
        }
        else
        {
            Debug.Log("Para limiti aşılamaz!");
        }
    }
}
