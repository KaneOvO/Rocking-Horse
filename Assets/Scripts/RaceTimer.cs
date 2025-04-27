using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceTimer : MonoBehaviour
{
    public static RaceTimer Instance;
    public float timer;

    private bool isTiming;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        GameManager.GameStartEvent += StartTimer;
    }

    private void OnDisable()
    {
        GameManager.GameStartEvent -= StartTimer;
    }

    private void Update()
    {
        if (isTiming)
        {
            timer += Time.deltaTime;
        }
    }

    private void StartTimer()
    {
        isTiming = true;
    }

    public void ResetTimer()
    {
        timer = 0f;
        isTiming = false;
    }
}
