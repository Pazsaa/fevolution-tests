using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    // Script for the main camera 
    // Follows the player from the offsted distances (on each axis).

    [Header("Target to follow")]
    [SerializeField]
    public GameObject Target;
    [Header("Offsets")]
    [SerializeField]
    public float Offset_x; public float Offset_y; public float Offset_z;

    private void LateUpdate()
    {
        transform.position = new Vector3(Target.transform.position.x + Offset_x, 
                                         Target.transform.position.y + Offset_y, 
                                         Target.transform.position.z + Offset_z);
    }
}
