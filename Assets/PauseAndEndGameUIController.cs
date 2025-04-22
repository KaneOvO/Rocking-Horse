using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseAndEndGameUIController : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject endGameCanvas;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.pauseMenu = pauseMenu;
        GameManager.Instance.endGameCanvas = endGameCanvas;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
