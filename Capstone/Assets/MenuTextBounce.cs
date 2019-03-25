using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuTextBounce : MonoBehaviour
{
    // Start is called before the first frame update

    public int maxMoveTime;
    public float moveSpeed;

    private bool moveUp = true;
    private Vector3 newPosition;
    private int moveCount;

    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (moveUp == true)
        {
            newPosition = new Vector3(0, moveSpeed, 0);
            transform.position = transform.position + newPosition;
            moveCount++;

            if (moveCount > maxMoveTime)
            {
                moveUp = false;
                //Debug.Log("movingDown");
            }

        }
        else
        {
            newPosition = new Vector3(0, -moveSpeed, 0);
            transform.position = transform.position + newPosition; moveCount--;

            if (moveCount < 0)
            {
                moveUp = true;
                //Debug.Log("movingUp");
            }
        }


    }
}
