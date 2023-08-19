using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    [SerializeField] private GameObject door;
    [SerializeField] private bool isDown = false;
    private Animator leverAnimator;
    

    private void Start()
    {
        leverAnimator = GetComponent<Animator>();
    }

    public void LeverSwitch()
    {
        Door doorInstance = door.GetComponent<Door>();
        bool isSwitching = doorInstance.GetIsSwitching();
        if (!isSwitching)
        {
            isDown = !isDown;
            leverAnimator.SetBool("IsDown", isDown);
            doorInstance.SetSwitch();
        }
        
    }

}
