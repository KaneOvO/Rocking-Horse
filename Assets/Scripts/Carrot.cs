using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carrot : MonoBehaviour
{
    public float spinSpeed = 30f;
    public Vector3 rotationAxis = Vector3.up;
    private MeshRenderer MeshRenderer;

    private void Awake()
    {
        MeshRenderer = this.GetComponent<MeshRenderer>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(MeshRenderer.enabled)
        {
            transform.Rotate(rotationAxis, spinSpeed * Time.deltaTime);
        }
    }
}
