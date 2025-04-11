using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    [Header("Scene 3")]
    [SerializeField]
    private GameObject[] Horses3;

    [SerializeField]
    private CinemachineVirtualCamera camera3;

    [Header("Scene 4")]
    [SerializeField]
    private GameObject[] Horses4;

    [SerializeField]
    private CinemachineVirtualCamera camera4;

    [SerializeField]
    private CinemachineVirtualCamera camera4_2;

    [SerializeField]
    private CinemachineDollyCart dolly4;

    [SerializeField]
    private GameObject chicken;

    private GameObject spawnedChicken;

    [SerializeField]
    private float lerp;

    private float gameLerp;

    [SerializeField]
    private GameObject chickenSpawn;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Scene1();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Scene2();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Scene3();
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Scene4();
        }
       
        if(spawnedChicken !=null)
        {

            gameLerp += Time.deltaTime * lerp;

            Vector3 tmep = Vector3.Lerp(Horses4[1].transform.position, Horses4[0].transform.position, gameLerp);

            spawnedChicken.transform.rotation = Quaternion.Slerp(spawnedChicken.transform.rotation, Horses4[0].transform.rotation, 1);

            spawnedChicken.transform.position = tmep;
        }
        else
        {
            gameLerp = 0;
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

    public void Scene3()
    {
        SwitchCamera(camera3);
        foreach (var horse in Horses3)
        {
            horse.GetComponent<SplineAnimate>().Restart(true);

            horse.GetComponentInChildren<Animator>().Play("Run");
        }
    }

    public void Scene4()
    {
        SwitchCamera(camera4);
        foreach (var horse in Horses4)
        {
            horse.GetComponent<SplineAnimate>().Restart(true);

            horse.GetComponentInChildren<Animator>().Play("Run");

            StartCoroutine(Scene4Switch1(1.25f));
        }
    }

    private IEnumerator Scene4Switch1(float delay)
    {
        yield return new WaitForSeconds(delay);

        SwitchCamera(camera4_2);

        dolly4.enabled = true;

        StartCoroutine(Scene4Switch2(1f));
    }

    private IEnumerator Scene4Switch2(float delay)
    {
        yield return new WaitForSeconds(delay);

        SwitchCamera(camera4);

        spawnedChicken = Instantiate(chicken, chickenSpawn.transform.position, Quaternion.identity);

        camera4.LookAt = spawnedChicken.transform;
        camera4.Follow = spawnedChicken.transform;

    }

}
