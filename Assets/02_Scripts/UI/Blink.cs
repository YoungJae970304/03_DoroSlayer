using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Blink : MonoBehaviour
{
    [SerializeField]
    private float fadeTime = 1f;     // 페이드 되는 시간
    private TextMeshProUGUI fadeImage;    // 페이드 효과에 사용되는 이미지 UI

    private void Awake()
    {
        fadeImage = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        // Fade 효과를 In -> Out 무한 반복
        StartCoroutine("FadeInOut");
    }

    private void OnDisable()
    {
        StopCoroutine("FadeInOut");
    }

    IEnumerator FadeInOut()
    {
        while (true)
        {
            yield return StartCoroutine(Fade(1, 0));    //페이드 인

            yield return StartCoroutine(Fade(0, 1));    //페이드 아웃
        }
    }

    IEnumerator Fade(float start, float end)
    {
        float current = 0;
        float percent = 0;

        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current / fadeTime;

            Color color = fadeImage.color;
            color.a = Mathf.Lerp(start, end, percent);
            fadeImage.color = color;

            yield return null;
        }
    }
}
