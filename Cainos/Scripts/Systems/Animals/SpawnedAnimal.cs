using UnityEngine;

public class SpawnedAnimal : MonoBehaviour
{
    private AnimalSpawner ownerSpawner;

    public void SetOwner(AnimalSpawner owner)
    {
        ownerSpawner = owner;
    }

    public bool IsBarnAnimal()
    {
        return ownerSpawner != null;
    }

    private void OnDestroy()
    {
        if (ownerSpawner != null)
        {
            ownerSpawner.RemoveAnimal(gameObject);
        }
    }
}