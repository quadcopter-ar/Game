using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveScript : MonoBehaviour {
    public float moveSpeed = 10f;
    private Rigidbody rbody,Rb;
    
    

    // Use this for initialization
    void Start () {
        GameObject body2 = GameObject.Find("Bouncer");
        rbody = GetComponent<Rigidbody>();
        Rb = body2.GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    


    void Update () {
        Vector3 v3=Vector3.zero;
        if (Input.GetKey(KeyCode.E))
        {
            
            

            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
            //this.GetComponent<Rigidbody>().AddForce(Vector3.back * 10.0f);
            //Vector3 back2 = transform.TransformDirection(Vector3.back * moveSpeed * Time.deltaTime);
            //rbody.AddForce(back2 * 5, ForceMode.Impulse);
        }


       
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);
            //transform.Rotate(Vector3.forward * moveSpeed * Time.deltaTime);
            
        }
        //transform.Translate(Vector3.forward * Time.deltaTime);
        //transform.Translate(Vector3.up * Time.deltaTime, Space.World);

        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.right * (moveSpeed - 2) * Time.deltaTime);
            //transform.Rotate(Vector3.back * moveSpeed * Time.deltaTime);
            
        }
        if ((Input.GetKey(KeyCode.A)))
        {
            ///transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
            //transform.Rotate(Vector3.left * moveSpeed * Time.deltaTime);
            this.GetComponent<Rigidbody>().MovePosition(transform.position + (transform.forward - transform.right).normalized * moveSpeed * Time.deltaTime);
            //Vector3 dir = Quaternion.AngleAxis(35, Vector3.forward) * Vector3.left;

        }
        if (Input.GetKey(KeyCode.D))
        {
            //transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
            //transform.Rotate(Vector3.right * moveSpeed * Time.deltaTime);
            // this.GetComponent<Rigidbody>().MovePosition(transform.position + (transform.forward - transform.right).normalized * moveSpeed * Time.deltaTime);
            v3 += Vector3.left;
        }

        transform.Translate(moveSpeed * v3.normalized * Time.deltaTime);
        
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        //rbody.AddForce(8, 4, 0);
        
        //this.GetComponent<Rigidbody>().isKinematic = true;
        //this.GetComponent<Rigidbody>().AddForce(0, 8, 0, ForceMode.Impulse);
        this.GetComponent<Rigidbody>().velocity = Vector3.zero;
        this.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        this.GetComponent<Rigidbody>().isKinematic = true;
        if (Rb.velocity.x <0.1 && Rb.velocity.y < 0.1 && Rb.velocity.z < 0.1)
        {
            Debug.Log("YAYYYYYY!");
            rbody.transform.position = Vector2.MoveTowards(rbody.transform.position, Rb.transform.position, 10f * Time.deltaTime);
            
        }

    }
}
