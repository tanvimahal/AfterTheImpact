using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    public float attackRadius = 1.5f;
    public float attackDistance = 1f;
    public float cooldown = 1f;

    [Header("Detection Settings")]
    public float detectionRadius = 2f;

    [Header("Target Layer")]
    public LayerMask animalLayer;
    public LayerMask treeLayer;

    [Header("Drops")]
    public GameObject chickenFoodDropPrefab;
    public GameObject sheepFoodDropPrefab;
    public GameObject cowFoodDropPrefab;

    [Header("Tree Drops")]
    public GameObject woodDropPrefab;
    public GameObject saplingDropPrefab;
    [Range(0f, 1f)] public float saplingDropChance = 0.35f;

    [Header("UI")]
    public GameObject killPrompt;
    public TextMeshProUGUI promptText;

    private float lastAttackTime;
    private Rigidbody2D rb;
    private Vector2 facingDirection = Vector2.right;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (killPrompt != null)
            killPrompt.SetActive(false);
    }

    void Update()
    {
        UpdateFacingDirection();
        CheckForNearbyTargets();

        if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            if (Time.time >= lastAttackTime + cooldown)
            {
                Attack();
                lastAttackTime = Time.time;
            }
        }
    }

    void UpdateFacingDirection()
    {
        if (rb != null && rb.linearVelocity.sqrMagnitude > 0.01f)
            facingDirection = rb.linearVelocity.normalized;
    }

    void CheckForNearbyTargets()
    {
        bool somethingNearby = false;

        Collider2D[] animalHits = Physics2D.OverlapCircleAll(
            transform.position, detectionRadius, animalLayer);

        foreach (Collider2D hit in animalHits)
        {
            if (hit.CompareTag("Sheep") ||
                hit.CompareTag("Chicken") ||
                hit.CompareTag("Cow"))
            {
                somethingNearby = true;
                if (promptText != null)
                    promptText.text = "Press E to kill";
                break;
            }
        }

        if (!somethingNearby)
        {
            Collider2D[] treeHits = Physics2D.OverlapCircleAll(
                transform.position, detectionRadius, treeLayer);

            foreach (Collider2D hit in treeHits)
            {
                if (hit.CompareTag("Trees"))
                {
                    somethingNearby = true;
                    if (promptText != null)
                        promptText.text = "Press E to chop";
                    break;
                }
            }
        }

        if (killPrompt != null)
            killPrompt.SetActive(somethingNearby);
    }

    void Attack()
    {
        Collider2D[] animalHits = Physics2D.OverlapCircleAll(
            transform.position, attackRadius, animalLayer);

        foreach (Collider2D hit in animalHits)
        {
            if (hit.CompareTag("Chicken"))
            {
                Vector3 dropPos = hit.transform.position + new Vector3(0f, -0.2f, 0f);
                if (chickenFoodDropPrefab != null)
                    Instantiate(chickenFoodDropPrefab, dropPos, Quaternion.identity);

                if (SustainabilityManager.Instance != null)
                    SustainabilityManager.Instance.KillAnimal();

                if (GameStatsManager.Instance != null)
                    GameStatsManager.Instance.RecordAnimalKilled();

                Destroy(hit.gameObject);
                return;
            }
            else if (hit.CompareTag("Sheep"))
            {
                Vector3 dropPos = hit.transform.position + new Vector3(0f, -0.2f, 0f);
                if (sheepFoodDropPrefab != null)
                    Instantiate(sheepFoodDropPrefab, dropPos, Quaternion.identity);

                if (SustainabilityManager.Instance != null)
                    SustainabilityManager.Instance.KillAnimal();

                if (GameStatsManager.Instance != null)
                    GameStatsManager.Instance.RecordAnimalKilled();

                Destroy(hit.gameObject);
                return;
            }
            else if (hit.CompareTag("Cow"))
            {
                Vector3 dropPos = hit.transform.position + new Vector3(0f, -0.2f, 0f);
                if (cowFoodDropPrefab != null)
                    Instantiate(cowFoodDropPrefab, dropPos, Quaternion.identity);

                if (SustainabilityManager.Instance != null)
                    SustainabilityManager.Instance.KillAnimal();

                if (GameStatsManager.Instance != null)
                    GameStatsManager.Instance.RecordAnimalKilled();

                Destroy(hit.gameObject);
                return;
            }
        }

        Collider2D[] treeHits = Physics2D.OverlapCircleAll(
            transform.position, attackRadius, treeLayer);

        foreach (Collider2D hit in treeHits)
        {
            if (hit.CompareTag("Trees"))
            {
                Vector3 dropPos = hit.transform.position + new Vector3(0.2f, -0.13f, 0f);

                if (woodDropPrefab != null)
                    Instantiate(woodDropPrefab, dropPos, Quaternion.identity);

                if (saplingDropPrefab != null && Random.value < saplingDropChance)
                {
                    Vector3 saplingPos = dropPos + new Vector3(0.4f, 0f, 0f);
                    Instantiate(saplingDropPrefab, saplingPos, Quaternion.identity);
                }

                if (SustainabilityManager.Instance != null)
                    SustainabilityManager.Instance.CutTree();

                if (GameStatsManager.Instance != null)
                    GameStatsManager.Instance.RecordTreeCut();

                Destroy(hit.gameObject);
                return;
            }
        }
    }
}