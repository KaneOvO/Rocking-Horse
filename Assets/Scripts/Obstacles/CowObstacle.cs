using Character;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using Triggers;
using UnityEngine;
using UnityEngine.VFX;

public class CowObstacle : Trigger
{
    [Header("Gameplay")]
    [SerializeField, Tooltip("The magnitude of the knockack force that the player will get when they collide with the cow")]
    private float knockbackMagnitude;

    [SerializeField, Tooltip("The time in seconds that will pass before the player can collide with the object again")]
    private float knockbackCooldown;

    //Variable that will hold the players that have recently collided with the obstacle to prevent multiple repeat overlaps.
    private List<GameObject> collidedPlayers;

    [Header("Visual")]
    [SerializeField]
    private VisualEffect dustDevilVFX;

    [SerializeField]
    private float lifeTime;

    public void Start()
    {
        collidedPlayers = new List<GameObject>();

        StartCoroutine(DustDevilRestart(lifeTime));
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

    private IEnumerator DustDevilRestart(float delay)
    {
        yield return new WaitForSeconds(delay);

        dustDevilVFX.Play();

        StartCoroutine(DustDevilRestart(delay));
    }

    protected override void OnCharacterEnter(HorseController controller)
    {
        if (!collidedPlayers.Contains(controller.gameObject))
        {
            GameObject newPlayer = controller.gameObject;

            collidedPlayers.Add(newPlayer);

            Debug.Log("New Player, adding to list");

            Vector3 knockbackDirection = -newPlayer.transform.forward;
            Vector2 force = new Vector2(knockbackDirection.x, knockbackDirection.z);
            force *= knockbackMagnitude;
            controller.AdditionalForce += force;
            controller.OnHitDustDevil();

            StartCoroutine(PlayerValidAgain(newPlayer));
        }
        
    }
}
