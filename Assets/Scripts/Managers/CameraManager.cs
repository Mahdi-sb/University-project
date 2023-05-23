using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditorInternal;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;
    [SerializeField] private Vector3 playerCameraPosition;
    [SerializeField] private Vector3 enemyCameraPosition;

    private bool isLookingAtEnemy = false;
    private Coroutine currentCoroutine;

    void Start()
    {
        instance = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            SwipeUp();
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            SwipeDown();
        }
    }

    public void SwipeUp()
    {
        if (!isLookingAtEnemy)
        {
            if (currentCoroutine != null)
                StopCoroutine(currentCoroutine);
            isLookingAtEnemy = true;
            currentCoroutine = StartCoroutine(MoveToPosition(enemyCameraPosition));
        }
    }

    public void SwipeDown()
    {
        if (isLookingAtEnemy)
        {
            if (currentCoroutine != null)
                StopCoroutine(currentCoroutine);
            isLookingAtEnemy = false;
            currentCoroutine = StartCoroutine(MoveToPosition(playerCameraPosition));
        }
    }

    IEnumerator MoveToPosition(Vector3 position)
    {
        float t = 0;
        Vector3 cameraPosition = transform.position;
        while (t < 1)
        {
            t = Math.Min(t + Time.deltaTime * 4, 1);
            transform.position = Vector3.Lerp(cameraPosition, position, t);
            yield return null;
        }

        currentCoroutine = null;
    }
}


