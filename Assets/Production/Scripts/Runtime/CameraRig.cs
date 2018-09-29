using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraRig : MonoBehaviour, ITriggerable
{
    public float TransitionSpeed = 2f;

    private Camera rigCamera;
    private GameObject rigCameraContainer;
    private CameraManager cameraManager;
    private GameObject dummyTarget;
    private GameObject trigger;

    void Start ()
	{
        cameraManager = FindObjectOfType<CameraManager>();
        rigCamera = GetComponentInChildren<Camera>();
        rigCameraContainer = this.FindGameObjectByName("camera_container");
        dummyTarget = this.FindGameObjectByName("target");
        trigger = this.FindGameObjectByName("trigger");

        if (cameraManager == null)
            throw new Exception("CameraManager required on CameraRig");
        if (rigCamera == null)
            throw new Exception("Camera required on CameraRig");
        if (rigCameraContainer == null)
            throw new Exception("Cannot find camera_container in CameraRig");
        if (dummyTarget == null)
            throw new Exception("Cannot find target in CameraRig");
        if (trigger == null)
            throw new Exception("Cannot find trigger in CameraRig");

        dummyTarget.SetActive(false);
        trigger.GetComponent<MeshRenderer>().enabled = false;
	}

    public void OnTrigger(Collider other)
    {
        Vector3 offset = rigCameraContainer.transform.localPosition;
        cameraManager.TransitionTo(this, offset, TransitionSpeed);
    }
}
