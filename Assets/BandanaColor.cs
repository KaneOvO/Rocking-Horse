using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BandanaColor : MonoBehaviour
{

    [SerializeField]
    private Material[] bandanaColors;

    [SerializeField]
    private MeshRenderer meshRenderer;

    void Start()
    {
        //meshRenderer = GetComponent<MeshRenderer>();
    }

    public void SetColor(int index)
    {
       meshRenderer.material = bandanaColors[index];
    }
}
