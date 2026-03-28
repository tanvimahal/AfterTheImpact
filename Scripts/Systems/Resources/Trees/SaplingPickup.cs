using UnityEngine;

public class SaplingPickup : MonoBehaviour
{
    [Header("Sapling Amount Chances")]
    public int chanceForOne = 65;
    public int chanceForTwo = 30;
    public int chanceForThree = 5;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            int saplingAmount = GetRandomSaplingAmount();
            SaplingSystem.Instance.AddSaplings(saplingAmount);
            Destroy(gameObject);
        }
    }

    int GetRandomSaplingAmount()
    {
        int roll = Random.Range(1, 101);

        if (roll <= chanceForOne)
            return 1;
        else if (roll <= chanceForOne + chanceForTwo)
            return 2;
        else
            return 3;
    }
}