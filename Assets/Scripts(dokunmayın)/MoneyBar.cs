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

    // Start is called before the first frame update
    void Start()
    {
        UpdateMoneyText();
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
