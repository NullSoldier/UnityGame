using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerTarget : MonoBehaviour
{
    public GameObject Target;
    private ITriggerable[] receivers;

    private void Start()
    {
        if(!Target)
            throw new Exception("Target is required on TriggerTarget");

        receivers = Target.GetComponents<ITriggerable>();

        if (receivers.Length == 0)
            Debug.LogWarning("Trigger target contains no scripts that implement ITriggerable");
    }

    private void OnTriggerEnter(Collider other)
    {
        foreach (ITriggerable receiver in receivers)
            receiver.OnTrigger(other);
    }
}
