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
        sensorData.gyroscopeX = float.Parse(msgSplit[0]);
        sensorData.gyroscopeY = float.Parse(msgSplit[1]);
        sensorData.gyroscopeZ = float.Parse(msgSplit[2]);
        sensorData.isBoosted = int.Parse(msgSplit[3]);
        if (msgSplit.Length > 4)
            sensorData.isJumped = int.Parse(msgSplit[4]);
        if (msgSplit.Length > 5)
            sensorData.isChangedLeft = int.Parse(msgSplit[5]);
        if (msgSplit.Length > 6)
            sensorData.isChangedRight = int.Parse(msgSplit[6]);

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

