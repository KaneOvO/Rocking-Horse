using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSystem.Input
{
    public class InputLayer
    {
        public delegate void AccelerateEvent(float value);
        public delegate void RotateEvent(float value);

        private static event AccelerateEvent OnAccelerateUpdate;
        private static event RotateEvent OnRotateUpdate;

        public int RegisterConatroller()
        {
            return 0;
        }
        public void Updateccelerate(float value)
        {

        }
        public void UpdateRotation()
        {

        }
    }

}
