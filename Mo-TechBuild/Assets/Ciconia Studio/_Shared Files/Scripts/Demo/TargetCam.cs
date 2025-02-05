using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class TargetCam : MonoBehaviour
{
    public GameObject target;

    void Update()
    {
        transform.LookAt(target.transform);
    }
}
