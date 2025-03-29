using UnityEngine;
using System.Threading;

/**
 * Serial Communication for ESP32 Bluetooth (SPP)
 * Modified for Bluetooth Classic
 */
public class SerialController : MonoBehaviour
{
    [Tooltip("Port name assigned to the ESP32 Bluetooth (e.g., COM5)")]
    public string portName = "COM13";  // Change this to your Bluetooth COM Port

    [Tooltip("Baud rate that the ESP32 is using.")]
    public int baudRate = 9600;  // Match the ESP32 baud rate

    [Tooltip("Reference to a GameObject that receives events.")]
    public GameObject messageListener;

    [Tooltip("Time to wait before reconnecting if disconnected.")]
    public int reconnectionDelay = 1000;

    [Tooltip("Max unread messages before discarding old ones.")]
    public int maxUnreadMessages = 1;

    private bool isSentMessage = false;
    private Thread thread;
    private SerialThreadLines serialThread;

    public const string SERIAL_DEVICE_CONNECTED = "__Connected__";
    public const string SERIAL_DEVICE_DISCONNECTED = "__Disconnected__";

    void OnEnable()
    {
        serialThread = new SerialThreadLines(portName, baudRate, reconnectionDelay, maxUnreadMessages);
        thread = new Thread(new ThreadStart(serialThread.RunForever));
        thread.Start();
    }

    void OnDisable()
    {
        if (serialThread != null)
        {
            serialThread.RequestStop();
            serialThread = null;
        }

        if (thread != null)
        {
            thread.Join();
            thread = null;
        }
    }

    void Update()
    {
        if (messageListener == null)
            return;

        string message = (string)serialThread.ReadMessage();
        if (message == null)
            return;

        if (ReferenceEquals(message, SERIAL_DEVICE_CONNECTED))
        {
            messageListener.SendMessage("OnConnectionEvent", true);

            if (!isSentMessage)
            {
                isSentMessage = true;
                SendSerialMessage("ESP32 Connected!");
            }
        }
        else if (ReferenceEquals(message, SERIAL_DEVICE_DISCONNECTED))
        {
            messageListener.SendMessage("OnConnectionEvent", false);
        }
        else
        {
            messageListener.SendMessage("OnMessageArrived", message);
        }
    }

    public string ReadSerialMessage()
    {
        return (string)serialThread.ReadMessage();
    }

    public void SendSerialMessage(string message)
    {
        serialThread.SendMessage(message);
    }

    public delegate void TearDownFunction();
    private TearDownFunction userDefinedTearDownFunction;

    public void SetTearDownFunction(TearDownFunction userFunction)
    {
        this.userDefinedTearDownFunction = userFunction;
    }
}
