using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Inputs;


public class TestControllerMapping : MonoBehaviour {

    public enum UUT { LogitechExtreme3DPro, Keyboard, HTCViveWand, None }
    public UUT unitUnderTest;
	// Use this for initialization

	
	// Update is called once per frame
	void Update ()
    {
        if(unitUnderTest == UUT.LogitechExtreme3DPro)
        {
            if (LogitechExtreme3DPro.StickY(AxisState.Up) != 0) { Debug.Log("Stick Up"); }
            if (LogitechExtreme3DPro.StickY(AxisState.Down) != 0) { Debug.Log("Stick Down"); }
            if (LogitechExtreme3DPro.StickX(AxisState.Left) != 0) { Debug.Log("Stick Left"); }
            if (LogitechExtreme3DPro.StickX(AxisState.Right) != 0) { Debug.Log("Stick Right"); }
            if (LogitechExtreme3DPro.StickRotate(AxisState.Left) != 0) { Debug.Log("Stick Rotate Left"); }
            if (LogitechExtreme3DPro.StickRotate(AxisState.Right) != 0) { Debug.Log("Stick Rotate Right"); }
            if (LogitechExtreme3DPro.HatY(AxisState.Up) != 0) { Debug.Log("Hat UP"); }
            if (LogitechExtreme3DPro.HatY(AxisState.Down) != 0) { Debug.Log("Hat DOWN"); }
            if (LogitechExtreme3DPro.HatX(AxisState.Left) != 0) { Debug.Log("Hat LEFT"); }
            if (LogitechExtreme3DPro.HatX(AxisState.Right) != 0) { Debug.Log("Hat RIGHT"); }
            if (LogitechExtreme3DPro.Throttle(AxisState.Positive) != 0) { Debug.Log("Throttle Positive"); }
            if(LogitechExtreme3DPro.Throttle(AxisState.Negative) != 0) { Debug.Log("Throttle Negative"); }
            if (LogitechExtreme3DPro.Trigger(ButtonState.Pressed)) { Debug.Log("Trigger Pressed"); }
            if (LogitechExtreme3DPro.Trigger(ButtonState.Held)) { Debug.Log("Trigger Held"); }
            if (LogitechExtreme3DPro.Trigger(ButtonState.Released)) { Debug.Log("Trigger Released"); }
            if (LogitechExtreme3DPro.Button2(ButtonState.Pressed)) { Debug.Log("Button2 Pressed"); }
            if (LogitechExtreme3DPro.Button2(ButtonState.Held)) { Debug.Log("Button2 Held"); }
            if (LogitechExtreme3DPro.Button2(ButtonState.Released)) { Debug.Log("Button2 Released"); }
            if (LogitechExtreme3DPro.Button3(ButtonState.Pressed)) { Debug.Log("Button3 Pressed"); }
            if (LogitechExtreme3DPro.Button3(ButtonState.Held)) { Debug.Log("Button3 Held"); }
            if (LogitechExtreme3DPro.Button3(ButtonState.Released)) { Debug.Log("Button3 Released"); }
            if (LogitechExtreme3DPro.Button4(ButtonState.Pressed)) { Debug.Log("Button4 Pressed"); }
            if (LogitechExtreme3DPro.Button4(ButtonState.Held)) { Debug.Log("Button4 Held"); }
            if (LogitechExtreme3DPro.Button4(ButtonState.Released)) { Debug.Log("Button4 Released"); }
            if (LogitechExtreme3DPro.Button5(ButtonState.Pressed)) { Debug.Log("Button5 Pressed"); }
            if (LogitechExtreme3DPro.Button5(ButtonState.Held)) { Debug.Log("Button5 Held"); }
            if (LogitechExtreme3DPro.Button5(ButtonState.Released)) { Debug.Log("Button5 Released"); }
            if (LogitechExtreme3DPro.Button6(ButtonState.Pressed)) { Debug.Log("Button6 Pressed"); }
            if (LogitechExtreme3DPro.Button6(ButtonState.Held)) { Debug.Log("Button6 Held"); }
            if (LogitechExtreme3DPro.Button6(ButtonState.Released)) { Debug.Log("Button6 Released"); }
            if (LogitechExtreme3DPro.Button7(ButtonState.Pressed)) { Debug.Log("Button7 Pressed"); }
            if (LogitechExtreme3DPro.Button7(ButtonState.Held)) { Debug.Log("Button7 Held"); }
            if (LogitechExtreme3DPro.Button7(ButtonState.Released)) { Debug.Log("Button7 Released"); }
            if (LogitechExtreme3DPro.Button8(ButtonState.Pressed)) { Debug.Log("Button8 Pressed"); }
            if (LogitechExtreme3DPro.Button8(ButtonState.Held)) { Debug.Log("Button8 Held"); }
            if (LogitechExtreme3DPro.Button8(ButtonState.Released)) { Debug.Log("Button8 Released"); }
            if (LogitechExtreme3DPro.Button9(ButtonState.Pressed)) { Debug.Log("Button9 Pressed"); }
            if (LogitechExtreme3DPro.Button9(ButtonState.Held)) { Debug.Log("Button9 Held"); }
            if (LogitechExtreme3DPro.Button9(ButtonState.Released)) { Debug.Log("Button9 Released"); }
            if (LogitechExtreme3DPro.Button10(ButtonState.Pressed)) { Debug.Log("Button10 Pressed"); }
            if (LogitechExtreme3DPro.Button10(ButtonState.Held)) { Debug.Log("Button10 Held"); }
            if (LogitechExtreme3DPro.Button10(ButtonState.Released)) { Debug.Log("Button10 Released"); }
            if (LogitechExtreme3DPro.Button11(ButtonState.Pressed)) { Debug.Log("Button11 Pressed"); }
            if (LogitechExtreme3DPro.Button11(ButtonState.Held)) { Debug.Log("Button11 Held"); }
            if (LogitechExtreme3DPro.Button11(ButtonState.Released)) { Debug.Log("Button11 Released"); }
            if (LogitechExtreme3DPro.Button12(ButtonState.Pressed)) { Debug.Log("Button12 Pressed"); }
            if (LogitechExtreme3DPro.Button12(ButtonState.Held)) { Debug.Log("Button12 Held"); }
            if (LogitechExtreme3DPro.Button12(ButtonState.Released)) { Debug.Log("Button12 Released"); }
        }
        else if(unitUnderTest == UUT.HTCViveWand)
        {

        }
        else if(unitUnderTest == UUT.Keyboard)
        {

        }

    }
}
