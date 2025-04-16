using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Listeners : MonoBehaviour
{
    public static Listeners instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
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
        // //for debug the singletons Listeners can be destroyed by pressing T
        // if(Input.GetKeyDown(KeyCode.T))
        // {
        //     UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        // }
    }
}
