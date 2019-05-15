////////////////////////////////////////////////////////
// AUTHOR:      Adrian T. Barberis
// CREATED:     Feb. 27, 2019
// LAST EDIT:   Feb. 28, 2019
/////////////////////////////////////////////////////////



/* 
 * Input handler for the Logitech Extreme 3D Pro Joystick
 *********************************************************/






//////////////////////////////\\\\\\\\\\\\\\\\\\\\\\\\\\\
/////               INPUT MANAGER MAPPNGS            \\\\\
//\\\\\\\\\\\\\\\\\\\\\\\\\\\\\///////////////////////////
/*
 * +-----------------------------------------------------+
 * |                        Axes                         |
 * +-----------------------------------------------------+
 * Stick Up             =                 Y-Axis < 0
 * Stick Down           =                 Y-Axis > 0
 * 
 * Stick Left           =                 X-Axis < 0
 * Stick Right          =                 X-Axis > 0
 * 
 * Stick Rotate Left    =                 3rd-Axis < 0
 * Stick Rotate Right   =                 3rd-Axis > 0
 * 
 * Hat Up               =                 6th-Axis > 0
 * Hat Down             =                 6th-Axis < 0
 * 
 * Hat Left             =                 5th-Axis < 0
 * Hat Right            =                 5th-Axis > 0
 * 
 * Throttle Up          =                 4th-Axis < 0
 * Throttle Down        =                 4th-Axis > 0
 * 
 * 
 * +-----------------------------------------------------+
 * |                     BUTTONS                         |
 * +-----------------------------------------------------+
 * Button 1 (Trigger)       =       joystick button 0 
 * Button 2                 =       joystick button 1 
 * Button 3                 =       joystick button 2 
 * Button 4                 =       joystick button 3 
 * Button 5                 =       joystick button 4 
 * Button 6                 =       joystick button 5 
 * Button 7                 =       joystick button 6 
 * Button 8                 =       joystick button 7 
 * Button 9                 =       joystick button 8 
 * Button 10                =       joystick button 9 
 * Button 11                =       joystick button 10
 * Button 12                =       joystick button 11
 */


// WARNING!:
//
// You'll have to add the inputs to the Unity Input manager yourself!
// There is no way (yet) to edit the Input Manager via script.
//////////////////////////////////////////////////////////////////////


using UnityEngine;

namespace Inputs
{
    public enum AxisState { Up, Down, Left, Right, Positive, Negative, Raw }
    public enum ButtonState { Pressed, Held, Released }

    public static class LogitechExtreme3DPro
    {

        // +-----------------------------------------------------+
        // |                        Axes                         |
        // +-----------------------------------------------------+
        public static float StickY(AxisState state)
        {
            switch (state)
            {
                case AxisState.Up:
                    {
                        var input = Input.GetAxis("Extreme3DPro_StickUD");
                        if (input < 0) return input;
                        else return 0;
                    }
                case AxisState.Down:
                    {
                        var input = Input.GetAxis("Extreme3DPro_StickUD");
                        if (input > 0) return input;
                        else return 0;
                    }
                case AxisState.Raw:
                    {
                        return Input.GetAxis("Extreme3DPro_StickUD");
                    }
                default: return Input.GetAxis("Extreme3DPro_StickUD");
            }
        }
        public static float StickX(AxisState state)
        {
            switch (state)
            {
                case AxisState.Left:
                    {
                        var input = Input.GetAxis("Extreme3DPro_StickLR");
                        if (input < 0) return input;
                        else return 0;
                    }
                case AxisState.Right:
                    {
                        var input = Input.GetAxis("Extreme3DPro_StickLR");
                        if (input > 0) return input;
                        else return 0;
                    }
                case AxisState.Raw:
                    {
                        return Input.GetAxis("Extreme3DPro_StickLR");
                    }
                default: return Input.GetAxis("Extreme3DPro_StickLR");
            }
        }
        public static float StickRotate(AxisState state)
        {
            switch (state)
            {
                case AxisState.Left:
                    {
                        var input = Input.GetAxis("Extreme3DPro_StickRotate");
                        if (input < 0) return input;
                        else return 0;
                    }
                case AxisState.Right:
                    {
                        var input = Input.GetAxis("Extreme3DPro_StickRotate");
                        if (input > 0) return input;
                        else return 0;
                    }
                case AxisState.Raw:
                    {
                        return Input.GetAxis("Extreme3DPro_StickRotate");
                    }
                default: return Input.GetAxis("Extreme3DPro_StickRotate");
            }
        }
        public static float HatY(AxisState state)
        {
            switch (state)
            {
                case AxisState.Up:
                    {
                        var input = Input.GetAxis("Extreme3DPro_HatUD");
                        if (input < 0) return input;
                        else return 0;
                    }
                case AxisState.Down:
                    {
                        var input = Input.GetAxis("Extreme3DPro_HatUD");
                        if (input > 0) return input;
                        else return 0;
                    }
                case AxisState.Raw:
                    {
                        return Input.GetAxis("Extreme3DPro_HatUD");
                    }
                default: return Input.GetAxis("Extreme3DPro_HatUD");
            }
        }
        public static float HatX(AxisState state)
        {
            switch (state)
            {
                case AxisState.Left:
                    {
                        var input = Input.GetAxis("Extreme3DPro_HatLR");
                        if (input < 0) return input;
                        else return 0;
                    }
                case AxisState.Right:
                    {
                        var input = Input.GetAxis("Extreme3DPro_HatLR");
                        if (input > 0) return input;
                        else return 0;
                    }
                case AxisState.Raw:
                    {
                        return Input.GetAxis("Extreme3DPro_HatLR");
                    }
                default: return Input.GetAxis("Extreme3DPro_HatLR");
            }
        }
        public static float Throttle(AxisState state)
        {
            switch (state)
            {
                case AxisState.Positive:
                    {
                        var input = Input.GetAxis("Extreme3DPro_Throttle");
                        if (input < 0) return input;
                        else return 0;
                    }
                case AxisState.Negative:
                    {
                        var input = Input.GetAxis("Extreme3DPro_Throttle");
                        if (input > 0) return input;
                        else return 0;
                    }
                default: return 0;
            }

        }



        // +-----------------------------------------------------+
        // |                        Buttons                      |
        // +-----------------------------------------------------+
        public static bool Trigger(ButtonState state)
        {
            switch (state)
            {
                case ButtonState.Pressed:
                    {
                        var input = Input.GetKeyDown("joystick button 0");
                        if (input) return true;
                        else return false;
                    }
                case ButtonState.Held:
                    {
                        var input = Input.GetKey("joystick button 0");
                        if (input) return true;
                        else return false;
                    }
                case ButtonState.Released:
                    {
                        var input = Input.GetKeyUp("joystick button 0");
                        if (input) return true;
                        else return false;
                    }
                default:
                    {
                        var input = Input.GetKeyDown("joystick button 0");
                        if (input) return true;
                        else return false;
                    }
            }

        }
        public static bool Button2(ButtonState state)
        {
            switch (state)
            {
                case ButtonState.Pressed:
                    {
                        var input = Input.GetKeyDown("joystick button 1");
                        if (input) return true;
                        else return false;
                    }
                case ButtonState.Held:
                    {
                        var input = Input.GetKey("joystick button 1");
                        if (input) return true;
                        else return false;
                    }
                case ButtonState.Released:
                    {
                        var input = Input.GetKeyUp("joystick button 1");
                        if (input) return true;
                        else return false;
                    }
                default:
                    {
                        var input = Input.GetKeyDown("joystick button 1");
                        if (input) return true;
                        else return false;
                    }
            }
        }
        public static bool Button3(ButtonState state)
        {
            switch (state)
            {
                case ButtonState.Pressed:
                    {
                        var input = Input.GetKeyDown("joystick button 2");
                        if (input) return true;
                        else return false;
                    }
                case ButtonState.Held:
                    {
                        var input = Input.GetKey("joystick button 2");
                        if (input) return true;
                        else return false;
                    }
                case ButtonState.Released:
                    {
                        var input = Input.GetKeyUp("joystick button 2");
                        if (input) return true;
                        else return false;
                    }
                default:
                    {
                        var input = Input.GetKeyDown("joystick button 2");
                        if (input) return true;
                        else return false;
                    }
            }

        }
        public static bool Button4(ButtonState state)
        {
            switch (state)
            {
                case ButtonState.Pressed:
                    {
                        var input = Input.GetKeyDown("joystick button 3");
                        if (input) return true;
                        else return false;
                    }
                case ButtonState.Held:
                    {
                        var input = Input.GetKey("joystick button 3");
                        if (input) return true;
                        else return false;
                    }
                case ButtonState.Released:
                    {
                        var input = Input.GetKeyUp("joystick button 3");
                        if (input) return true;
                        else return false;
                    }
                default:
                    {
                        var input = Input.GetKeyDown("joystick button 3");
                        if (input) return true;
                        else return false;
                    }
            }
        }
        public static bool Button5(ButtonState state)
        {
            switch (state)
            {
                case ButtonState.Pressed:
                    {
                        var input = Input.GetKeyDown("joystick button 4");
                        if (input) return true;
                        else return false;
                    }
                case ButtonState.Held:
                    {
                        var input = Input.GetKey("joystick button 4");
                        if (input) return true;
                        else return false;
                    }
                case ButtonState.Released:
                    {
                        var input = Input.GetKeyUp("joystick button 4");
                        if (input) return true;
                        else return false;
                    }
                default:
                    {
                        var input = Input.GetKeyDown("joystick button 4");
                        if (input) return true;
                        else return false;
                    }
            }
        }
        public static bool Button6(ButtonState state)
        {
            switch (state)
            {
                case ButtonState.Pressed:
                    {
                        var input = Input.GetKeyDown("joystick button 5");
                        if (input) return true;
                        else return false;
                    }
                case ButtonState.Held:
                    {
                        var input = Input.GetKey("joystick button 5");
                        if (input) return true;
                        else return false;
                    }
                case ButtonState.Released:
                    {
                        var input = Input.GetKeyUp("joystick button 5");
                        if (input) return true;
                        else return false;
                    }
                default:
                    {
                        var input = Input.GetKeyDown("joystick button 5");
                        if (input) return true;
                        else return false;
                    }
            }
        }
        public static bool Button7(ButtonState state)
        {
            switch (state)
            {
                case ButtonState.Pressed:
                    {
                        var input = Input.GetKeyDown("joystick button 6");
                        if (input) return true;
                        else return false;
                    }
                case ButtonState.Held:
                    {
                        var input = Input.GetKey("joystick button 6");
                        if (input) return true;
                        else return false;
                    }
                case ButtonState.Released:
                    {
                        var input = Input.GetKeyUp("joystick button 6");
                        if (input) return true;
                        else return false;
                    }
                default:
                    {
                        var input = Input.GetKeyDown("joystick button 6");
                        if (input) return true;
                        else return false;
                    }
            }
        }
        public static bool Button8(ButtonState state)
        {
            switch (state)
            {
                case ButtonState.Pressed:
                    {
                        var input = Input.GetKeyDown("joystick button 7");
                        if (input) return true;
                        else return false;
                    }
                case ButtonState.Held:
                    {
                        var input = Input.GetKey("joystick button 7");
                        if (input) return true;
                        else return false;
                    }
                case ButtonState.Released:
                    {
                        var input = Input.GetKeyUp("joystick button 7");
                        if (input) return true;
                        else return false;
                    }
                default:
                    {
                        var input = Input.GetKeyDown("joystick button 7");
                        if (input) return true;
                        else return false;
                    }
            }
        }
        public static bool Button9(ButtonState state)
        {
            switch (state)
            {
                case ButtonState.Pressed:
                    {
                        var input = Input.GetKeyDown("joystick button 8");
                        if (input) return true;
                        else return false;
                    }
                case ButtonState.Held:
                    {
                        var input = Input.GetKey("joystick button 8");
                        if (input) return true;
                        else return false;
                    }
                case ButtonState.Released:
                    {
                        var input = Input.GetKeyUp("joystick button 8");
                        if (input) return true;
                        else return false;
                    }
                default:
                    {
                        var input = Input.GetKeyDown("joystick button 8");
                        if (input) return true;
                        else return false;
                    }
            }
        }
        public static bool Button10(ButtonState state)
        {
            switch (state)
            {
                case ButtonState.Pressed:
                    {
                        var input = Input.GetKeyDown("joystick button 9");
                        if (input) return true;
                        else return false;
                    }
                case ButtonState.Held:
                    {
                        var input = Input.GetKey("joystick button 9");
                        if (input) return true;
                        else return false;
                    }
                case ButtonState.Released:
                    {
                        var input = Input.GetKeyUp("joystick button 9");
                        if (input) return true;
                        else return false;
                    }
                default:
                    {
                        var input = Input.GetKeyDown("joystick button 9");
                        if (input) return true;
                        else return false;
                    }
            }
        }
        public static bool Button11(ButtonState state)
        {
            switch (state)
            {
                case ButtonState.Pressed:
                    {
                        var input = Input.GetKeyDown("joystick button 10");
                        if (input) return true;
                        else return false;
                    }
                case ButtonState.Held:
                    {
                        var input = Input.GetKey("joystick button 10");
                        if (input) return true;
                        else return false;
                    }
                case ButtonState.Released:
                    {
                        var input = Input.GetKeyUp("joystick button 10");
                        if (input) return true;
                        else return false;
                    }
                default:
                    {
                        var input = Input.GetKeyDown("joystick button 10");
                        if (input) return true;
                        else return false;
                    }
            }
        }
        public static bool Button12(ButtonState state)
        {
            switch (state)
            {
                case ButtonState.Pressed:
                    {
                        var input = Input.GetKeyDown("joystick button 11");
                        if (input) return true;
                        else return false;
                    }
                case ButtonState.Held:
                    {
                        var input = Input.GetKey("joystick button 11");
                        if (input) return true;
                        else return false;
                    }
                case ButtonState.Released:
                    {
                        var input = Input.GetKeyUp("joystick button 11");
                        if (input) return true;
                        else return false;
                    }
                default:
                    {
                        var input = Input.GetKeyDown("joystick button 11");
                        if (input) return true;
                        else return false;
                    }
            }
        }

    }
}

