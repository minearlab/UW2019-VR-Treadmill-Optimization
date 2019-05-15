using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Written by Adrian Barberis for Dr. Meredith Minear, University of Wyoming, 7/09/18
//
// Sets an object's tag to [m_Tag] whenever the object becomes enabled

public class SetTag : MonoBehaviour {

    public string m_Tag = "Untagged";
    private void OnEnable()
    {
        gameObject.tag = m_Tag;
        DebugLogger.Log("[SetTag.cs] :: [" + gameObject.name + "] tagged with [" + m_Tag + "]\r\n");
    }
}
