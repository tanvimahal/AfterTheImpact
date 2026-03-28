using UnityEngine;
using UnityEngine.EventSystems;

public class ScrollBlocker : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static bool IsMouseOverShop = false;

    public void OnPointerEnter(PointerEventData eventData)
    {
        IsMouseOverShop = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        IsMouseOverShop = false;
    }

    private void OnDisable()
    {
        IsMouseOverShop = false;
    }
}