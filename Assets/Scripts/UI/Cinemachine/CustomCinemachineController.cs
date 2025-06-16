using System;
using UI;
using Unity.Cinemachine;
using UnityEngine;
namespace Game.Utility
{
    public class CustomCinemachineController : InputAxisControllerBase<CustomCinemachineController.Reader>
    {
        void Update()
        {
            if (Application.isPlaying)
                UpdateControllers();
        }

        [Serializable]
        public class Reader : IInputAxisReader
        {
            float X;
            float Y;
            public float GetValue(UnityEngine.Object context, IInputAxisOwner.AxisDescriptor.Hints hint)
            {
                X = Mathf.Lerp(X, Freelook.s_Instance.X * 2, Time.deltaTime * 10);
                Y = Mathf.Lerp(Y, Freelook.s_Instance.Y * 2, Time.deltaTime * 10);
                return (hint == IInputAxisOwner.AxisDescriptor.Hints.Y ? -Y : X);
            }
        }
    }
}