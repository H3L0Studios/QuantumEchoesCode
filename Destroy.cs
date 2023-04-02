//PURPOSE: Simple script to destroy a gameobject this gets attached to after x time.
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    [SerializeField] float destroyTime = 0.2f; //time to wait before destroying

    void Start()
    {
        Destroy(gameObject, destroyTime);  //destroy the object this is attached to.
    }

}
