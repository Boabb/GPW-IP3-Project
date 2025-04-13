using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MontageImageFade : MonoBehaviour
{
    public float fadeDuration = 1.0f;          // How quickly to fade in/out
    public float centreThreshold = 0.05f;      // How close to screen centre before fading starts

    private Image image;
    private Canvas canvas;
    private float canvasWidth;

    private bool hasFadedOut = false;

    void Start()
    {
        image = GetComponent<Image>();
        canvas = GetComponentInParent<Canvas>();

        if (canvas != null)
        {
            canvasWidth = canvas.pixelRect.width;
        }
    }

    void Update()
    {
        if (image == null || canvas == null || canvasWidth == 0f)
            return;

        Vector3 screenPos = image.rectTransform.position;
        float normalisedX = Mathf.Clamp01(screenPos.x / canvasWidth);

        // Fade out when near the centre
        if (Mathf.Abs(normalisedX - 0.5f) <= centreThreshold && !hasFadedOut)
        {
            StartCoroutine(FadeOut());
            hasFadedOut = true;
        }
    }

    private IEnumerator FadeOut()
    {
        float currentAlpha = image.color.a;
        float timeElapsed = 0f;

        while (timeElapsed < fadeDuration)
        {
            timeElapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(currentAlpha, 0f, timeElapsed / fadeDuration);
            SetAlpha(alpha);
            yield return null;
        }

        SetAlpha(0f); // Ensure it reaches 0 at the end
    }

    private void SetAlpha(float alpha)
    {
        Color newColour = image.color;
        newColour.a = alpha;
        image.color = newColour;
    }
}
