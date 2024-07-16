using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public void BTN_GoMainScene()
    {
        SceneManager.LoadScene("01_MainScene");
    }

    public void BTN_Quit()
    {
        
    }
}
