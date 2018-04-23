using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Rokoko.Smartsuit
{
    /// <summary>
    /// Example that demonstrate how to use the smartsuit commands interface.
    /// </summary>
    public class SmartsuitControls : MonoBehaviour
    {

        /// <summary>
        /// The name of the body model.
        /// </summary>
        [Header("Body model configuration")]
        public string bodyModelName = "Default";

        /// <summary>
        /// The color of the body model.
        /// </summary>
        public Color bodyModelColor = new Color32(0xBC, 0xBC, 0xBC, 0xFF);

        /// <summary>
        /// The user dimensions to be assigned to the suit.
        /// </summary>
        public BodyModel bodyModel;

        /// <summary>
        /// Reference to an active smartsuit.
        /// </summary>
        [Header("Smartsuit reference")]
        public Smartsuit smartsuit;

        /// <summary>
        /// This is the target position and rotation that the suit will be positioned
        /// when clicking in "Set position" button.
        /// </summary>
        [Header("Position target")]
        public Transform targetTransform;

        /// <summary>
        /// Reset body model and smartsuit to its default values.
        /// </summary>
        private void Reset()
        {
            bodyModel = BodyModel.Default();
            bodyModel.TotalHeight = Mathf.Abs(bodyModel.TotalHeight);
            bodyModel.HipHeight = Mathf.Abs(bodyModel.HipHeight);
            bodyModel.ShoulderHeight = Mathf.Abs(bodyModel.ShoulderHeight);
            bodyModel.HipWidth = Mathf.Abs(bodyModel.HipWidth);
            bodyModel.ShoulderWidth = Mathf.Abs(bodyModel.ShoulderWidth);
            bodyModel.ArmSpan = Mathf.Abs(bodyModel.ArmSpan);
            bodyModel.AnkleHeight = Mathf.Abs(bodyModel.AnkleHeight);
            bodyModel.FootLength = Mathf.Abs(bodyModel.FootLength);
            bodyModel.HeelOffset = Mathf.Abs(bodyModel.HeelOffset);
            smartsuit = FindObjectOfType<Smartsuit>();
            bodyModelName = "Default";
            bodyModelColor = new Color32(0xBC, 0xBC, 0xBC, 0xFF);
        }

        /// <summary>
        /// Sends a restart command to the suit.
        /// </summary>
        public void Restart()
        {
            smartsuit.Restart();
        }

        /// <summary>
        /// Sends a calibrate command to the suit.
        /// </summary>
        public void Calibrate()
        {
            smartsuit.Calibrate();
        }

        /// <summary>
        /// Ask the smartsuit to broadcast it's messages.
        /// This allows multiple devices to listen to the packets of this suit.
        /// </summary>
        public void Broadcast()
        {
            smartsuit.Broadcast();
        }

        /// <summary>
        /// ASk the smartsuit to unicast messages to this device.
        /// </summary>
        public void Unicast()
        {
            smartsuit.Unicast();
        }

        /// <summary>
        /// Apply a random body model to the suit.
        /// </summary>
        public void SetBodyModel()
        {

            bodyModel.name = bodyModelName;
            bodyModel._Color = bodyModelColor;

            smartsuit.ApplyBodyDimensions(bodyModel.body, (bodyBack) =>
            {
                //Debug.Log("Body dimensions applied successfully");
            }, () => {
                Debug.LogWarning("Smartsuit didn't respond. Smartsuit failed to receive command, or Unity failed to receive response.");
            });
        }

        /// <summary>
        /// Apply a specific position and rotation  to the suit.
        /// </summary>
        public void ResetPositionRotation()
        {
            if (targetTransform)
            {
                smartsuit.SetSmartsuitTransform(smartsuit.transform.InverseTransformPoint(targetTransform.position), Quaternion.Inverse(targetTransform.rotation) * smartsuit.transform.rotation);
            } else
            {
                Debug.LogWarning("Please define targetTransform when using ResetPositionRotation");
            }
        }
    }
}
