using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class CameraMain : MonoBehaviour
{
    public Transform Target;

    private bool firstCamera = true;
    private float lerpTimeSeconds;
    private float totalLerpSeconds;
    private Vector3 cameraOffset;
    private Camera internalCamera;

    public void TransitionTo(Vector3 offset, float timeSeconds)
    {
        lerpTimeSeconds = firstCamera ? timeSeconds : 0f;
        totalLerpSeconds = timeSeconds;
        cameraOffset = offset;
        firstCamera = false;
        enabled = true;
    }

    void Start()
    {
        internalCamera = GetComponentInChildren<Camera>();

        if (internalCamera == null)
            throw new Exception("Camera not found in CameraMain");

        if (Target == null)
	        throw new Exception("Target is required on CameraFollow");

        enabled = false;
    }

	void Update()
	{
        if (lerpTimeSeconds < totalLerpSeconds)
            lerpTimeSeconds += Time.deltaTime;

	    Vector3 destPosition = Target.transform.position + cameraOffset;
	    transform.position = Vector3.Lerp(transform.position, destPosition, lerpTimeSeconds / totalLerpSeconds);

        internalCamera.transform.LookAt(Target);
	}
}