using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyListener : MonoBehaviour
{
    public SensorData sensorData;
    public bool isConnected;
    public bool isSentConnected = false;
    public System.Action<SensorData> OnEquipmentConnected;
    public System.Action<SensorData> OnSensorDataUpdated;

    void OnMessageArrived(string msg)
    {
        //Debug.Log("Received Msg: " + msg);
        string[] msgSplit = msg.Split(',');

        if (msgSplit.Length < 1) return;

        sensorData.gyroscopeX = float.Parse(msgSplit[0]);

        if (msgSplit.Length < 2) return;
        sensorData.rotationZ = float.Parse(msgSplit[1]);

        if (msgSplit.Length > 2)
            sensorData.isBoosted = int.Parse(msgSplit[2]);

        OnSensorDataUpdated?.Invoke(sensorData);
    }

    void OnConnectionEvent(bool success)
    {
        if (isConnected == success) return;

        isConnected = success;

        if (success && !isSentConnected)
        {
            OnEquipmentConnected?.Invoke(sensorData);
            isSentConnected = true;
        }
        else if (!success)
        {
            isSentConnected = false;
        }

        Debug.Log(success ? "Device connected" : "Device disconnected");
    }
}

