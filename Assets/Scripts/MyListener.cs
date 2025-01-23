using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MyListener : MonoBehaviour
{
    public static MyListener instance;

    public SensorData sensorData;
    public static System.Action<SensorData> OnSensorDataUpdated;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnMessageArrived(string msg)
    {
        //Debug.Log("Received Msg: " + msg);
        string[] msgSplit = msg.Split(',');
        sensorData.gyroscopeX = float.Parse(msgSplit[0]);
        sensorData.gyroscopeY = float.Parse(msgSplit[1]);
        sensorData.gyroscopeZ = float.Parse(msgSplit[2]);
        sensorData.isBoosted = int.Parse(msgSplit[3]);
        OnSensorDataUpdated?.Invoke(sensorData);
    }
    
    void OnConnectionEvent(bool success)
    {
        Debug.Log(success ? "Device connected" : "Device disconnected");
    }
}

public struct SensorData
{
    public float gyroscopeX;
    public float gyroscopeY;
    public float gyroscopeZ;
    public int isBoosted;
}