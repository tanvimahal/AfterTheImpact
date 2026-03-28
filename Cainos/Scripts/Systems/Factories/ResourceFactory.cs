using UnityEngine;
using System.Collections;

public class ResourceFactory : MonoBehaviour
{
    [Header("Production Settings")]
    public ResourceType resourceType = ResourceType.None;
    public int amountPerCycle = 1;
    public float productionInterval = 5f;

    private Coroutine productionRoutine;

    private void OnEnable()
    {
        productionRoutine = StartCoroutine(ProduceResource());
    }

    private IEnumerator ProduceResource()
    {
        while (true)
        {
            yield return new WaitForSeconds(productionInterval);

            if (ResourceManager.Instance == null)
            {
                Debug.LogWarning("ResourceManager instance not found.");
                continue;
            }

            if (resourceType == ResourceType.None)
            {
                Debug.LogWarning(gameObject.name + " has ResourceType.None assigned.");
                continue;
            }

            ResourceManager.Instance.Add(resourceType, amountPerCycle);
            Debug.Log(gameObject.name + " produced " + amountPerCycle + " " + resourceType);
        }
    }

    private void OnDisable()
    {
        if (productionRoutine != null)
        {
            StopCoroutine(productionRoutine);
            productionRoutine = null;
        }
    }
}