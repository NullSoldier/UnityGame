using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LookAtInEditor : MonoBehaviour
{
    public Transform target;

    void Start()
    {
        enabled = Application.isEditor;

        if (target == null)
            throw new Exception("target required");
    }

    void Update()
    {
        transform.localPosition = Vector3.zero;
        transform.LookAt(target);
    }
}
