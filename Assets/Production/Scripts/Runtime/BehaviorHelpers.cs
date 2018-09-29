using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BehaviorHelpers
{
    public static GameObject FindGameObjectByName(this MonoBehaviour self, string name)
    { 
        foreach (Transform child in self.GetComponentsInChildren<Transform>())
            if (child.name == name)
                return child.gameObject;

        return null;
    }
}