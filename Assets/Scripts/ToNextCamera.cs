using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class ToNextCamera : MonoBehaviour
{

    [Header("Scene 1")]
    [SerializeField]
    private GameObject[] Horses1;

    [SerializeField]
    private CinemachineVirtualCamera camera1;

    [SerializeField]
    private CinemachineDollyCart dolly1;


    [Header("Scene 2")]
    [SerializeField]
    private GameObject[] Horses2;

    [SerializeField]
    private CinemachineVirtualCamera camera2;
    [SerializeField]
    private CinemachineVirtualCamera camera2_2;

    private CinemachineVirtualCamera previousCamera;
    private CinemachineVirtualCamera currentCamera;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            Scene1();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Scene2();
        }
    }

    public void SwitchCamera(CinemachineVirtualCamera camera)
    {
        previousCamera = currentCamera;
        if (currentCamera != null)
        {
            previousCamera.Priority = 0;
        }
        currentCamera = camera;

        currentCamera.Priority = 99;
    }

    public void Scene1()
    {
        SwitchCamera(camera1);

        dolly1.enabled = true;
    }

    public void Scene2()
    {
        SwitchCamera(camera2);

        foreach (var horse in Horses1)
        {
            horse.GetComponent<SplineAnimate>().Restart(true);

            horse.GetComponentInChildren<Animator>().Play("Run");

            StartCoroutine(Scene2Switch(1f));
        }
    }

    private IEnumerator Scene2Switch(float delay)
    {
        yield return new WaitForSeconds(delay);

        SwitchCamera(camera2_2);

    }
}
