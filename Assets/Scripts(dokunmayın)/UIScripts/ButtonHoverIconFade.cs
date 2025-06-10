using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHoverIconFade : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image iconImage;
    public float fadeDuration = 0.3f;

    private Coroutine fadeCoroutine;

    public void OnPointerEnter(PointerEventData eventData)
    {
        StartFade(1f); // Görünür hale getir
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StartFade(0f); // Kaybol
    }

    void StartFade(float targetAlpha)
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeIcon(targetAlpha));
    }

    System.Collections.IEnumerator FadeIcon(float targetAlpha)
    {
        float startAlpha = iconImage.color.a;
        float time = 0f;

        while (time < fadeDuration)
        {
            float t = time / fadeDuration;
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, t);
            Color newColor = iconImage.color;
            newColor.a = alpha;
            iconImage.color = newColor;
            time += Time.deltaTime;
            yield return null;
        }

        Color finalColor = iconImage.color;
        finalColor.a = targetAlpha;
        iconImage.color = finalColor;
    }
}
