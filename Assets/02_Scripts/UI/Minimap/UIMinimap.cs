using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIMinimap : MonoBehaviour
{
    [SerializeField]
    Camera minimapCamera;   // 미니맵으로 사용할 카메라 (메인카메라x)
    [SerializeField]
    float zoomMin = 1;      // 카메라 orthographicSize 최소값 
    [SerializeField]
    float zoomMax = 30;     // 카메라 orthographicSize 최대값  
    [SerializeField]
    float zoomOneStep = 1;  // 한번에 증감시킬 orthographicSize 값

    public void ZoonIn()
    {
        // 카메라의 orthographicSize 값을 감소시켜 카메라에 보이는 사물 크기 확대
        minimapCamera.orthographicSize =
            Mathf.Max(minimapCamera.orthographicSize - zoomOneStep, zoomMin);
    }

    public void ZoomOut()
    {
        // 반대
        minimapCamera.orthographicSize =
            Mathf.Min(minimapCamera.orthographicSize + zoomOneStep, zoomMax);
    }
}
