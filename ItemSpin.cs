using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpin : MonoBehaviour
{
    public float spinspeed;

    // Update is called once per frame
    void Update()
    {
        transform.localRotation *= Quaternion.AngleAxis(spinspeed * Time.deltaTime, Vector3.back);
        transform.localRotation *= Quaternion.AngleAxis(spinspeed * Time.deltaTime, Vector3.left);
    }
}
