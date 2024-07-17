using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public Image gauge;
    
    public void BTN_GoMainScene()
    {
        SceneManager.LoadScene("01_MainScene");
    }

    public void BTN_Quit()
    {
        UnityEditor.EditorApplication.isPlaying = false;
    }

    private void Update()
    {
        float currentGauge = Managers.Data.PlayerGage / Managers.Data.PlayerMaxGauge;
        gauge.fillAmount = Mathf.Lerp(gauge.fillAmount, currentGauge, 0.01f);
    }
}
