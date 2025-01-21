using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSystem.Input
{
    public class InputLayer
    {
        private static int ControllerID = 0;

        public delegate void AccelerateEvent(float value);
        public delegate void RotateEvent(float value);
        public delegate void JumpEvent(float value);
        public delegate void ChangeLaneEvent(Vector2 direction);
        public delegate void BoosterEvent();

        private static Dictionary<int, AccelerateEvent> AccelerateEventDict = new Dictionary<int, AccelerateEvent>();
        private static Dictionary<int, RotateEvent> RotateEventDict = new Dictionary<int, RotateEvent>();
        private static Dictionary<int, JumpEvent> JumpEventDict = new Dictionary<int, JumpEvent>();
        private static Dictionary<int, ChangeLaneEvent> ChangeLaneEventDict = new Dictionary<int, ChangeLaneEvent>();
        private static Dictionary<int, BoosterEvent> BoosterEventDict = new Dictionary<int, BoosterEvent>();

        public static int RegisterConatroller(bool debug = false)
        {
            ControllerID++;

            AccelerateEventDict.Add(ControllerID, new AccelerateEvent((float value) => { if (debug && value != 0) Debug.Log($"{ControllerID} - Accelerate:{value}"); }));
            RotateEventDict.Add(ControllerID, new RotateEvent((float value) => { if (debug && value != 0) Debug.Log($"{ControllerID} - Rotate:{value}"); }));
            JumpEventDict.Add(ControllerID, new JumpEvent((float value) => { if (debug && value != 0) Debug.Log($"{ControllerID} - jump:{value}"); }));
            ChangeLaneEventDict.Add(ControllerID, new ChangeLaneEvent((Vector2 direction) => { if (debug && direction.magnitude != 0) Debug.Log($"{ControllerID} - change lane:{direction}"); }));
            BoosterEventDict.Add(ControllerID, new BoosterEvent(() => { if (debug) Debug.Log($"{ControllerID} - booster"); }));

            return ControllerID;
        }
        public static void RemoveController(int cid)
        {
            if (AccelerateEventDict.ContainsKey(cid)) AccelerateEventDict.Remove(cid);
            if (RotateEventDict.ContainsKey(cid)) RotateEventDict.Remove(cid);
            if (JumpEventDict.ContainsKey(cid)) JumpEventDict.Remove(cid);
            if (ChangeLaneEventDict.ContainsKey(cid)) ChangeLaneEventDict.Remove(cid);
            if (BoosterEventDict.ContainsKey(cid)) BoosterEventDict.Remove(cid);
        }
        public static void UpdateAccelerate(int cid, float value)
        {
            if (AccelerateEventDict.ContainsKey(cid)) AccelerateEventDict[cid].Invoke(value);
        }
        public static void UpdateRotation(int cid, float value)
        {
            if (RotateEventDict.ContainsKey(cid)) RotateEventDict[cid].Invoke(value);
        }
        public static void Jump(int cid, float value)
        {
            if (JumpEventDict.ContainsKey(cid)) JumpEventDict[cid].Invoke(value);
        }
        public static void ChangeLane(int cid, Vector2 direction)
        {
            if (ChangeLaneEventDict.ContainsKey(cid)) ChangeLaneEventDict[cid].Invoke(direction);
        }
        public static void UseBooster(int cid)
        {
            if (BoosterEventDict.ContainsKey(cid)) BoosterEventDict[cid].Invoke();
        }
        public static void AddAccelerateEventListener(int cid, AccelerateEvent evt)
        {
            if (AccelerateEventDict.ContainsKey(cid)) AccelerateEventDict[cid] += evt;
        }
        public static void AddRotateEventListener(int cid, RotateEvent evt)
        {
            if (RotateEventDict.ContainsKey(cid)) RotateEventDict[cid] += evt;
        }
        public static void AddJumpEventListener(int cid, JumpEvent evt)
        {
            if (JumpEventDict.ContainsKey(cid)) JumpEventDict[cid] += evt;
        }
        public static void AddChangeLaneEventListener(int cid, ChangeLaneEvent evt)
        {
            if (ChangeLaneEventDict.ContainsKey(cid)) ChangeLaneEventDict[cid] += evt;
        }
        public static void AddBoosterEventListener(int cid, BoosterEvent evt)
        {
            if (BoosterEventDict.ContainsKey(cid)) BoosterEventDict[cid] += evt;
        }
        public static void RemoveAccelerateEventListener(int cid, AccelerateEvent evt)
        {
            if (AccelerateEventDict.ContainsKey(cid)) AccelerateEventDict[cid] -= evt;
        }
        public static void RemoveRotateEventListener(int cid, RotateEvent evt)
        {
            if (RotateEventDict.ContainsKey(cid)) RotateEventDict[cid] -= evt;
        }
        public static void RemoveJumpEventListener(int cid, JumpEvent evt)
        {
            if (JumpEventDict.ContainsKey(cid)) JumpEventDict[cid] -= evt;
        }
        public static void RemoveChangeLaneEventListener(int cid, ChangeLaneEvent evt)
        {
            if (ChangeLaneEventDict.ContainsKey(cid)) ChangeLaneEventDict[cid] -= evt;
        }
        public static void RemoveBoosterEventListener(int cid, BoosterEvent evt)
        {
            if (BoosterEventDict.ContainsKey(cid)) BoosterEventDict[cid] -= evt;
        }
    }

}
