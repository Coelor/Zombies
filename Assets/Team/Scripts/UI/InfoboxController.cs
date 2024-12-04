using UnityEngine;
using UnityEngine.UI;

public class InfoboxController : MonoBehaviour
{
    public RectTransform canvasRectTransform;
    public RectTransform infoBoxRectTransform;

    void Update()
    {
        ClampToTopWhenNeeded();
    }

    void ClampToTopWhenNeeded()
    {
        if (canvasRectTransform == null || infoBoxRectTransform == null)
            return;

        LayoutRebuilder.ForceRebuildLayoutImmediate(infoBoxRectTransform);

        Vector3[] corners = new Vector3[4];
        infoBoxRectTransform.GetWorldCorners(corners);

        Vector3 topCorner = corners[1];

        Vector2 topCornerLocalPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, topCorner, null, out topCornerLocalPos);

        float overflowHeight = topCornerLocalPos.y - canvasRectTransform.rect.height / 2;

        if (overflowHeight > 0)
        {
            Vector3 newPosition = infoBoxRectTransform.localPosition;
            newPosition.y -= overflowHeight;
            infoBoxRectTransform.localPosition = newPosition;
        }
    }
}