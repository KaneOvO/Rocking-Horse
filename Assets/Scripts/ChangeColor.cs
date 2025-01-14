using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColor : MonoBehaviour
{
    public Color color = Color.red;

    // Start is called before the first frame update
    void Start()
    {
        Renderer renderer = GetComponent<Renderer>();

        if (renderer != null)
        {
            renderer.material.color = color;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
