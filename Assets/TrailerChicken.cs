using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class TrailerChicken : MonoBehaviour
{
    [SerializeField]
    private VisualEffect normal;

    [SerializeField]
    private VisualEffect explosion;

    [SerializeField]
    private VisualEffect feathers;

    public void Detonation()
    {
        normal.enabled = false;
        explosion.enabled = true;
        feathers.enabled = true;
    }
}
