using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UIElements;
using UnityEngine.UI;
using Toggle = UnityEngine.UIElements.Toggle;

public class optionScript : MonoBehaviour
{
    private keyBinds currentKeyState;
    private KeyCode lastPressed;
    private bool waiting = false;
    private bool isFullScreen = false;

    public AudioMixer audioMixer;

    enum keyBinds
    {
        jump,
        crouch,
        sprint
    }
    private void Start()
    {
        Time.timeScale = 1;
 
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
        audioMixer.SetFloat("volume", vol);
    }

    public void setQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
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

    public void SetFullscreen()
    {
        isFullScreen = !isFullScreen;
        Screen.fullScreen = isFullScreen;
        print("Full Screen mode" + isFullScreen.ToString());
    }
}