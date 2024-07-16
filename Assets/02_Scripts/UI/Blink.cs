using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Blink : MonoBehaviour
{
    [SerializeField]
    private float fadeTime = 1f;     // ���̵� �Ǵ� �ð�
    private TextMeshProUGUI fadeImage;    // ���̵� ȿ���� ���Ǵ� �̹��� UI

    private void Awake()
    {
        fadeImage = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        // Fade ȿ���� In -> Out ���� �ݺ�
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
            yield return StartCoroutine(Fade(1, 0));    //���̵� ��

            yield return StartCoroutine(Fade(0, 1));    //���̵� �ƿ�
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
