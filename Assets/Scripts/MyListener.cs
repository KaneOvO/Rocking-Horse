using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class MyListener : MonoBehaviour
{
    private SerialPort serialPort;
    private GameObject cube;

    void Start()
    {
        // Open the serial port (adjust port name and baud rate as per Arduino settings)
        serialPort = new SerialPort("COM3", 9600);
        serialPort.Open();

        // Find the cube object
        cube = GameObject.Find("Cube");
    }

    void Update()
    {
        if (serialPort.IsOpen)
        {
            try
            {
                string data = serialPort.ReadLine(); // Read a line of data
                string[] values = data.Split(','); // Split data into yaw, pitch, roll

                if (values.Length == 3)
                {
                    float yaw = float.Parse(values[0]);
                    float pitch = float.Parse(values[1]);
                    float roll = float.Parse(values[2]);

                    // Example: Use pitch to move the cube
                    float speed = pitch * 10; // Scale as needed
                    cube.transform.Translate(Vector3.up * Time.deltaTime * speed);
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogWarning($"Error reading serial data: {ex.Message}");
            }
        }
    }

    private void OnApplicationQuit()
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.Close();
        }
    }
}
