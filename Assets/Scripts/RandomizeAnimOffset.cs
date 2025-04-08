using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeAnimOffset : MonoBehaviour
{
    // Start is called before the first frame update

    private Animator animator;

    [SerializeField]
    private float maxOffset = 0;

    private float actualOffset = 0;

    private void Start()
    {
        animator = GetComponent<Animator>();

        actualOffset = Random.Range(0, maxOffset);

        animator.Play(0, 0, actualOffset);
    }
}
