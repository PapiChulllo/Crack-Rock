using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public string itemName = "Mana Orb";
    public int manaBoost = 20; // Boost mana by this amount
    public GameObject pickupPrompt; // UI popup for "Press E"

    private bool playerInRange = false;

    private void Start()
    {
        // Ensure the prompt starts disabled
        if (pickupPrompt != null)
        {
            pickupPrompt.SetActive(false);
        }
    }

    private void Update()
    {
        // Allow the player to pick up the item when in range
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            PickupItem();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            if (pickupPrompt != null)
            {
                pickupPrompt.SetActive(true); // Show the pickup prompt
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            if (pickupPrompt != null)
            {
                pickupPrompt.SetActive(false); // Hide the pickup prompt
            }
        }
    }

    private void PickupItem()
    {
        playerInRange = false;

        // Hide the prompt
        if (pickupPrompt != null)
        {
            pickupPrompt.SetActive(false);
        }

        // Debug message to confirm pickup
        Debug.Log($"Picked up {itemName}. Mana Boost: {manaBoost}");

        // Destroy the item after pickup (optional)
        Destroy(gameObject);
    }
}
