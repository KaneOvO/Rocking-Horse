using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CameraLookAt : MonoBehaviour
{
    // Start is called before the first frame update
    void Update()
    {
        if (Application.isPlaying)
        {
            transform.LookAt(Camera.main.transform.position, Vector3.up);
        }
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            transform.LookAt(SceneView.GetAllSceneCameras()[0].transform.position, Vector3.up);
        }
#endif

    }
}