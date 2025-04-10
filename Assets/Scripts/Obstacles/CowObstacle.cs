using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CowObstacle : MonoBehaviour
{
    [Header("Gameplay")]
    [SerializeField, Tooltip("The magnitude of the knockack force that the player will get when they collide with the cow")]
    private float knockbackMagnitude;

    [SerializeField, Tooltip("The time in seconds that will pass before the player can collide with the object again")]
    private float knockbackCooldown;

    //Variable that will hold the players that have recently collided with the obstacle to prevent multiple repeat overlaps.
    private List<GameObject> collidedPlayers;

    public void Start()
    {
        collidedPlayers = new List<GameObject>();
    }

    private void OnTriggerEnter(Collider collision)
    {

        Debug.Log("Collision started");

        Debug.Log(collision.gameObject.tag);

        //Check to see if a player is what collided with the obstacle
        if (collision.gameObject.CompareTag("Player"))
        {

            Debug.Log("Collided with a player");

            //Check to see if the player has been added to the collision array
            if (!collidedPlayers.Contains(collision.gameObject))
            {
                GameObject newPlayer = collision.gameObject;

                collidedPlayers.Add(newPlayer);

                Debug.Log("New Player, adding to list");

                Vector3 knockbackDirection = newPlayer.transform.forward * -1;
                knockbackDirection.y = 0.25f;
                knockbackDirection *= knockbackMagnitude;  
                
                newPlayer.GetComponent<Rigidbody>().AddForce(knockbackDirection, ForceMode.Acceleration);

                StartCoroutine(PlayerValidAgain(newPlayer));
            }
        }
    }

    private IEnumerator PlayerValidAgain(GameObject player)
    {
        yield return new WaitForSeconds(knockbackCooldown);

        Debug.Log("Removed player from list");

        if (collidedPlayers.Contains(player))
        {
            collidedPlayers.Remove(player);
        }
    }
}
