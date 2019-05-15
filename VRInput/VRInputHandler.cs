using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
      LAST EDIT:      2/6/2019
      FOR:            Dr. Meredith Minear, University of Wyoming
*/
/*************************************************************************************************/

namespace Inputs
{
    public enum Interaction { GripPressed,
                              GripReleased,
                              MenuPressed,
                              MenuReleased,
                              PadPressed,
                              PadTouched,
                              PadReleased,
                              TriggerHeld,
                              TriggerTapped,
                              TriggerClicked,
                              TriggerReleased
                             }


    [RequireComponent(typeof(VRInput))]
    public class VRInputHandler : MonoBehaviour
    {
        private VRInput m_VRInput;
        private static Dictionary<Interaction, VRInputEventArgs> m_Inputs = new Dictionary<Interaction, VRInputEventArgs>()
        {
            {Interaction.GripPressed, null },
            {Interaction.GripReleased, null },
            {Interaction.MenuPressed, null },
            {Interaction.MenuReleased, null },
            {Interaction.PadPressed, null },
            {Interaction.PadTouched, null },
            {Interaction.PadReleased, null },
            {Interaction.TriggerHeld, null },
            {Interaction.TriggerTapped, null },
            {Interaction.TriggerClicked, null },
            {Interaction.TriggerReleased, null }
        };


        private void Awake(){ Subscribe(); }
        private void OnEnable(){ Subscribe(); }
        private void OnDisable(){ Unsubscribe(); }
        private void OnDestroy(){ Unsubscribe(); }


        private void Unsubscribe()
        {
            m_VRInput.GripPressed -= OnGripPressed;
            m_VRInput.GripReleased -= OnGripReleased;


            m_VRInput.MenuPressed -= OnMenuPressed;
            m_VRInput.MenuReleased -= OnMenuReleased;


            m_VRInput.PadPressed -= OnPadPressed;
            m_VRInput.PadTouched -= OnPadTouched;
            m_VRInput.PadReleased -= OnPadReleased;


            m_VRInput.TriggerTapped -= OnTriggerTapped;
            m_VRInput.TriggerHeld -= OnTriggerHeld;
            m_VRInput.TriggerClicked -= OnTriggerClicked;
            m_VRInput.TriggerReleased -= OnTriggerReleased;
        }
        private void Subscribe()
        {
            m_VRInput = GetComponent<VRInput>();
            if (m_VRInput == null)
            {
                Debug.LogError("Controller is missing VRInput Component");
                return;
            }

            m_VRInput.GripPressed += OnGripPressed;
            m_VRInput.GripReleased += OnGripReleased;


            m_VRInput.MenuPressed += OnMenuPressed;
            m_VRInput.MenuReleased += OnMenuReleased;


            m_VRInput.PadPressed += OnPadPressed;
            m_VRInput.PadTouched += OnPadTouched;
            m_VRInput.PadReleased += OnPadReleased;


            m_VRInput.TriggerTapped += OnTriggerTapped;
            m_VRInput.TriggerHeld += OnTriggerHeld;
            m_VRInput.TriggerClicked += OnTriggerClicked;
            m_VRInput.TriggerReleased += OnTriggerReleased;
        }


        //+-------------+
        //| Grip Events |
        //+-------------+
        private void OnGripPressed(object sender, VRInputEventArgs e)
        {
            Debug.Log(e.HandRole + " Grip Pressed");
            m_Inputs[Interaction.GripPressed] = e;
            m_Inputs[Interaction.GripReleased] = null;
        }
        private void OnGripReleased(object sender, VRInputEventArgs e)
        {
            Debug.Log(e.HandRole + " Grip Released");
            m_Inputs[Interaction.GripReleased] = e;
            m_Inputs[Interaction.GripPressed] = null;
        }
        //================================================================



        //+--------------------+
        //| Menu Button Events |
        //+--------------------+
        private void OnMenuPressed(object sender, VRInputEventArgs e)
        {
            Debug.Log(e.HandRole + " Menu Button Pressed");
            m_Inputs[Interaction.MenuPressed] = e;
            m_Inputs[Interaction.MenuReleased] = null;
        }
        private void OnMenuReleased(object sender, VRInputEventArgs e)
        {
            Debug.Log(e.HandRole + " Menu Button Released");
            m_Inputs[Interaction.MenuReleased] = e;
            m_Inputs[Interaction.MenuPressed] = null;
        }
        //================================================================



        //+------------+
        //| Pad Events |
        //+------------+
        private void OnPadPressed(object sender, VRInputEventArgs e)
        {
            Debug.Log(e.HandRole + " Pad Pressed");
            m_Inputs[Interaction.PadPressed] = e;
            m_Inputs[Interaction.PadTouched] = null;
            m_Inputs[Interaction.PadReleased] = null;
        }
        private void OnPadTouched(object sender, VRInputEventArgs e)
        {
            Debug.Log(e.HandRole + " Pad Touched");
            m_Inputs[Interaction.PadTouched] = e;
            m_Inputs[Interaction.PadPressed] = null;
            m_Inputs[Interaction.PadReleased] = null;
        }
        private void OnPadReleased(object sender, VRInputEventArgs e)
        {
            Debug.Log(e.HandRole + " Pad Released");
            m_Inputs[Interaction.PadReleased] = e;
            m_Inputs[Interaction.PadPressed] = null;
            m_Inputs[Interaction.PadTouched] = null;
        }
        //================================================================



        //+----------------+
        //| Trigger Events |
        //+----------------+
        private void OnTriggerHeld(object sender, VRInputEventArgs e)
        {
            Debug.Log(e.HandRole + " Trigger Pressed/Held");
            m_Inputs[Interaction.TriggerHeld] = e;
            m_Inputs[Interaction.TriggerClicked] = null;
            m_Inputs[Interaction.TriggerTapped] = null;
            m_Inputs[Interaction.TriggerReleased] = null;
        }
        private void OnTriggerTapped(object sender, VRInputEventArgs e)
        {
            Debug.Log(e.HandRole + " Trigger Tapped/Light Hold");
            m_Inputs[Interaction.TriggerTapped] = e;
            m_Inputs[Interaction.TriggerClicked] = null;
            m_Inputs[Interaction.TriggerHeld] = null;
            m_Inputs[Interaction.TriggerReleased] = null;
        }
        private void OnTriggerClicked(object sender, VRInputEventArgs e)
        {
            Debug.Log(e.HandRole + " Trigger Clicked/Full Press/Ful Hold");
            m_Inputs[Interaction.TriggerClicked] = e;
            m_Inputs[Interaction.TriggerHeld] = null;
            m_Inputs[Interaction.TriggerTapped] = null;
            m_Inputs[Interaction.TriggerReleased] = null;
        }
        private void OnTriggerReleased(object sender, VRInputEventArgs e)
        {
            Debug.Log(e.HandRole + " Trigger Released");
            m_Inputs[Interaction.TriggerReleased] = e;
            m_Inputs[Interaction.TriggerClicked] = null;
            m_Inputs[Interaction.TriggerTapped] = null;
            m_Inputs[Interaction.TriggerHeld] = null;
        }
        //================================================================


        public static VRInputEventArgs GripPressed() { return m_Inputs[Interaction.GripPressed]; }
        public static VRInputEventArgs GripReleased() { return m_Inputs[Interaction.GripReleased]; }
        public static VRInputEventArgs MenuPressed() { return m_Inputs[Interaction.MenuPressed]; }
        public static VRInputEventArgs MenuReleased() { return m_Inputs[Interaction.MenuReleased]; }
        public static VRInputEventArgs PadPressed() { return m_Inputs[Interaction.PadPressed]; }
        public static VRInputEventArgs PadTouched() { return m_Inputs[Interaction.PadTouched]; }
        public static VRInputEventArgs PadReleased() { return m_Inputs[Interaction.PadReleased]; }
        public static VRInputEventArgs TriggerHeld() { return m_Inputs[Interaction.TriggerHeld]; }
        public static VRInputEventArgs TriggerTapped() { return m_Inputs[Interaction.TriggerTapped]; }
        public static VRInputEventArgs TriggerClicked() { return m_Inputs[Interaction.TriggerClicked]; }
        public static VRInputEventArgs TriggerReleased() { return m_Inputs[Interaction.TriggerReleased]; }

    }
}

