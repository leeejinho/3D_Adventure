using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageIndicator : MonoBehaviour
{
    public float FlashSpeed;

    Image image;
    Coroutine coroutine;
    float startAlpha;

    private void Start()
    {
        GameManager.Instance.player.condition.OnTakeDamageIndicator += Flash;
        image = GetComponent<Image>();

        startAlpha = image.color.a;
    }

    void Flash()
    { 
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }

        image.enabled = true;
        coroutine = StartCoroutine(FadeAway());
    }

    IEnumerator FadeAway()
    {
        float curAlpha = startAlpha;
        float elapsedTime = 0f;

        while (curAlpha > 0f)
        {
            elapsedTime += Time.deltaTime;
            curAlpha = Mathf.Lerp(curAlpha, 0f, elapsedTime / FlashSpeed);
            image.color = new Color(image.color.r, image.color.g, image.color.b, curAlpha);

            yield return null;
        }

        image.enabled = false;
    }
}
