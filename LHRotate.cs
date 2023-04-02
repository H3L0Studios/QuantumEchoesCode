//PURPOSE: SImple script that when attached to an object, causes it to spin on y axis. Could be extended to add different axises. That a word?
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LHRotate : MonoBehaviour
{
    [SerializeField] GameObject LHObject;
    [SerializeField] float speed = 0.1f;

    void Update()
    {
        LHObject.transform.Rotate(0.0f, speed, 0.0f,Space.World);
    }
}
