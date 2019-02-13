using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InputType { NONE, KEYBOARD, PS4_CONTROLLER, XBOX_CONTROLLER }

public class MyControllerInput
{
    
    // -- Controller left and right analog sticks
    public string LeftHorizontalAxis { get; set; }
    public string LeftVerticalAxis { get; set; }
    public string RightHorizontalAxis { get; set; }
    public string RightVerticalAxis { get; set; }

    // -- Controller D-Pads
    public string LeftDPad { get; set; }
    public string RightDPad { get; set; }
    public string UpDPad { get; set; }
    public string DownDPad { get; set; }

    // -- Controller right side buttons
    public string DownButton { get; set; }    // PS4 - X Button || XBOX - A Button
    public string RightButton { get; set; }   // PS4 - O Button || XBOX - B Button
    public string UpButton { get; set; }      // PS4 - Triangle Button || XBOX - Y Button
    public string LeftButton { get; set; }    // PS4 - Square Button || XBOX - X Button
    

    // -- Controller bumpers
    public string LTrigger { get; set; }
    public string LBumper { get; set; }
    public string RTrigger { get; set; }
    public string RBumper { get; set; }

    public InputType inputType { get; set; } = InputType.NONE; // default 

    public MyControllerInput(InputType inputType, int joystickNum)
    {
        this.inputType = inputType;
        if(inputType == InputType.PS4_CONTROLLER || inputType == InputType.XBOX_CONTROLLER)
        {
            string controllerType = inputType == InputType.PS4_CONTROLLER ? "PS4" : "XBOX";

            UpButton = "J" + joystickNum + controllerType +"_UpButton";
            RightButton = "J" + joystickNum + controllerType + "_RightButton";
            LeftButton = "J" + joystickNum + controllerType + "_LeftButton";
            DownButton = "J" + joystickNum + controllerType + "_DownButton";
            RightHorizontalAxis = "J" + joystickNum + controllerType + "_Right_Horizontal_Axis";
            RightVerticalAxis = "J" + joystickNum + controllerType + "_Right_Vertical_Axis";
            LeftHorizontalAxis = "J" + joystickNum + controllerType + "_Left_Horizontal_Axis";
            LeftVerticalAxis = "J" + joystickNum + controllerType + "_Left_Vertical_Axis";
        }
        else
        {
            LeftHorizontalAxis = "KeyboardHorizontal";
            LeftVerticalAxis = "KeyboardVertical";
            RightButton = "KeyboardReload";
            LeftButton = "KeyboardBack";
        }

    }

    // so empty...
    public MyControllerInput() { }


}


