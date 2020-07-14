using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBallScript : MonoBehaviour
{
    public float moveSpeed = 10f;
    private Rigidbody rbody;
    // Use this for initialization
    void Start()
    {
        rbody = GetComponent<Rigidbody>();
        //_rb = GetComponent<Rigidbody>();
        // Increase max angular velocity or we won't see much spin.
        rbody.maxAngularVelocity = 1000;
        //StartCoroutine(ChangeRotation());
        var speed = rbody.velocity.magnitude;
        rbody.AddForce(8, 4, 0);

    }
    private IEnumerator ChangeRotation()
    {
        while (true)
        {
            rbody.AddTorque(new Vector3(10 * UnityEngine.Random.Range(0, 1f), UnityEngine.Random.Range(0, 1f), UnityEngine.Random.Range(0, 1f)), ForceMode.Impulse);
            yield return new WaitForSeconds(1);
        }
    }

    // Update is called once per frame
    void Update()
    {

        //float inputX = Input.GetAxis("Horizontal");
        //float inputZ = Input.GetAxis("Vertical");

        //float moveX = inputX * moveSpeed * Time.deltaTime;
        //float moveZ = inputZ * moveSpeed * Time.deltaTime;


        //transform.Translate(moveX, 0, moveZ);
        //rbody.AddForce(moveX, 0, moveZ);
        if (Input.GetKey(KeyCode.K))
        {
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }
        //Backward Movement
        if (Input.GetKey(KeyCode.I))
        {
            transform.Translate(Vector3.back * (moveSpeed - 2) * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.J))
        {
            transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.L))
        {
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.Space))
        {
            Vector3 up = transform.TransformDirection(Vector3.up);
            rbody.AddForce(up * 5, ForceMode.Impulse);
        }
        //rbody.AddForce(moveX, 0, moveZ);

    }



    private void OnCollisionEnter(Collision collision)
    {
        Vector3 left = transform.TransformDirection(Vector3.left);
        this.GetComponent<Rigidbody>().AddForce(left * 8, ForceMode.Impulse);

        //print(collision.collider.name);
        if ((collision.collider.name).Contains("pocket1"))
        {
            print("corner1");
            Debug.Log("corner1");
        }
        if ((collision.collider.name).Contains("pocket1p"))
        {
            print("corner1p");
            Debug.Log("corner1p");
        }
        if ((collision.collider.name).Contains("pocket2"))
        {
            print("corner2");
            Debug.Log("corner2");
        }
        if ((collision.collider.name).Contains("pocket2p"))
        {
            print("corner2p");
            Debug.Log("corner2p");
        }
        if ((collision.collider.name).Contains("pocket3"))
        {
            print("corner3");
            Debug.Log("corner3");
        }
        if ((collision.collider.name).Contains("pocket3p"))
        {
            print("corner3p");
            Debug.Log("corner3p");
        }
        if ((collision.collider.name).Contains("pocket4"))
        {
            print("corner4");
            Debug.Log("corner4");
        }
        if ((collision.collider.name).Contains("pocket4p"))
        {
            print("corner4p");
            Debug.Log("corner4p");
        }

        //if (rbody.velocity.x < 0.5 && rbody.velocity.y < 0.5 &&  rbody.velocity.z < 0.5) {
          //  Debug.Log("Red ball is stationary!");
        //}
        }
}
