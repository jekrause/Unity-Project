using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InputType { NONE, KEYBOARD, PS4_CONTROLLER, XBOX_CONTROLLER }

public class MyControllerInput
{

    // -- Controller left and right analog sticks --//

    /// <summary>
    /// Use only in Input.GetAxis() method. This will be the Keyboard and console controller left and right movement direction.
    /// </summary>
    public string LeftHorizontalAxis { get; private set; }

    /// <summary>
    /// Use only in Input.GetAxis() method. This will be the Keyboard and console controller up and down movement direction.
    /// </summary>
    public string LeftVerticalAxis { get; private set; }

    /// <summary>
    /// Use only in Input.GetAxis() method and should only be used for Console controllers and NOT keyboard input.
    /// This will be the console controller left and right facing direction.
    /// </summary>
    public string RightHorizontalAxis { get; private set; }

    /// <summary>
    /// Use only in Input.GetAxis() method and should only be used for Console controllers and NOT keyboard input.
    /// This will be the console controller up and down facing direction.
    /// </summary>
    public string RightVerticalAxis { get; private set; }



    // -- Controller D-Pads --//

    /// <summary>
    /// Must be on Windows OS and use only in Input.GetAxis() method or (Input.GetKeyDown() for keyboard). Comparing the float value equal to 1 will determine if
    /// the (RIGHT D-Pad) was pressed and value equal to -1 for the oppisite button.
    /// </summary>
    public string DPadX_Windows { get; private set; }

    /// <summary>
    /// Must be on Windows OS and use only in Input.GetAxis() method or (Input.GetKeyDown() for keyboard). Comparing the float value equal to 1 will determine if
    /// the (UP D-Pad) was pressed and value equal to -1 for the oppisite button.
    /// </summary>
    public string DPadY_Windows { get; private set; }

    /// <summary>
    /// Must be on Mac OS and use only in Input.GetButton() method.
    /// </summary>
    public string DPadRight_Mac { get; private set; }

    /// <summary>
    /// Must be on Mac OS and use only in Input.GetButton() method.
    /// </summary>
    public string DPadLeft_Mac { get; private set; }

    /// <summary>
    /// Must be on Mac OS and use only in Input.GetButton() method.
    /// </summary>
    public string DPadUp_Mac { get; private set; }

    /// <summary>
    /// Must be on Mac OS and use only in Input.GetButton() method.
    /// </summary>
    public string DPadDown_Mac { get; private set; }



    // -- Controller right side buttons --//

    /// <summary>
    /// Use only in Input.GetButton(). This button is represented differently depending what platform it is  (PS4 - X Button || XBOX-360 - A Button)
    /// </summary>
    public string DownButton { get; private set; }

    /// <summary>
    /// Use only in Input.GetButton(). This button is represented differently depending what platform it is  (PS4 - O Button || XBOX-360 - B Button)
    /// </summary>
    public string RightButton { get; private set; }

    /// <summary>
    /// Use only in Input.GetButton(). This button is represented differently depending what platform it is  (PS4 - Triangle Button || XBOX-360 - Y Button)
    /// </summary>
    public string UpButton { get; private set; }

    /// <summary>
    /// Use only in Input.GetButton(). This button is represented differently depending what platform it is (PS4 - Square Button || XBOX-360 - X Button)
    /// </summary>
    public string LeftButton { get; private set; }


    // -- Controller Triggers and Bumpers --//

    /// <summary>
    /// Use only in Input.GetAxis() or (Input.GetButton()  for keyboard). Value return are -1 to 1 to distinguish if trigger was pressed.
    /// </summary>
    public string LTrigger { get; private set; }

    /// <summary>
    /// Use only in Input.GetButton().
    /// </summary>
    public string LBumper { get; private set; }

    /// <summary>
    /// Use only in Input.GetAxis() or (Input.GetButton()  for keyboard). Value return are -1 to 1 to distinguish if trigger was pressed.
    /// </summary>
    public string RTrigger { get; private set; }

    /// <summary>
    /// Use only in Input.GetButton().
    /// </summary>
    public string RBumper { get; private set; }


    /// <summary>
    /// The input type that is binded to the player (InputType.NONE if not binded). 
    /// </summary>
    public InputType inputType { get; set; } = InputType.NONE; // default 



    public MyControllerInput(InputType inputType, int joystickNum)
    {
        this.inputType = inputType;
        if(inputType == InputType.PS4_CONTROLLER || inputType == InputType.XBOX_CONTROLLER)
        {
            string controllerType = inputType == InputType.PS4_CONTROLLER ? "PS4" : "XBOX";
            string OS = Settings.OS;

            UpButton = "J" + joystickNum + controllerType +"_UpButton_" + OS;
            RightButton = "J" + joystickNum + controllerType + "_RightButton_" + OS;
            LeftButton = "J" + joystickNum + controllerType + "_LeftButton_" + OS;
            DownButton = "J" + joystickNum + controllerType + "_DownButton_" + OS;
            RightHorizontalAxis = "J" + joystickNum + controllerType + "_Right_Horizontal_Axis_" + OS; // only for controllers
            RightVerticalAxis = "J" + joystickNum + controllerType + "_Right_Vertical_Axis_" + OS;     // only for controllers
            LeftHorizontalAxis = "J" + joystickNum + controllerType + "_Left_Horizontal_Axis_" + OS;
            LeftVerticalAxis = "J" + joystickNum + controllerType + "_Left_Vertical_Axis_" + OS;
            LTrigger = "J" + joystickNum + controllerType + "_LTrigger_" + OS;
            RTrigger = "J" + joystickNum + controllerType + "_RTrigger_" + OS;
            LBumper = "J" + joystickNum + controllerType + "_LBumper_" + OS;
            RBumper = "J" + joystickNum + controllerType + "_RBumper_" + OS;
            if (Settings.OS.Equals("Windows") || controllerType.Equals("PS4"))
            {
                DPadX_Windows = "J" + joystickNum + controllerType + "_DPadX_" + OS;
                DPadY_Windows = "J" + joystickNum + controllerType + "_DPadY_" + OS;
            }
            else
            {
                DPadDown_Mac = "J" + joystickNum + controllerType + "_DPadDown_" + OS;
                DPadUp_Mac = "J" + joystickNum + controllerType + "_DPadUp_" + OS;
                DPadRight_Mac = "J" + joystickNum + controllerType + "_DPadRight_" + OS;
                DPadLeft_Mac = "J" + joystickNum + controllerType + "_DPadLeft_" + OS;
            }
            
        }
        else
        {
            // key binds on keyboard subject to change depending on what you guys want
            LeftHorizontalAxis = "KeyboardHorizontal"; // keyboard a and d
            LeftVerticalAxis = "KeyboardVertical"; // keyboard w and s
            LeftButton = "Keyboard_R";
            RightButton = "Keyboard_Escape";
            UpButton = "Keyboard_I";
            DownButton = "Keyboard_E";
            RTrigger = "Keyboard_Fire"; // mouse left button
            LTrigger = "Keyboard_Q";
            LBumper = "Keyboard_C";
            RBumper = "Keyboard_V";
            DPadX_Windows = "Keyboard_DPadX"; // keyboard left and right
            DPadY_Windows = "Keyboard_DPadY"; // keyboard up and down

        }

    }

    // so empty...
    public MyControllerInput() { }


}


