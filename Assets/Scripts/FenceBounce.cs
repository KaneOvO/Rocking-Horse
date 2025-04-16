using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FenceBounce : MonoBehaviour
{

    [SerializeField]
    private float forceAmount;

    private List <GameObject> collided;

    public void Start()
    {
        collided = new List<GameObject>();
    }

    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player") && !collided.Contains(collision.gameObject))
        {

            Vector3 forceDirection = collision.transform.forward * 0.5f;
            forceDirection.y = 0.25f;

            collision.gameObject.GetComponent<Rigidbody>().AddForce(forceDirection * forceAmount, ForceMode.Acceleration);

            StartCoroutine(ClearPlayer(collision.gameObject));
        }
    }

    private IEnumerator ClearPlayer(GameObject gameObject)
    {
        yield return new WaitForSeconds(1);

        collided.Remove(gameObject);

    }

}
