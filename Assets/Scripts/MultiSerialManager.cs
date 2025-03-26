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
        foreach (string port in ports)
        {
            SerialPort sp = new SerialPort(port, baudRate);
            sp.ReadTimeout = 1000;
            sp.Open();

            if (sp.IsOpen)
            {
                string data = sp.ReadLine();
                if (data == "0")
                {
                    sp.Close();
                    GameObject serialController = Instantiate(serialControllerPrefab);
                    if(serialController.TryGetComponent<SerialController>(out var controller))
                    {
                        controller.portName = port;
                        controller.messageListener = listeners[0];
                    }
                }
                else if(data == "1")
                {
                    sp.Close();
                    GameObject serialController = Instantiate(serialControllerPrefab);
                    if(serialController.TryGetComponent<SerialController>(out var controller))
                    {
                        controller.portName = port;
                        controller.messageListener = listeners[1];
                    }
                }
                else if(data == "2")
                {
                    sp.Close();
                    GameObject serialController = Instantiate(serialControllerPrefab);
                    if(serialController.TryGetComponent<SerialController>(out var controller))
                    {
                        controller.portName = port;
                        controller.messageListener = listeners[2];
                    }
                }
                else if(data == "3")
                {
                    sp.Close();
                    GameObject serialController = Instantiate(serialControllerPrefab);
                    if(serialController.TryGetComponent<SerialController>(out var controller))
                    {
                        controller.portName = port;
                        controller.messageListener = listeners[3];
                    }
                }
            }

        }
    }
}
