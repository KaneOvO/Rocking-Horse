using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MyListener : MonoBehaviour
{
    public static MyListener instance;

    public float gyroscopeX, gyroscopeY, gyroscopeZ;

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
        Debug.Log("moving at speed: " + msg);
        string[] msgSplit = msg.Split(',');
        gyroscopeX = float.Parse(msgSplit[0]);
        gyroscopeY = float.Parse(msgSplit[1]);
        gyroscopeZ = float.Parse(msgSplit[2]);
    }
    
    void OnConnectionEvent(bool success)
    {
        Debug.Log(success ? "Device connected" : "Device disconnected");
    }
}