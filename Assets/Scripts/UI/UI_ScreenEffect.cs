using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_ScreenEffect : MonoBehaviour
{
    public Coroutine fadeEffectCo { get; private set; }
    private Image fadeEffectImage;

    private void Awake()
    {
        fadeEffectImage = GetComponent<Image>();
        fadeEffectImage.color = new Color(0, 0, 0, 1);
    }

    public void FadeIn(float duration = 1) // black to transparent
    {
        fadeEffectImage.color = new Color(0, 0, 0, 1);
        FadeEffect(0f, duration);
    }

    public void FadeOut(float duration = 1) // transparent to black
    {
        fadeEffectImage.color = new Color(0, 0, 0, 0);
        FadeEffect(1f, duration);
    }

    private void FadeEffect(float targetAlpha, float duration)
    {
        if (fadeEffectCo != null)
            StopCoroutine(fadeEffectCo);

        fadeEffectCo = StartCoroutine(FadeEffectCo(targetAlpha, duration));
    }

    private IEnumerator FadeEffectCo(float targetAlpha, float duration)
    {
        float startAlpha = fadeEffectImage.color.a;
        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;

            var color = fadeEffectImage.color;
            color.a = Mathf.Lerp(startAlpha, targetAlpha, time / duration);

            fadeEffectImage.color = color;

            yield return null;
        }

        fadeEffectImage.color = new Color(fadeEffectImage.color.r, fadeEffectImage.color.g, fadeEffectImage.color.b, targetAlpha);
    }
}
