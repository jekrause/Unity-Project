using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InputType { NONE, KEYBOARD, PS4_CONTROLLER, XBOX_CONTROLLER }

public class MyControllerInput
{
    
    // -- Controller left and right analog sticks
    public string LeftHorizontalAxis { get; private set; }
    public string LeftVerticalAxis { get; private set; }
    public string RightHorizontalAxis { get; private set; }
    public string RightVerticalAxis { get; private set; }

    // -- Controller D-Pads
    public string DPadX { get; private set; }
    public string DPadY { get; private set; }


    // -- Controller right side buttons
    public string DownButton { get; private set; }    // PS4 - X Button || XBOX - A Button
    public string RightButton { get; private set; }   // PS4 - O Button || XBOX - B Button
    public string UpButton { get; private set; }      // PS4 - Triangle Button || XBOX - Y Button
    public string LeftButton { get; private set; }    // PS4 - Square Button || XBOX - X Button
    

    // -- Controller bumpers
    public string LTrigger { get; private set; }
    public string LBumper { get; private set; }
    public string RTrigger { get; private set; }
    public string RBumper { get; private set; }

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
            DPadX = "J" + joystickNum + controllerType + "_DPadX_" + OS;
            DPadY = "J" + joystickNum + controllerType + "_DPadY_" + OS;
        }
        else
        {
            // key binds on keyboard subject to change depending on what you guys want
            LeftHorizontalAxis = "KeyboardHorizontal"; // keyboard a and d
            LeftVerticalAxis = "KeyboardVertical"; // keyboard w and s
            RightButton = "Keyboard_Reload"; // mouse right button
            LeftButton = "Keyboard_Escape";
            UpButton = "Keyboard_I";
            DownButton = "Keyboard_E";
            RTrigger = "Keyboard_Fire"; // mouse left button
            LTrigger = "Keyboard_Q";
            LBumper = "Keyboard_C";
            RBumper = "Keyboard_V";
            DPadX = "Keyboard_DPadX"; // keyboard left and right
            DPadY = "Keyboard_DPadY"; // keyboard up and down

        }

    }

    // so empty...
    public MyControllerInput() { }


}


