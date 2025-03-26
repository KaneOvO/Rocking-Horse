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
                bool isArduino = dataParts.Length >= 6 && float.TryParse(dataParts[0], out float f) && float.TryParse(dataParts[1], out f) && float.TryParse(dataParts[2], out f) && float.TryParse(dataParts[3], out f) && float.TryParse(dataParts[4], out f) && float.TryParse(dataParts[5], out f);
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
