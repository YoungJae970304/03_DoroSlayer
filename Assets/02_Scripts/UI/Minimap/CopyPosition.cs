using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyPosition : MonoBehaviour
{
    [SerializeField]
    private bool x, y, z;
    // 이 값이 true면 target의 좌표, false면 현재좌표를 그대로 사용

    [SerializeField]
    Transform target;   // 쫓아가야할 대상 Transform

    void Update()
    {
        if (!target) return;

        transform.position = new Vector3(
            (x ? target.position.x : transform.position.x),
            (y ? target.position.y : transform.position.y),
            (z ? target.position.z : transform.position.z));
    }
}