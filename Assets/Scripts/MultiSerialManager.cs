using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;
using System.Diagnostics;
using System;

public class MultiSerialManager : MonoBehaviour
{
    public static MultiSerialManager Instance { get; private set; }
    public GameObject serialControllerPrefab;
    public GameObject[] listeners;
    public GameObject[] playerIcons;
    public List<GameObject> serialControllers = new List<GameObject>();
    public int baudRate = 9600;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

        }
        else
        {
            Destroy(gameObject);
        }

    }

    void Start()
    {
        SearchDevice();
    }

    void SearchDevice()
    {
        string[] ports = SerialPort.GetPortNames();
        int i = 0;
        int index = 999;

        foreach (string port in ports)
        {
            SerialPort sp = new SerialPort(port, baudRate);
            sp.ReadTimeout = 100;
            sp.NewLine = "\n";

            try
            {
                sp.Open();
                //UnityEngine.Debug.Log($"Trying port: {port}");

                string receivedData = null;
                Stopwatch stopwatch = Stopwatch.StartNew();
                while (stopwatch.ElapsedMilliseconds < 1000)
                {
                    try
                    {
                        receivedData = sp.ReadLine();
                        if (!string.IsNullOrEmpty(receivedData))
                        {
                            //UnityEngine.Debug.Log($"[{port}] Received: {receivedData}");
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
                    bool isArduino = dataParts.Length >= 4 &&
                                     float.TryParse(dataParts[0], out _) &&
                                     float.TryParse(dataParts[1], out _) &&
                                     float.TryParse(dataParts[2], out _) &&
                                     int.TryParse(dataParts[3], out index);

                    UnityEngine.Debug.Log($"[{port}] Format valid: {isArduino}");

                    if (isArduino)
                    {
                        if (i >= listeners.Length)
                        {
                            UnityEngine.Debug.LogWarning("Not enough listeners for connected devices.");
                            sp.Close();
                            break;
                        }

                        sp.Close();

                        GameObject serialController = Instantiate(serialControllerPrefab);
                        serialControllers.Add(serialController);
                        if (serialController.TryGetComponent<SerialController>(out var controller))
                        {
                            controller.portName = port;
                            controller.baudRate = baudRate;
                            controller.messageListener = listeners[i];
                            listeners[i].GetComponent<MyListener>().color = (PlayerColor)index;
                            GameObject iconObject = playerIcons[i];

                            if (iconObject != null)
                            {
                                var switcher = iconObject.GetComponent<DynamicPlayerNumber>();
                                if (switcher != null)
                                {
                                    switcher.SetColor((PlayerColor)index);
                                    UnityEngine.Debug.Log($"[{port}] Icon object found and color set for player {index}.");
                                }
                            }

                            i++;
                            serialController.SetActive(true);
                            controller.enabled = true;
                            UnityEngine.Debug.Log("Add player and device count");
                            GameManager.Instance.PlayerCount++;
                            GameManager.Instance.deviceCount++;
                            //UnityEngine.Debug.Log($"[{port}] Controller assigned to listener {i - 1}");
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

    public void ResearchDevice()
    {
        foreach (var controller in serialControllers)
        {
            Destroy(controller);
        }

        foreach (var listener in listeners)
        {
            listener.GetComponent<MyListener>().Reset();
        }

        serialControllers.Clear();
        GameManager.Instance.PlayerCount = 0;
        GameManager.Instance.deviceCount = 0;
        SearchDevice();
    }
}