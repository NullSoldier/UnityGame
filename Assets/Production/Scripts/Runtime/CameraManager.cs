using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public CameraMain mainCamera;
    private CameraRig currentRig;

	void Start()
    {
        mainCamera = FindObjectOfType<CameraMain>();

        if (mainCamera == null)
            throw new Exception("main is required");
    }

    public void TransitionTo(CameraRig rig, Vector3 offset, float speed)
    {
        if (rig == currentRig)
            return;

        mainCamera.TransitionTo(offset, speed);
        currentRig = rig;
    }
}
