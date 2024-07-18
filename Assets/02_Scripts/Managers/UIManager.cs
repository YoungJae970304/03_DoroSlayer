using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    public GameObject hpOb;
    public GameObject hpParent;

    public Image gauge;
    public GameObject panel;

    public TextMeshProUGUI moneyTxt;

    private void Start()
    {
        StartCoroutine(Count(Managers.Data._money, 0));

        for (int i = 0; i < Managers.Data.PlayerMaxLife; i++)
        {
            GameObject go = Instantiate(hpOb, hpParent.transform);
            go.transform.position += new Vector3(50f * i, 0);
            Managers.Data.hp.Push(go);
        }
    }

    public void BTN_GoMainScene()
    {
        Time.timeScale = 1;
        Managers.Data.PlayerLife = Managers.Data.PlayerMaxLife;
        Managers.Data.PlayerGage = Managers.Data.PlayerMaxGauge;
        SceneManager.LoadScene("01_MainScene");
    }

    public void BTN_GoTitle()
    {
        Time.timeScale = 1;
        Managers.Data.PlayerLife = Managers.Data.PlayerMaxLife;
        Managers.Data.PlayerGage = Managers.Data.PlayerMaxGauge;
        SceneManager.LoadScene("00_Title");
    }

    public void BTN_Quit()
    {
        UnityEditor.EditorApplication.isPlaying = false;
    }

    public void OpenOverPanel()
    {
        if (panel != null)
        {
            panel.SetActive(true);
        }
    }

    private void Update()
    {
        if (gauge != null)
        {
            float currentGauge = Managers.Data.PlayerGage / Managers.Data.PlayerMaxGauge;
            gauge.fillAmount = Mathf.Lerp(gauge.fillAmount, currentGauge, 0.01f);
        }
    }

    public void SetMoney(float money)
    {
        Managers.Data._money += money;    // 증가 이후 돈
        StartCoroutine(Count(Managers.Data._money, Managers.Data._money - money));  // _money - money = 증가되기 전 돈
    }

    IEnumerator Count(float target, float current)
    {
        float duration = 0.5f;  // 카운팅에 걸리는 시간
        float offset = (target - current) / duration;

        while (current < target)
        {
            current += offset * Time.deltaTime;

            moneyTxt.text = string.Format("{0:n0}", (int)current);
            yield return null;
        }

        current = target;
        moneyTxt.text = string.Format("{0:n0}", (int)current);
    }
}
