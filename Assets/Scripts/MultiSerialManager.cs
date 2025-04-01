using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;

public class MultiSerialManager : MonoBehaviour
{
    public GameObject serialControllerPrefab;
    public GameObject[] listeners;
    public int baudRate = 9600;

    void Start()
    {
        string[] ports = SerialPort.GetPortNames();
        int i = 0;
        foreach (string port in ports)
        {
            SerialPort sp = new SerialPort(port, baudRate);
            sp.ReadTimeout = 1000;
            sp.Open();

            if (sp.IsOpen)
            {
                string data = sp.ReadLine();
                string[] dataParts = data.Split(',');
                //the data needs to match the arduino data format
                //currently it is set to 3 floats, but this may change in the future
                bool isArduino = dataParts.Length >= 3 && float.TryParse(dataParts[0], out float f) && float.TryParse(dataParts[1], out f) && float.TryParse(dataParts[2], out f);
                
                if (isArduino && i >= listeners.Length)
                {
                    Debug.LogWarning("Not enough listeners for the number of Arduino devices connected.");
                    break;
                }

                if (isArduino)
                {
                    sp.Close();
                    GameObject serialController = Instantiate(serialControllerPrefab);
                    if (serialController.TryGetComponent<SerialController>(out var controller))
                    {
                        controller.portName = port;
                        controller.messageListener = listeners[i];
                        controller.baudRate = baudRate;
                        i++;
                    }
                }
            }
        }
    }
}
