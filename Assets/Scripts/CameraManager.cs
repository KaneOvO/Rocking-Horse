using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance { get; private set; }

    public GameObject camera1_;
    public GameObject camera2_;
    public GameObject camera3_;
    public GameObject camera4_;

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
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void TwoPlayeMode()
    {
        //camera1_.SetActive(true);
        camera2_.SetActive(true);
        camera1_.GetComponent<Camera>().rect = new Rect(0, 0, 0.5f, 1);
        camera2_.GetComponent<Camera>().rect = new Rect(0.5f, 0, 0.5f, 1);
    }

    void FourPlayerMode()
    {
        //camera1_.SetActive(true);
        camera2_.SetActive(true);
        camera3_.SetActive(true);
        camera4_.SetActive(true);
        camera1_.GetComponent<Camera>().rect = new Rect(0, 0.5f, 0.5f, 0.5f);
        camera2_.GetComponent<Camera>().rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
        camera3_.GetComponent<Camera>().rect = new Rect(0, 0, 0.5f, 0.5f);
        camera4_.GetComponent<Camera>().rect = new Rect(0.5f, 0, 0.5f, 0.5f);
    }

    void OnDisable()
    {
        camera2_.SetActive(false);
        camera3_.SetActive(false);
        camera4_.SetActive(false);
        camera1_.GetComponent<Camera>().rect = new Rect(0, 0, 1, 1);
        
    }
}
