using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// should this be made abstract?
public class MossyConcreteTerrain : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D other)
    {
        //((Player)other?.gameObject?.GetComponent("Player"))?.MultiplyMoveRate(1/3f);
        Debug.Log("Player entered Mossy Concrete Terrain");
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
        Debug.Log("Player exited Mossy Conrete Terrain");

    }

    // should change footstep frequency with velocity.magnitude
    void createWalkingSound(Vector2 vect)
    {
        //TODO
    }

}
