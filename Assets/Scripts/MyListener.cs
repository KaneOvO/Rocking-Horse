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
        
        if(msgSplit.Length < 3) return;
        sensorData.gyroscopeX = float.Parse(msgSplit[0]);
        sensorData.gyroscopeY = float.Parse(msgSplit[1]);
        sensorData.gyroscopeZ = float.Parse(msgSplit[2]);

        if(msgSplit.Length < 6) return;
        sensorData.rotationX = float.Parse(msgSplit[3]);
        sensorData.rotationY = float.Parse(msgSplit[4]);
        sensorData.rotationZ = float.Parse(msgSplit[5]);

        if (msgSplit.Length > 6)
        sensorData.isBoosted = int.Parse(msgSplit[6]);

        if (msgSplit.Length > 7)
            sensorData.isJumped = int.Parse(msgSplit[7]);

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

