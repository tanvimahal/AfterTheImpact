using UnityEngine;

public class FoodPickup : MonoBehaviour
{
    [SerializeField] private int foodAmount = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (FoodSystem.Instance != null)
            {
                FoodSystem.Instance.AddFood(foodAmount);
            }

            Destroy(gameObject);
        }
    }
}