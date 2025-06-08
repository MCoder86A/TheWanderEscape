using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLook : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if(Keyboard.current.hKey.isPressed) Cursor.lockState = CursorLockMode.Locked;
        if(Keyboard.current.escapeKey.isPressed) Cursor.lockState = CursorLockMode.None;
    }
}
