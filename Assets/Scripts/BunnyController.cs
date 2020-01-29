using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BunnyController : MonoBehaviour
{
    [Header("WALK/RUN Settings")]
    public float WalkSpeed = 2;
    public float RunSpeed = 3;
    public float CurrentSpeed;
    public bool IsRunning;

    [Header("JoysTick Settings")]
    public Joystick MoveStick;
    public Joystick AimStick;
    [Range(0f, 0.5f)] public float Offset;

    private float _xAxis;
    private float _zAxis;
    private float _rot;

    [Header("Shooting settings")]
    public Transform FirePoint;
    public GameObject BulletPrefab;
    public float BulletForce = 20f;
    public float ShootDelay = .3f;
    public float RotationSmoothnes = 7f;

    private float lastShootDate;

    // Animator
    private Animator anim;

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        lastShootDate = Time.time;
    }

    // Non-physics related logic p/frame
    bool Shooting = false;
    private void Update()
    {
        // Get movement input
        // Only triggers if the joystick displacement is > than offset for the given axis (better usability).
        if (MoveStick.Horizontal <= -Offset || MoveStick.Horizontal >= Offset || MoveStick.Vertical <= -Offset || MoveStick.Vertical >= Offset)
        {
            _xAxis = MoveStick.Horizontal < 0 ? MoveStick.Horizontal : MoveStick.Horizontal;
            _zAxis = MoveStick.Vertical < 0 ? MoveStick.Vertical : MoveStick.Vertical;
        }
        // Resets movement values when the joystick is released.
        else
        {
            _zAxis = 0;
            _xAxis = 0;
            anim.SetBool("IsWalking", false);
        }

        //JOYSTICK ROTATION
        // Get rotation if the joystick is not iddle
        if (AimStick.Horizontal != 0f || AimStick.Vertical != 0f)
        {
            // Angle from the player to the given coordinates.
            _rot = Mathf.Atan2(AimStick.Horizontal, AimStick.Vertical);

            // As long as the aiming joystick is getting input, the player is shooting.
            Shooting = true;
        }
        else
        {
            Shooting = false;
        }

        // Adjust speed
        CurrentSpeed = IsRunning ? RunSpeed : WalkSpeed;
    }

    // FixedUpdate for any physics related logic p/frame
    // TEST
    private void FixedUpdate()
    {
        // Apply movement relative to world space.
        Vector3 move = new Vector3(_xAxis, 0f, _zAxis);
        move.Normalize();
        transform.position += move * CurrentSpeed * Time.deltaTime;
        anim.SetBool("IsWalking", true);


        // Apply rotation JOYSTICK
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, _rot * Mathf.Rad2Deg, 0f), RotationSmoothnes * Time.deltaTime);

        // Shoot if the player is moving the aim joystick and ShootDelay has passed since the last time he shot.
        if (Shooting && Time.time - lastShootDate > ShootDelay)
            Shoot();
    }

    void Shoot()
    {
        GameObject projectile = Instantiate(BulletPrefab, FirePoint.position, FirePoint.rotation);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        // Add the force to the freshly spawned carrot
        rb.AddForce(FirePoint.up * BulletForce, ForceMode.Impulse);
        lastShootDate = Time.time;
    }

    #region UNUSED STUFF
    //---------- UNUSED STUFF ---------- (previous logic n failed attempts)

    // Movement (relative to local space)
    //  local movement changes directions based on local rotations.

    //      _rb.MovePosition(_rb.position + Time.deltaTime * CurrentSpeed *
    //      _rb.transform.TransformDirection(_xAxis, 0, _zAxis));


    // Rotation 
    // (first solution made axis to invert based on last rotation)
    // (second solution caused custom model to rotate around some buggy pivot, the solution was to parent it with an invisible object and move/rotate that instead)

    //      _rb.transform.eulerAngles = _rot;
    //      transform.rotation = Quaternion.Euler(0f, lookAt * Mathf.Rad2Deg, 0f);

    //// ROTATION TEST (ROTATE TO TOUCH SCREEN) -------------------------------------
    //if(Input.touchCount > 0) {
    //    Plane playerPlane = new Plane(Vector3.up, transform.position);
    //    Ray ray; /*= Camera.main.ScreenPointToRay(Input.GetTouch(0).position);*/

    //    // TESTING WORKAROUND TO DETECT IF MOVEMENT JOYSTICK IS BEING TOUCHED
    //    bool aiming = false;
    //    if (touchingStick && Input.touchCount > 1)
    //    {
    //        ray = Camera.main.ScreenPointToRay(Input.GetTouch(1).position);
    //        aiming = true;
    //    }
    //    else
    //    {
    //        if (!touchingStick) aiming = true;
    //        ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
    //    }

    //    // -----------------------------------------------------------------------

    //    float hitDist = .0f;
    //    if (playerPlane.Raycast(ray, out hitDist) && aiming)
    //    {
    //        Vector3 targetPoint = ray.GetPoint(hitDist);
    //        Quaternion targetRotation = Quaternion.LookRotation(targetPoint - parent.transform.position);
    //        targetRotation.x = 0; targetRotation.z = 0;

    //        parent.transform.rotation = Quaternion.Slerp(parent.transform.rotation, targetRotation, 7f * Time.deltaTime);
    //    }
    //    // ---------------------------------------------------
    //}

    //---------- EOF UNUSED STUFF ----------    
    #endregion
}
