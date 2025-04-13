using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParallaxForImage : MonoBehaviour
{
    public float maxOffset = 500f; // Maximum horizontal offset
    private RectTransform rectTransform;
    private Canvas canvas;
    private float canvasWidth;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();

        if (canvas != null)
        {
            canvasWidth = canvas.pixelRect.width;
        }
    }

    void Update()
    {
        if (rectTransform == null || canvasWidth == 0f)
            return;

        // Get screen position
        Vector3 screenPos = rectTransform.position;

        // Normalize screen position X from 0 to 1
        float normalizedX = Mathf.Clamp01(screenPos.x / canvasWidth);

        // Convert to range -maxOffset to +maxOffset
        float offset = Mathf.Lerp(-maxOffset, maxOffset, normalizedX);

        // Apply offset to the anchoredPosition
        Vector2 newAnchoredPosition = rectTransform.anchoredPosition;
        newAnchoredPosition.x = offset;
        rectTransform.anchoredPosition = newAnchoredPosition;
    }
}
