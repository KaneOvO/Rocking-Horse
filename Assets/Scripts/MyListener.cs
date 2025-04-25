using UnityEngine;

public class MyListener : MonoBehaviour
{
    public SensorData sensorData;
    public bool isConnected;
    public bool isSentConnected = false;
    public System.Action<SensorData> OnEquipmentConnected;
    public System.Action<SensorData> OnSensorDataUpdated;
    public PlayerColor color;

    private float pressStartTime = 0f;
    private bool isPressing = false;

    void OnMessageArrived(string msg)
    {
        string[] msgSplit = msg.Split(',');

        if (msgSplit.Length < 1) return;

        sensorData.gyroscopeX = float.Parse(msgSplit[0]);

        if (msgSplit.Length < 2) return;
        sensorData.rotationZ = float.Parse(msgSplit[1]);

        if (msgSplit.Length > 2)
            sensorData.isBoosted = int.Parse(msgSplit[2]);

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

    public void Reset()
    {
        isConnected = false;
        isSentConnected = false;
    }

#if UNITY_EDITOR
    private void Update()
    {
        // Simulate gyro movement
        float gyroX = 0;
        if (Input.GetKey(KeyCode.LeftArrow)) gyroX = -5f;
        else if (Input.GetKey(KeyCode.RightArrow)) gyroX = 5f;

        float rotationZ = 0;
        int isBoosted = 0;

        // Detect press start
        if (Input.GetKeyDown(KeyCode.Space))
        {
            pressStartTime = Time.time;
            isPressing = true;
            isBoosted = 1;
        }

        // While holding
        if (Input.GetKey(KeyCode.Space))
        {
            isBoosted = 1;
        }

        // Detect release
        if (Input.GetKeyUp(KeyCode.Space))
        {
            isPressing = false;
            float heldTime = Time.time - pressStartTime;

            if (heldTime >= 10f)
            {
                Debug.Log("Long press detected");
                // MenuManager.Instance.BackToMenu(); // optional
            }
            else
            {
                Debug.Log("Short press detected");
            }

            isBoosted = 0;
        }

        // Send simulated sensor data
        string simulatedMsg = $"{gyroX},{rotationZ},{isBoosted}";
        OnMessageArrived(simulatedMsg);
    }
#endif
}
