using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;
using System.Diagnostics;
using System;

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
            sp.ReadTimeout = 100;
            sp.NewLine = "\n";

            try
            {
                sp.Open();
                UnityEngine.Debug.Log($"Trying port: {port}");

                string receivedData = null;
                Stopwatch stopwatch = Stopwatch.StartNew();
                while (stopwatch.ElapsedMilliseconds < 1000)
                {
                    try
                    {
                        receivedData = sp.ReadLine();
                        if (!string.IsNullOrEmpty(receivedData))
                        {
                            UnityEngine.Debug.Log($"[{port}] Received: {receivedData}");
                            break;
                        }
                    }
                    catch (TimeoutException)
                    {
                    }
                }

                if (receivedData != null)
                {
                    string[] dataParts = receivedData.Split(',');
                    bool isArduino = dataParts.Length >= 3 &&
                                     float.TryParse(dataParts[0], out _) &&
                                     float.TryParse(dataParts[1], out _) &&
                                     float.TryParse(dataParts[2], out _);

                    UnityEngine.Debug.Log($"[{port}] Format valid: {isArduino}");

                    if (isArduino)
                    {
                        if (i > listeners.Length)
                        {
                            UnityEngine.Debug.LogWarning("Not enough listeners for connected devices.");
                            sp.Close();
                            break;
                        }

                        sp.Close();

                        GameObject serialController = Instantiate(serialControllerPrefab);
                        if (serialController.TryGetComponent<SerialController>(out var controller))
                        {
                            controller.portName = port;
                            controller.baudRate = baudRate;
                            controller.messageListener = listeners[i];
                            i++;
                            serialController.SetActive(true);
                            controller.enabled = true;
                            UnityEngine.Debug.Log($"[{port}] Controller assigned to listener {i - 1}");
                        }
                    }
                }

                
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogWarning($"Failed to read from port {port}: {ex.Message}");
                if (sp.IsOpen) sp.Close();
            }
        }
    }
}