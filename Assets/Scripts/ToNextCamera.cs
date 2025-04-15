using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Splines;
using UnityEngine.VFX;

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

    [Header("Scene 5")]
    [SerializeField]
    private GameObject[] Horses5;

    [SerializeField]
    private CinemachineVirtualCamera camera5;

    [SerializeField]
    private CinemachineVirtualCamera camera5_2;

    [Header("Scene 6")]
    [SerializeField]
    private CinemachineVirtualCamera camera6;

    [SerializeField]
    private CinemachineDollyCart dolly6;

    [Header("Scene 7")]
    [SerializeField]
    private CinemachineVirtualCamera camera7;

    [SerializeField]
    private CinemachineDollyCart dolly7;


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
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            Scene5();
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            Scene6();
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            Scene7();
        }

        if (spawnedChicken !=null)
        {

            gameLerp += Time.deltaTime * lerp;

            Vector3 tmep = Vector3.Lerp(Horses4[1].transform.position, Horses4[0].transform.position, gameLerp);

            spawnedChicken.transform.rotation = Quaternion.Slerp(spawnedChicken.transform.rotation, Horses4[0].transform.rotation, 1);

            spawnedChicken.transform.position = tmep;

            if(gameLerp > 1)
            {
                spawnedChicken.GetComponent<TrailerChicken>().Detonation();

                Horses4[0].GetComponent<SplineAnimate>().enabled = false;
                Horses4[0].GetComponentInChildren<Animator>().Play("Idle");
            }
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

        StartCoroutine(Scene2Start(1f));
        
    }

    private IEnumerator Scene2Start(float delay)
    {
        yield return new WaitForSeconds(delay);

        foreach (var horse in Horses1)
        {
            horse.GetComponent<SplineAnimate>().Restart(true);

            horse.GetComponentInChildren<Animator>().Play("Run");
        }

        StartCoroutine(Scene2Switch(1f));

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

    public void Scene5()
    {
        SwitchCamera(camera5);

        foreach (var horse in Horses5)
        {
            horse.SetActive(true);

            horse.GetComponent<SplineAnimate>().Restart(true);
            horse.GetComponentInChildren<Animator>().Play("Run");

            horse.GetComponentInChildren<Animator>().speed = 0.5f;
        }

        StartCoroutine(Scene5Switch(1f));

    }

    private IEnumerator Scene5Switch(float delay)
    {
        yield return new WaitForSeconds(delay);

        SwitchCamera(camera5_2);
    }

    public void Scene6()
    {
        SwitchCamera(camera6);

        dolly6.enabled = true;
    }

    public void Scene7()
    {
        SwitchCamera(camera7);

        dolly7.enabled = true;
    }


}
