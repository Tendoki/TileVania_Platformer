using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Vector2 openPosition;
    [SerializeField] private Vector2 closePosition;
    [SerializeField] private float switchSpeed;
    [SerializeField] private float switchDelay;
    private bool isOpen = false;
    private bool isSwitching = false;

    private void Update()
    {
        SwitchOpenDoor();
    }

    private void SwitchOpenDoor()
    {
        if (isSwitching)
        {
            Vector3 targetPosition;
            if (!isOpen)
            {
                targetPosition = openPosition;
            }
            else
            {
                targetPosition = closePosition;
            }
            float moveSpeed = switchSpeed * Time.deltaTime;
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPosition, moveSpeed);

            if (transform.localPosition == targetPosition)
            {
                isOpen = !isOpen;
                isSwitching = false;
            }
        }
    }

    public bool GetIsSwitching()
    {
        return isSwitching;
    }

    public void SetSwitch()
    {
        if (!isSwitching)
        {
            StartCoroutine(SetSwitchDelay());
        }
    }

    IEnumerator SetSwitchDelay()
    {
        yield return new WaitForSeconds(switchDelay);
        isSwitching = !isSwitching;
    }

}
