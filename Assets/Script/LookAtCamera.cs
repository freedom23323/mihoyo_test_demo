using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    void LateUpdate()
    {
        // 让 UI 容器的朝向永远实时对齐主相机
        if (Camera.main != null)
        {
            transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward,
                             Camera.main.transform.rotation * Vector3.up);
        }
    }
}