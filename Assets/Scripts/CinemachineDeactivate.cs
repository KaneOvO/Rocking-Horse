using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemachineDeactivate : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera cVCamera;

    [SerializeField]
    private float endPathUnits;

    private CinemachineDollyCart dolly;

    [SerializeField]
    private float delayTime;

    // Start is called before the first frame update
    void Start()
    {
     //   cVCamera = GetComponent<CinemachineVirtualCamera>();
        dolly = GetComponent<CinemachineDollyCart>();
    }

    public void Update()
    {
        if (dolly.m_Position >= endPathUnits)
        {
            //TurnOffCamera();
            StartCoroutine(delayedTurnOff());
        }
    }
    public void TurnOffCamera()
    {
        Debug.Log("Camera should be disabled");
        cVCamera.enabled = false;
    }

    private IEnumerator delayedTurnOff()
    {
        yield return new WaitForSeconds (delayTime);
        TurnOffCamera();
    }
}
