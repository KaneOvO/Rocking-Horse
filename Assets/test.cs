using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<VisualEffect>().Play();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
