using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// should this be made abstract?
public class GrassyTerrain : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D other)
    {
        //((Player)other?.gameObject?.GetComponent("Player"))?.MultiplyMoveRate(1/3f);
        Debug.Log("Player entered Grassy Terrain");
    }

    // Start is called before the first frame update

    // make walking sound when moving
    void OnTriggerStay2D(Collider2D other)
    {
        // Player p = (Player) other.gameObject.GetComponent("Player");
        //TODO
    }


    void OnTriggerExit2D(Collider2D other)
    {
        //((Player)other?.gameObject?.GetComponent("Player"))?.MultiplyMoveRate(3f);
        Debug.Log("Player exited Grassy Terrain");

    }

    // should change footstep frequency with velocity.magnitude
    void createWalkingSound(Vector2 vect)
    {
        //TODO
    }

}
