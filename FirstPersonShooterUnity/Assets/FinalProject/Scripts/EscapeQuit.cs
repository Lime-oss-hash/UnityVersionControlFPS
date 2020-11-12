using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeQuit : MonoBehaviour
{
    void Update()
    {
         if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Home))
        {
            Application.Quit();
        }  
    }
}


