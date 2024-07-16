using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �Ŵ������� �Ѱ��ϴ� ��ũ��Ʈ
public class Managers : MonoBehaviour
{
    static Managers s_instance; // ���ϼ� ����
    // ������ �Ŵ����� ������ // ������Ƽ // �б� ����
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

            if (go == null)  // go�� ������
            {
                go = new GameObject { name = "@Managers" }; // �ڵ������ ������Ʈ�� �������
                go.AddComponent<Managers>();
            }

            DontDestroyOnLoad(go);
            s_instance = go.GetComponent<Managers>();
        }
    }
}
