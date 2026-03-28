using UnityEngine;

public class WoodPickup : MonoBehaviour
{
    public int minWood = 1;
    public int maxWood = 5;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            int woodAmount = Random.Range(minWood, maxWood + 1);
            Debug.Log("Adding wood: " + woodAmount);

            if (WoodSystem.Instance == null)
            {
                Debug.LogError("WoodSystem.Instance is NULL");
                return;
            }

            WoodSystem.Instance.AddWood(woodAmount);
            Destroy(gameObject);
        }
    }
}