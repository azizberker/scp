using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance;

    public int Money = 0;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void AddMoney(int amount)
    {
        if (MoneyBar.Instance != null && !MoneyBar.Instance.CanAddMoney(amount))
        {
            Debug.Log("Cannot add money: Would exceed limit!");
            return;
        }

        Money += amount;
        Debug.Log("Money: " + Money);
        
        if (MoneyBar.Instance != null)
        {
            MoneyBar.Instance.UpdateMoneyText();
        }
    }
}

