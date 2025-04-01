using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FenceBounce : MonoBehaviour
{

    [SerializeField]
    private float forceAmount;

    private List <GameObject> collided;

    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player") && !collided.Contains(collision.gameObject))
        {

            Vector3 forceDirection = collision.transform.forward *-1;

            collision.gameObject.GetComponent<Rigidbody>().AddForce(forceDirection * forceAmount);

            StartCoroutine(ClearPlayer(collision.gameObject));
        }
    }

    private IEnumerator ClearPlayer(GameObject gameObject)
    {
        yield return new WaitForSeconds(1);

        collided.Remove(gameObject);

    }

}
