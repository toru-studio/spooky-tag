using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;

public class optionScript : MonoBehaviour
{
    private keyBinds currentKeyState;
    private KeyCode lastPressed;
    private bool waiting = false;

    enum keyBinds
    {
        jump,
        crouch,
        sprint
    }


    void OnGUI()
    {
        if (waiting)
        {
            Event e = Event.current;
            if (e.isKey)
            {
                changeKeyBind(e.keyCode);
            }
        }
    }

    public void setVolume(float vol)
    {
        print("Volume: " + vol);
    }

    public void changeKeyState(string text)
    {
        waiting = true;
        currentKeyState = text switch
        {
            "jump" => keyBinds.jump,
            "sprint" => keyBinds.sprint,
            "crouch" => keyBinds.crouch,
            _ => currentKeyState
        };
    }

    private void changeKeyBind(KeyCode key)
    {
        switch (currentKeyState)
        {
            case keyBinds.jump:
                PlayerMovement.jumpKey = key;
                GameObject.Find("Jump Keybind").GetComponentInChildren<TextMeshProUGUI>().text = key.ToString();
                break;
            case keyBinds.sprint:
                GameObject.Find("Sprint Keybind").GetComponentInChildren<TextMeshProUGUI>().text = key.ToString();
                PlayerMovement.sprintKey = key;
                break;
            case keyBinds.crouch:
                GameObject.Find("Crouch Keybind").GetComponentInChildren<TextMeshProUGUI>().text = key.ToString();
                PlayerMovement.crouchKey = key;
                break;
        }

        waiting = false;
    }
}