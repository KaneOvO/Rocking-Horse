using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GyroMenuNavigator : MonoBehaviour
{
    public MyListener myListener; // Drag in your MyListener GameObject
    public List<Button> menuButtons; // Assign your buttons here
    public float scrollDelay = 1f;
    public float gyroThreshold = 5f; // Adjust based on your sensor values

    private int selectedIndex = 0;
    private float lastScrollTime = 0f;

    void OnEnable()
    {
        if (myListener != null)
            myListener.OnSensorDataUpdated += OnSensorUpdated;
    }

    void OnDisable()
    {
        if (myListener != null)
            myListener.OnSensorDataUpdated -= OnSensorUpdated;
    }

    void OnSensorUpdated(SensorData data)
    {
        float rotationZ = data.rotationZ;

        if (Time.time - lastScrollTime < scrollDelay)
            return;

        if (rotationZ > gyroThreshold)
        {
            ScrollDown();
            lastScrollTime = Time.time;
        }
        else if (rotationZ < -gyroThreshold)
        {
            ScrollUp();
            lastScrollTime = Time.time;
        }
    }

    void ScrollDown()
    {
        selectedIndex = (selectedIndex + 1) % menuButtons.Count;
        HighlightSelected();
    }

    void ScrollUp()
    {
        selectedIndex = (selectedIndex - 1 + menuButtons.Count) % menuButtons.Count;
        HighlightSelected();
    }

    void HighlightSelected()
    {
        for (int i = 0; i < menuButtons.Count; i++)
        {
            var colors = menuButtons[i].colors;
            colors.normalColor = (i == selectedIndex) ? Color.yellow : Color.white;
            menuButtons[i].colors = colors;
        }
    }

    void Update()
    {
        // Optional: Activate selected button with spacebar or custom input
        if (Input.GetKeyDown(KeyCode.Space))
        {
            menuButtons[selectedIndex].onClick.Invoke();
        }
    }
}

