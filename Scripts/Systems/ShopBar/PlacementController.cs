using UnityEngine;

public class PlacementController : MonoBehaviour
{
    public static bool IsPlacing { get; private set; }

    private ShopItem selectedItem;
    private GameObject ghostObject;
    private Camera mainCam;

    void Start()
    {
        mainCam = Camera.main;
        UIEvents.OnItemSelected += StartPlacement;
        UIEvents.OnPlacementCancelled += CancelPlacement;
    }

    void OnDestroy()
    {
        UIEvents.OnItemSelected -= StartPlacement;
        UIEvents.OnPlacementCancelled -= CancelPlacement;
    }

    void Update()
    {
        if (selectedItem == null) return;

        Vector3 mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        if (ghostObject != null)
            ghostObject.transform.position = mousePos;

        if (Input.GetMouseButtonDown(0))
        {
            if (!UnityEngine.EventSystems.EventSystem.current
                .IsPointerOverGameObject())
                PlaceItem(mousePos);
        }

        if (Input.GetMouseButtonDown(1))
            CancelPlacement();
    }

    void StartPlacement(ShopItem item)
    {
        CancelPlacement();
        selectedItem = item;
        IsPlacing = true;

        if (item.prefab != null)
        {
            ghostObject = Instantiate(item.prefab);

            foreach (SpriteRenderer sr in
                ghostObject.GetComponentsInChildren<SpriteRenderer>())
            {
                Color c = sr.color;
                c.a = 0.5f;
                sr.color = c;
            }

            foreach (Collider2D c in
                ghostObject.GetComponentsInChildren<Collider2D>())
                c.enabled = false;
        }
    }

    void PlaceItem(Vector3 position)
    {
        if (selectedItem == null) return;

        bool canPlace = false;
        if (selectedItem.costType == ResourceType.Sapling)
            canPlace = SaplingSystem.Instance.UseSapling(selectedItem.costAmount);
        else if (selectedItem.costType == ResourceType.Wood)
            canPlace = WoodSystem.Instance.UseWood(selectedItem.costAmount);
        else
            canPlace = true;

        if (!canPlace) return;

        GameObject placed = Instantiate(selectedItem.prefab, position, Quaternion.identity);

        if (PlacedBuildingTracker.Instance != null)
            PlacedBuildingTracker.Instance.RegisterBuilding(placed);

        selectedItem.placedThisRound++;

        // Track stats
        if (GameStatsManager.Instance != null)
        {
            if (selectedItem.itemName == "Tree")
                GameStatsManager.Instance.RecordTreePlaced();

            if (selectedItem.category == ShopItemCategory.Structure)
                GameStatsManager.Instance.RecordBuildingPlaced();
        }

        // Sustainability
        if (SustainabilityManager.Instance != null)
        {
            if (selectedItem.itemName == "Tree")
                SustainabilityManager.Instance.PlantTree();

            SustainabilityManager.Instance.Increase(selectedItem.sustainabilityChange);
        }

        // XP
        if (XPManager.Instance != null)
            XPManager.Instance.AddXP(selectedItem.xpReward);

        UIEvents.OnResourceChanged?.Invoke();
        CancelPlacement();
    }

    void CancelPlacement()
    {
        if (ghostObject != null)
            Destroy(ghostObject);

        ghostObject = null;
        selectedItem = null;
        IsPlacing = false;
    }
}