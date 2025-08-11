using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float steerSpeed = 180.0f;
    private Rigidbody rb;
    private BoxCollider boxCollider;
    private float horizontalInput;
    public void Awake()
    {
        rb = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
    }

    public void Update()
    {
        //Flip();
        //float moveHorizontal = Input.GetAxis("Horizontal");
        //float moveVertical = Input.GetAxis("Vertical");
        //Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        float moveVertical = Input.GetAxis("Vertical");
        transform.position +=transform.forward * moveVertical * moveSpeed * Time.deltaTime;
        float steerDirection = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up * steerDirection * steerSpeed * Time.deltaTime);
    }

    //private void Flip()
    //{
    //    horizontalInput = Input.GetAxis("Horizontal");
    //    if (horizontalInput > 0.01f)
    //    {
    //        transform.localScale = Vector3.one;
    //    }
    //    else if (horizontalInput < -0.01f)
    //    {
    //        transform.localScale = new Vector3(-1, 1, 1);
    //    }
    //}               

}
