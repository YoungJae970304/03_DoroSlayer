using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public Image gauge;
    public GameObject panel;

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
}
