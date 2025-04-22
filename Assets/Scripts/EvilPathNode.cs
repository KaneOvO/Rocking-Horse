using Character;
using NPC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvilPathNode : MonoBehaviour
{

    private int nodeIndex;

    [SerializeField]
    private PathPoint selfPath;

    [SerializeField]
    public bool isLastNode;

    // Start is called before the first frame update
    void Start()
    {
        //nodeIndex = FindObjectOfType<NPCMap>().Path.FindIndex(selfPath => selfPath != null);
        for(int i=0; i<FindObjectOfType<NPCMap>().Path.Capacity; i++)
        {
            if(selfPath == FindObjectOfType<NPCMap>().Path[i])
            {
                nodeIndex = i;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {

            if (isLastNode)
            {
                other.gameObject.GetComponent<HorseController>().Lapped();
            }
            else
            {
                other.gameObject.GetComponent<HorseController>().PassedNode(nodeIndex);
            }
        }
    }
}
