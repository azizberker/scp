using UnityEngine;

public class ShopZone : MonoBehaviour
{
    private bool playerInZone = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = true;
            Debug.Log("You can sell items here! Press E to sell.");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = false;
        }
    }

    void Update()
    {
        if (playerInZone && Input.GetKeyDown(KeyCode.E))
        {
            int earned = Inventory.Instance.SellAllItems();
            Debug.Log($"Sold all items for {earned}!");
        }
    }
}
