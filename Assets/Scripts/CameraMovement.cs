using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public GameObject target;
    public Vector3 offset;

    private void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, target.transform.position + offset, Time.deltaTime);
    }
}
