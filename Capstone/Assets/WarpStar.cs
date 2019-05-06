using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpStar : MonoBehaviour
{
    int number;
    [SerializeField] public EndStar destination;

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
            if(other.gameObject.GetComponent<Player>() != null)
            {
                ((Player)other.gameObject.GetComponent("Player")).transform.position = destination.transform.position;
                //Debug.Log("Entered WarpStar");
            }
            else if(other.gameObject.GetComponent<ObstacleBox>() != null)
            {
                //Debug.Log("Box detected Warp");
                ((ObstacleBox)other.gameObject.GetComponent<ObstacleBox>()).transform.position = destination.transform.position;
            }
            /*else if(other.gameObject.GetComponent<Bullet>() != null)
            {
                Debug.Log("Bullet detected Warp");
                ((Bullet)other.gameObject.GetComponent<Bullet>()).transform.position = destination.transform.position;

            }
            */
            //Vector2 pos = destination.transform.position;
            //((Player)other.gameObject.GetComponent("Player")).SetLocation(0, 0);
        }
    }

    void setDestination(EndStar dest)
    {
        destination = dest;
    }
}
