using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // player's movement
    public float movementSpeed = 10f;
    private Rigidbody2D rb;
    private Vector2 velocity;
    private Vector2 joyVector;

    // mouse
    private Vector2 direction;

    // input selection
    public bool useKeyboard = false;
    //public bool usePS4Controller = false;
    
    
    public int playerNum;
    private string[] controllerAxis;

    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //string controllerType = "XBOX"; //default controller
        //if (!useKeyboard)
        //{
        //    if (usePS4Controller)
        //    {
        //        controllerType = "PS4";
        //    }
           
        //}
        ////initialize which controller to use for the player
        //controllerAxis = new string[] 
        //{ "P" + playerNum + "_" + controllerType + "_Left_JoyStick_Horizontal",
        //  "P" + playerNum + "_" + controllerType + "_Left_JoyStick_Vertical",
        //  "P" + playerNum + "_" + controllerType + "_Right_JoyStick_Horizontal",
        //  "P" + playerNum + "_" + controllerType + "_Right_JoyStick_Vertical"
        //};
    }


    void Update()
    {
        getMovementInput();
        getRotationPosition();
        rb.MovePosition(rb.position + velocity * Time.deltaTime); // move the player after updating user input
    }


    void getMovementInput()
    {
        float moveHorizontal = 0;
        float moveVertical = 0;

        if (useKeyboard)
        {
            moveHorizontal = Input.GetAxis("KeyboardHorizontal");
            moveVertical = Input.GetAxis("KeyboardVertical");
        }
       
        // -- controler axis
        //moveHorizontal = Input.GetAxis(controllerAxis[0]);
        //moveVertical = -Input.GetAxis(controllerAxis[1]);
        
        velocity = new Vector2(moveHorizontal, moveVertical) * movementSpeed;
    }

    void getRotationPosition()
    {
        if (useKeyboard)
        {
           Vector3 position = Input.mousePosition;
           position = Camera.main.ScreenToWorldPoint(position);
           direction = new Vector2(position.x - transform.position.x, position.y - transform.position.y);
          transform.right = direction; //transform may vary depending on sprite's image
        }

        //else use controller inputs 

        //{   
            
        //    float horizontal = Input.GetAxisRaw(controllerAxis[2]);
        //    float vertical = Input.GetAxisRaw(controllerAxis[3]);
        //    if(horizontal != 0 || vertical != 0)
        //    {
        //        Vector2 lookDirection = new Vector2(horizontal, vertical);
        //        transform.right = lookDirection;
        //    }
            
          
        //}

    }

}
