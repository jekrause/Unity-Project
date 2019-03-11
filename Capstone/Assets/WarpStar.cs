using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpStar : MonoBehaviour
{

    EndStar destination;

    private void Start()
    {
        //destination = (EndStar)GameObject.FindWithTag("EndStar");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //if (other is Player) or enemy
        {
            /*
            float xPos = destination.gameObject.GetComponent("Transform").transform.position.x;
            float yPos = destination.gameObject.GetComponent("Transform").transform.position.y;

            ((Player)other.gameObject.GetComponent("Player")).SetLocation(xPos, yPos);
            */
            ((Player)other.gameObject.GetComponent("Player")).SetLocation(0, 0);
            Debug.Log("Entered WarpStar");

                //Vector2 pos = destination.transform.position;
                //((Player)other.gameObject.GetComponent("Player")).SetLocation(0, 0);
        }
    }

    void setDestination(EndStar dest)
    {
        destination = dest;
    }
}
