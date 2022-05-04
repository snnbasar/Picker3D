using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("PLAYER STATS")]
    [SerializeField] private float verticalSpeed; //Sets On Inspector
    [SerializeField] private float horizontalSpeed; //Sets On Inspector
    [Header("MOVE REGION")]
    [SerializeField] private float XClampMin; //Sets On Inspector
    [SerializeField] private float XClampMax; //Sets On Inspector
    [Header("REFERANCES")]
    [SerializeField] private PlayerTrigger playerTrigger; //Sets On Inspector

    
    private Rigidbody rb;
    private bool canMove;
    private bool moveOnHorizontal;
    private bool moveOnVertical;

    private bool forwardMoveWithForce;



    void Update()
    {
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, XClampMin, XClampMax), transform.position.y, transform.position.z);
    }

    
    private void Start()
    {
        PlayerManager.instance.SetPlayerController(this);
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
    }


    private void FixedUpdate()
    {
        if (canMove)
            Movement();
        if (forwardMoveWithForce)
            MoveForwardForNextLevelRamp();
    }

    private void MoveForwardForNextLevelRamp() //Just Forward Movement
    {
        float ver = (Input.GetAxis("Vertical") + 1) * verticalSpeed / 10;

        Vector3 move = new Vector3(0, rb.velocity.y, ver);

        rb.AddForce(transform.forward + move, ForceMode.Acceleration);
    }

    private void Movement() //Movement with both axis
    {
        
        float hor = 0;
        if(moveOnHorizontal)
            hor = (Input.GetAxis("Horizontal") + MouseInput.instance.MoveFactorX) * horizontalSpeed;
        float ver = 0;
        if (moveOnVertical)
            ver = (Input.GetAxis("Vertical") + 1) * verticalSpeed;

        Vector3 move = new Vector3(hor, rb.velocity.y, ver);

        //move = transform.TransformDirection(move);

        //rb.MovePosition(transform.position + move * Time.deltaTime * playerSpeed);
        rb.velocity = (transform.forward + move) * Time.deltaTime;
        //rb.AddForce(move);

    }

    

    public void CanPlayerMove(bool status) 
    {
        canMove = status;
        CanPlayerMoveOnHorizontal(status);
        CanPlayerMoveOnVertical(status);
        LockPlayersRigidbodyRotations(status);
        rb.velocity = (status == true) ? rb.velocity : Vector3.zero;
        rb.angularVelocity = (status == true) ? rb.angularVelocity : Vector3.zero;
        //rb.isKinematic = !status;
    }

    public void CanPlayerMoveOnHorizontal(bool status)
    {
        moveOnHorizontal = status;
    }

    public void CanPlayerMoveOnVertical(bool status)
    {
        moveOnVertical = status;
    }

    public Rigidbody GetPlayersRigidbody()
    {
        return rb;
    }

    public PlayerTrigger GetPlayerTrigger()
    {
        return playerTrigger;
    }

    public void LockPlayersRigidbodyRotations(bool status)
    {
        GetPlayersRigidbody().constraints = (status) ? RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ : RigidbodyConstraints.None;
    }

    public void CanPlayerDoForwardMove(bool status)
    {
        forwardMoveWithForce = status;
    }
    
    public void SwitchMovementToForward(bool status)
    {
        CanPlayerMove(!status);
        CanPlayerDoForwardMove(status);
    }

}
