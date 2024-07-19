using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIMinimap : MonoBehaviour
{
    [SerializeField]
    Camera minimapCamera;   // �̴ϸ����� ����� ī�޶� (����ī�޶�x)
    [SerializeField]
    float zoomMin = 1;      // ī�޶� orthographicSize �ּҰ� 
    [SerializeField]
    float zoomMax = 30;     // ī�޶� orthographicSize �ִ밪  
    [SerializeField]
    float zoomOneStep = 1;  // �ѹ��� ������ų orthographicSize ��

    public void ZoonIn()
    {
        // ī�޶��� orthographicSize ���� ���ҽ��� ī�޶� ���̴� �繰 ũ�� Ȯ��
        minimapCamera.orthographicSize =
            Mathf.Max(minimapCamera.orthographicSize - zoomOneStep, zoomMin);
    }

    public void ZoomOut()
    {
        // �ݴ�
        minimapCamera.orthographicSize =
            Mathf.Min(minimapCamera.orthographicSize + zoomOneStep, zoomMax);
    }
}
