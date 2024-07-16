using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 매니저들을 총괄하는 스크립트
public class Managers : MonoBehaviour
{
    static Managers s_instance; // 유일성 보장
    // 유일한 매니저를 가져옴 // 프로퍼티 // 읽기 전용
    public static Managers Instance { get { Init(); return s_instance; } }

    DataManager _data = new DataManager();
    public static DataManager Data { get { return Instance._data; } }

    UIManager _ui = new UIManager();
    public static UIManager UI { get { return Instance._ui; } }

    void Start()
    {
        Init();
    }

    static void Init()
    {
        if (s_instance == null)
        {
            GameObject go = GameObject.Find("@Managers");

            if (go == null)  // go가 없으면
            {
                go = new GameObject { name = "@Managers" }; // 코드상으로 오브젝트를 만들어줌
                go.AddComponent<Managers>();
            }

            DontDestroyOnLoad(go);
            s_instance = go.GetComponent<Managers>();
        }
    }
}
