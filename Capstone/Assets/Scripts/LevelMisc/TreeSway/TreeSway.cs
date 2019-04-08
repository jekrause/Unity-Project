using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeSway : MonoBehaviour
{
    [Range(-0.1f, 0.1f)]
    public float xSwaySpeed = 0;
    [Range(-0.1f, 0.1f)]
    public float ySwaySpeed = 0;
    [Range(0f, 1f)]
    public float xDistance = 0;
    [Range(0f, 1f)]
    public float yDistance = 0;

    [Range(0.1f, 5f)]
    public float scaleSpeed = 1;
    [Range(0.1f, 5f)]
    public float scaleDistance = 1;

    public bool getDataFromParent;
    public bool isParent;

    private float xOffset;
    private float yOffset;
    private bool isMovingRight;
    private bool isMovingUp;
    private Vector3 originalPosition;
    private Vector3 currentPosition;



    private void Awake()
    {
        xSwaySpeed = xSwaySpeed * scaleSpeed;
        ySwaySpeed = ySwaySpeed * scaleSpeed;
        xDistance = xDistance * scaleDistance;
        yDistance = yDistance * scaleDistance;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (isParent)
        {
            return;
        }

        isMovingRight = true;
        isMovingUp = true;
        originalPosition = transform.position;
        currentPosition = originalPosition;


        if (getDataFromParent)
        {
            xSwaySpeed = transform.parent.GetComponentInParent<TreeSway>().xSwaySpeed;
            ySwaySpeed = transform.parent.GetComponentInParent<TreeSway>().ySwaySpeed;
            xDistance = transform.parent.GetComponentInParent<TreeSway>().xDistance;
            yDistance = transform.parent.GetComponentInParent<TreeSway>().yDistance;
        }


    }

    // Update is called once per frame
    void Update()
    {
        if (isParent)
        {
            return;
        }


        if (isMovingRight)
        {
            xOffset = xOffset + xSwaySpeed;
            //currentPosition = originalPosition + new Vector3(xOffset, 0, 0);
            //transform.position = currentPosition;

            if (xOffset > xDistance)
            {
                isMovingRight = false;
            }
        }
        else
        {
            xOffset = xOffset - xSwaySpeed;
            //currentPosition = originalPosition + new Vector3(xOffset, 0, 0);
            //transform.position = currentPosition;

            if (xOffset < -xDistance)
            {
                isMovingRight = true;
            }
        }


        if (isMovingUp)
        {
            yOffset = yOffset + ySwaySpeed;
            //currentPosition = originalPosition + new Vector3(yOffset, yOffset, 0);
            //transform.position = currentPosition;

            if (yOffset > yDistance)
            {
                isMovingUp = false;
            }
        }
        else
        {
            yOffset = yOffset - ySwaySpeed;
            //currentPosition = originalPosition + new Vector3(yOffset, 0, 0);
            //transform.position = currentPosition;

            if (yOffset < -yDistance)
            {
                isMovingUp = true;
            }
        }


        currentPosition = originalPosition + new Vector3(xOffset, yOffset, 0);
        transform.position = currentPosition;

    }
}
