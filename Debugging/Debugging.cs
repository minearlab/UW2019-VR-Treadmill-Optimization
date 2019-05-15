using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debugging : MonoBehaviour {

    [SerializeField] private bool m_EnableDebugLogging = false;

	// Use this for initialization
	void Awake () {
        DebugLogger.Init();
        DebugLogger.enableDebugging = m_EnableDebugLogging;
        if(!DebugLogger.enableDebugging && m_EnableDebugLogging)
        {
            Debug.LogError("Debugging Failed To Start!");
        }
	}
	
}
