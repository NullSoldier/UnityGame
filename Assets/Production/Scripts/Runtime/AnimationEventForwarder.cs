using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAnimationEventReceiver
{
    void OnAnimationEvent(AnimationEvent ev);

}
public class AnimationEventForwarder : MonoBehaviour
{
    public GameObject Target;
    private IAnimationEventReceiver[] receivers;

    private void Start()
    {
        if (!Target)
            throw new Exception("Target is required on TriggerTarget");

        receivers = Target.GetComponents<IAnimationEventReceiver>();

        if (receivers.Length == 0)
            Debug.LogWarning("Target contains no scripts that implement ITriggerable");
    }

    public void OnEvent(AnimationEvent ev)
    {
        foreach(IAnimationEventReceiver receiver in receivers)
            receiver.OnAnimationEvent(ev);
    }
}
