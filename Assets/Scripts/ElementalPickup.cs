using UnityEngine;

public class ElementalPickup : MonoBehaviour
{
    public ElementalType type;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ElementalInventory inventory = other.GetComponent<ElementalInventory>();
            if (inventory != null && inventory.TryAddElemental(type))
            {
                Destroy(gameObject);
            }
        }
    }
}
