using System;
using UnityEngine;
using HTC.UnityPlugin.Vive;

/*************************************************************************************************/
/* 

WARNING:  
  This particular utility requires that the "Vive Input Plugin" be installed.
  You can find it on the Unity Store for free


WARNING:
  This utility is set up for the HTC Vive but, the "Vive Input Plugin" also works with Occulus.
  If you need Occulus support, you will need to add it


INFORMATION:

      WRITTEN BY:     Adrian T. Barberis -- 14abarberis@gmail.com
      CREATED:        2/4/2019
      LAST EDIT:      2/4/2019
      FOR:            Dr. Meredith Minear, University of Wyoming
*/
/*************************************************************************************************/


namespace Inputs
{
    public enum ButtonStatus { Pressed, Released }


    [RequireComponent(typeof(Valve.VR.InteractionSystem.Hand))]
    public class VRInput : MonoBehaviour
    {
        public event EventHandler<VRInputEventArgs> GripPressed;
        public event EventHandler<VRInputEventArgs> TriggerTapped;
        public event EventHandler<VRInputEventArgs> TriggerHeld;
        public event EventHandler<VRInputEventArgs> TriggerClicked;
        public event EventHandler<VRInputEventArgs> PadPressed;
        public event EventHandler<VRInputEventArgs> PadTouched;
        public event EventHandler<VRInputEventArgs> MenuPressed;
        public event EventHandler<VRInputEventArgs> GripReleased;
        public event EventHandler<VRInputEventArgs> MenuReleased;
        public event EventHandler<VRInputEventArgs> PadReleased;
        public event EventHandler<VRInputEventArgs> TriggerReleased;

        private void Update()
        {
            // Grip Press Checks
            CheckGripPressed(HandRole.RightHand);
            CheckGripPressed(HandRole.LeftHand);
            //------------------------------------


            // Menu Button Press Checks
            CheckMenuPressed(HandRole.RightHand);
            CheckMenuPressed(HandRole.LeftHand);
            //------------------------------------


            // Pad Press Checks
            CheckPadPressed(HandRole.RightHand);
            CheckPadPressed(HandRole.LeftHand);

            CheckPadTouched(HandRole.RightHand);
            CheckPadTouched(HandRole.LeftHand);
            //------------------------------------


            // Trigger Press Checks
            CheckTriggerHeld(HandRole.RightHand);
            CheckTriggerHeld(HandRole.LeftHand);

            CheckTriggerTapped(HandRole.RightHand);
            CheckTriggerTapped(HandRole.LeftHand);

            CheckTriggerClicked(HandRole.RightHand);
            CheckTriggerClicked(HandRole.LeftHand);
            //------------------------------------


            // Grip Release Checks
            CheckGripReleased(HandRole.RightHand);
            CheckGripReleased(HandRole.LeftHand);
            //------------------------------------


            // Menu Button Release Checks
            CheckMenuReleased(HandRole.RightHand);
            CheckMenuReleased(HandRole.LeftHand);
            //------------------------------------


            // Pad Release Checks
            CheckPadReleased(HandRole.RightHand);
            CheckPadReleased(HandRole.LeftHand);
            //------------------------------------


            // Trigger Release Checks
            CheckTriggerReleased(HandRole.RightHand);
            CheckTriggerReleased(HandRole.LeftHand);
            //------------------------------------

        }



        #region /* Check For Button Presses */
        //+------------------------+
        //| Check For Grip Pressed |
        //+------------------------+
        private void CheckGripPressed(HandRole hr)
        {
            if(ViveInput.GetPress(hr, ControllerButton.Grip))
            {
                VRInputEventArgs args = new VRInputEventArgs();
                args.ButtonPressed = ControllerButton.Grip;
                args.HandRole = hr;
                args.Timestamp = DateTime.Now;
                OnGripPressed(args);
            }
        }
        //================================================================



        //+------------------------+
        //| Check For Menu Pressed |
        //+------------------------+
        private void CheckMenuPressed(HandRole hr)
        {
            if (ViveInput.GetPressDown(hr, ControllerButton.Menu))
            {
                VRInputEventArgs args = new VRInputEventArgs();
                args.ButtonPressed = ControllerButton.Menu;
                args.HandRole = hr;
                args.Timestamp = DateTime.Now;
                OnMenuPressed(args);
            }
        }
        //================================================================



        //+-----------------------+
        //| Check For Pad Pressed |
        //+-----------------------+
        private void CheckPadPressed(HandRole hr)
        {
            if (ViveInput.GetPressDown(hr, ControllerButton.Pad))
            {
                VRInputEventArgs args = new VRInputEventArgs();
                args.ButtonPressed = ControllerButton.Pad;
                args.HandRole = hr;
                args.PressCoordinates = ViveInput.GetPadAxis(hr);
                args.Timestamp = DateTime.Now;
                OnPadPressed(args);
            }
        }
        //================================================================



        //+-----------------------+
        //| Check For Pad Touched |
        //+-----------------------+
        private void CheckPadTouched(HandRole hr)
        {
            if (ViveInput.GetPressDown(hr, ControllerButton.PadTouch))
            {
                VRInputEventArgs args = new VRInputEventArgs();
                args.ButtonPressed = ControllerButton.PadTouch;
                args.HandRole = hr;
                args.TouchCoordinates = ViveInput.GetPadAxis(hr);
                args.Timestamp = DateTime.Now;
                OnPadTouched(args);
            }
        }
        //================================================================



        //+----------------------------------+
        //| Check For Trigger Pressed / Held |
        //+----------------------------------+
        private void CheckTriggerHeld(HandRole hr)
        {
            if (ViveInput.GetPressDown(hr, ControllerButton.Trigger))
            {
                VRInputEventArgs args = new VRInputEventArgs();
                args.ButtonPressed = ControllerButton.Trigger;
                args.HandRole = hr;
                args.Timestamp = DateTime.Now;
                OnTriggerHeld(args);
            }
        }
        //================================================================



        //+-----------------------------------------------------+
        //| Check For Trigger Tapped / Light Press / Light Hold |
        //+-----------------------------------------------------+
        private void CheckTriggerTapped(HandRole hr)
        {
            if (ViveInput.GetPressDown(hr, ControllerButton.TriggerTouch))
            {
                VRInputEventArgs args = new VRInputEventArgs();
                args.ButtonPressed = ControllerButton.TriggerTouch;
                args.HandRole = hr;
                args.Timestamp = DateTime.Now;
                OnTriggerTapped(args);
            }
        }
        //================================================================



        //+----------------------------------------------------+
        //| Check For Trigger Clicked / Full Press / Full Hold |
        //+----------------------------------------------------+
        private void CheckTriggerClicked(HandRole hr)
        {
            if (ViveInput.GetPressDown(hr, ControllerButton.FullTrigger))
            {
                VRInputEventArgs args = new VRInputEventArgs();
                args.ButtonPressed = ControllerButton.FullTrigger;
                args.HandRole = hr;
                args.Timestamp = DateTime.Now;
                OnTriggerClicked(args);
            }
        }
        //================================================================
        #endregion


        #region /* Check For Button Releases */
        //+------------------------+
        //| Check For Grip Release |
        //+------------------------+
        private void CheckGripReleased(HandRole hr)
        {
            if (ViveInput.GetPressUp(hr, ControllerButton.Grip))
            {
                VRInputEventArgs args = new VRInputEventArgs();
                args.ButtonPressed = ControllerButton.Grip;
                args.HandRole = hr;
                args.Timestamp = DateTime.Now;
                OnGripReleased(args);
            }
        }
        //================================================================



        //+------------------------+
        //| Check For Menu Release |
        //+------------------------+
        private void CheckMenuReleased(HandRole hr)
        {
            if (ViveInput.GetPressUp(hr, ControllerButton.Menu))
            {
                VRInputEventArgs args = new VRInputEventArgs();
                args.ButtonPressed = ControllerButton.Menu;
                args.HandRole = hr;
                args.Timestamp = DateTime.Now;
                OnMenuReleased(args);
            }
        }
        //================================================================



        //+-----------------------+
        //| Check For Pad Release |
        //+-----------------------+
        private void CheckPadReleased(HandRole hr)
        {
            if (ViveInput.GetPressUp(hr, ControllerButton.Pad))
            {
                VRInputEventArgs args = new VRInputEventArgs();
                args.ButtonPressed = ControllerButton.Pad;
                args.HandRole = hr;
                args.Timestamp = DateTime.Now;
                OnPadReleased(args);
            }
            else if(ViveInput.GetPressUp(hr, ControllerButton.PadTouch))
            {
                VRInputEventArgs args = new VRInputEventArgs();
                args.ButtonPressed = ControllerButton.PadTouch;
                args.HandRole = hr;
                args.Timestamp = DateTime.Now;
                OnPadReleased(args);
            }
        }
        //================================================================



        //+---------------------------+
        //| Check For Trigger Release |
        //+---------------------------+
        private void CheckTriggerReleased(HandRole hr)
        {
            if (ViveInput.GetPressUp(hr, ControllerButton.Trigger))
            {
                VRInputEventArgs args = new VRInputEventArgs();
                args.ButtonPressed = ControllerButton.Trigger;
                args.HandRole = hr;
                args.Timestamp = DateTime.Now;
                OnTriggerReleased(args);
            }
            else if(ViveInput.GetPressUp(hr, ControllerButton.TriggerTouch))
            {
                VRInputEventArgs args = new VRInputEventArgs();
                args.ButtonPressed = ControllerButton.TriggerTouch;
                args.HandRole = hr;
                args.Timestamp = DateTime.Now;
                OnTriggerReleased(args);
            }
            else if (ViveInput.GetPressUp(hr, ControllerButton.FullTrigger))
            {
                VRInputEventArgs args = new VRInputEventArgs();
                args.ButtonPressed = ControllerButton.FullTrigger;
                args.HandRole = hr;
                args.Timestamp = DateTime.Now;
                OnTriggerReleased(args);
            }
        }
        //================================================================
        #endregion


        #region /* "OnPressed" Signals */

        //+---------------------+
        //| Signal Grip Pressed |
        //+---------------------+
        protected virtual void OnGripPressed(VRInputEventArgs e)
        {
            EventHandler<VRInputEventArgs> handler = GripPressed;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        //================================================================



        //+---------------------+
        //| Signal Menu Pressed |
        //+---------------------+
        protected virtual void OnMenuPressed(VRInputEventArgs e)
        {
            EventHandler<VRInputEventArgs> handler = MenuPressed;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        //================================================================



        //+--------------------+
        //| Signal Pad Pressed |
        //+--------------------+
        protected virtual void OnPadPressed(VRInputEventArgs e)
        {
            EventHandler<VRInputEventArgs> handler = PadPressed;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        //================================================================



        //+--------------------+
        //| Signal Pad Touched |
        //+--------------------+
        protected virtual void OnPadTouched(VRInputEventArgs e)
        {
            EventHandler<VRInputEventArgs> handler = PadTouched;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        //================================================================



        //+-------------------------------+
        //| Signal Trigger Held / Pressed |
        //+-------------------------------+
        protected virtual void OnTriggerHeld(VRInputEventArgs e)
        {
            /* Activates If:
             *      1) Trigger if Held at least to 55% of the way
             *      2) Trigger is Pressed at least to 55% of the way
             */

            EventHandler <VRInputEventArgs> handler = TriggerHeld;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        //================================================================



        //+--------------------------------------------------+
        //| Signal Trigger Tapped / Light Press / Light Hold |
        //+--------------------------------------------------+
        protected virtual void OnTriggerTapped(VRInputEventArgs e)
        {
            /* Activates If:
             *      1) Trigger if Held at least to 25% of the way
             *      2) Trigger is Pressed at least to 25% of the way
             */
            EventHandler<VRInputEventArgs> handler = TriggerTapped;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        //================================================================



        //+-------------------------------------------------+
        //| Signal Trigger Clicked / Full Press / Full Hold |
        //+-------------------------------------------------+
        protected virtual void OnTriggerClicked(VRInputEventArgs e)
        {
            /* Activates If:
             *      1) Trigger if Held at least to 100% of the way
             *      2) Trigger is Pressed at least to 100% of the way
             */
            EventHandler<VRInputEventArgs> handler = TriggerClicked;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        //================================================================
        #endregion


        #region /* "On Release" Signals */
        //+----------------------+
        //| Signal Grip Released |
        //+----------------------+
        protected virtual void OnGripReleased(VRInputEventArgs e)
        {
            EventHandler<VRInputEventArgs> handler = GripReleased;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        //================================================================



        //+----------------------+
        //| Signal Menu Released |
        //+----------------------+
        protected virtual void OnMenuReleased(VRInputEventArgs e)
        {
            EventHandler<VRInputEventArgs> handler = MenuReleased;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        //================================================================



        //+---------------------+
        //| Signal Pad Released |
        //+---------------------+
        protected virtual void OnPadReleased(VRInputEventArgs e)
        {
            EventHandler<VRInputEventArgs> handler = PadReleased;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        //================================================================



        //+---------------------+
        //| Signal Pad Released |
        //+---------------------+
        protected virtual void OnTriggerReleased(VRInputEventArgs e)
        {
            EventHandler<VRInputEventArgs> handler = TriggerReleased;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        //================================================================
        #endregion


    }

    public class VRInputEventArgs : EventArgs
    {
        public ControllerButton ButtonPressed { get; set; }
        public HandRole HandRole { get; set; }
        public ButtonStatus Status {get; set;}
        public Vector2 TouchCoordinates { get; set; }
        public Vector2 PressCoordinates { get; set; }
        public DateTime Timestamp { get; set; }
    }
}

