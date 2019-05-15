/************************************************************************************

Filename    :   CVirtDeviceNative.cs
Content     :   Implentation of the native plugin device
Created     :   August 8, 2014
Authors     :   Lukas Pfeifhofer

Copyright   :   Copyright 2014 Cyberith GmbH

Licensed under the ___LICENSE___

Edited by   :   Adrian Barberis 7/6/2018

************************************************************************************/

using UnityEngine;
using System;

namespace CybSDK
{

    public class CVirtDeviceNative : CVirtDevice
    {

        private IntPtr devicePtr;

        public CVirtDeviceNative(IntPtr devicePtr)
        {
            this.devicePtr = devicePtr;
        }

        public override bool Open()
        {
            return CVirt.CybSDK_VirtDevice_Open(this.devicePtr);
        }

        public override bool IsOpen()
        {
            return CVirt.CybSDK_VirtDevice_IsOpen(this.devicePtr);
        }

        public override bool Close()
        {
            return CVirt.CybSDK_VirtDevice_Close(this.devicePtr);
        }

        public override float GetPlayerHeight()
        {
            return CVirt.CybSDK_VirtDevice_GetPlayerHeight(this.devicePtr);
        }

        public override void ResetPlayerHeight()
        {
            CVirt.CybSDK_VirtDevice_ResetPlayerHeight(this.devicePtr);
        }

        public override Vector3 GetPlayerOrientation()
        {
            float playerOrient =  CVirt.CybSDK_VirtDevice_GetPlayerOrientation(this.devicePtr);
            return new Vector3(
                Mathf.Cos(playerOrient * 2.0f * Mathf.PI - Mathf.PI / 2.0f),
                0.0f,
                -Mathf.Sin(playerOrient * 2.0f * Mathf.PI - Mathf.PI / 2.0f)
            ).normalized;
        }

        public override float GetMovementSpeed()
        {
            return CVirt.CybSDK_VirtDevice_GetMovementSpeed(this.devicePtr);
        }

        public override Vector3 GetMovementDirection()
        {
            float movDir =  CVirt.CybSDK_VirtDevice_GetMovementDirection(this.devicePtr);
            return new Vector3(
                Mathf.Cos(movDir * Mathf.PI - Mathf.PI / 2.0f),
                0.0f,
                -Mathf.Sin(movDir * Mathf.PI - Mathf.PI / 2.0f)
            ).normalized;
        }

        ///////////////////////////////////////////////////////////
        // Following methods added by Adrian Barberis on 7/5/2018
        //
        /// <summary>
        /// <para>Get Raw orientation data</para>
        /// <para>Moving the rotational ring of the Virtualizer Rig to the right will cause the vaue to increase.</para>
        /// <para>Moving to the left will cause the value to decrease.</para>
        /// <para>Moving to the left when the current value is zero will return a number around 0.99 and continued movement will decrease the value</para>
        /// </summary>
        /// <returns>float ranging from 0 to 0.99</returns>
        public override float GetOrientationRaw()
        {
            return CVirt.CybSDK_VirtDevice_GetPlayerOrientation(this.devicePtr);
        }

        /// <summary>
        /// <para>Get raw movement direction data</para>
        /// <para>Return value of 0 = Moving forwards</para>
        /// <para>Return value of 1 = Moving backwards</para>
        /// </summary>
        /// <returns>Float either 0 or 1</returns>
        public override float GetDirectionRaw()
        {
            // Get raw direction data: Float value either 0 or 1
            return CVirt.CybSDK_VirtDevice_GetMovementDirection(this.devicePtr);
        }
        /////////////////////////////////////////////////////////////////////////


        public override void ResetPlayerOrientation()
        {
            CVirt.CybSDK_VirtDevice_ResetPlayerOrientation(this.devicePtr);
        }

        public override bool HasHaptic()
        {
            return CVirt.CybSDK_VirtDevice_HasHaptic(this.devicePtr);
        }

        public override void SetHapticBaseplate(float value)
        {
            CVirt.CybSDK_VirtDevice_SetHapticBaseplate(this.devicePtr, value);
        }

        public bool IsPtrNull()
        {
            return (devicePtr.ToInt32() == 0);
        }

    }

}