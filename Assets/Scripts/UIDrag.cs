using UnityEngine;
using UnityEngine.EventSystems;

public class UIDrag : MonoBehaviour, IDragHandler, IBeginDragHandler
{
    public RectTransform a;      // 드래그 가능한 물체, 항상 Canvas내에 위치 예: TextPanel
    public RectTransform b;      // 드래그되는 전체 UI 예: InventoryUI
    public Canvas canvas;

    private Vector2 canvasSize;
    private Vector2 halfSizeA;

    void Start()
    {
        if (b == null) b = GetComponent<RectTransform>();

        // Canvas RectTransform 크기 구하기
        RectTransform canvasRect = canvas.GetComponent<RectTransform>();
        canvasSize = canvasRect.sizeDelta;

        // 드래그 물체의 중심좌표
        halfSizeA = a.rect.size * 0.5f;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        // b의 위치를 마우스 움직임에 맞춰 이동
        b.anchoredPosition += eventData.delta / canvas.scaleFactor;

        // 드래그 하는 물체가 Canvas 안에 있도록 제한
        Vector2 aPos = a.position;  // a의 월드 좌표
        Vector2 localA;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            aPos,
            canvas.worldCamera,
            out localA);

        // a의 위치가 Canvas의 제한 범위를 벗어나지 않도록 보정
        float clampedX = Mathf.Clamp(localA.x, -canvasSize.x / 2 + halfSizeA.x, canvasSize.x / 2 - halfSizeA.x);
        float clampedY = Mathf.Clamp(localA.y, -canvasSize.y / 2 + halfSizeA.y, canvasSize.y / 2 - halfSizeA.y);

        // 만약 a의 위치가 제한 범위를 넘었으면 b를 보정
        Vector2 offset = new Vector2(clampedX - localA.x, clampedY - localA.y);
        b.anchoredPosition += offset;
    }
}
