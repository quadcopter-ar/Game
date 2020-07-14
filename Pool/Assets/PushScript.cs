using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PushScript : MonoBehaviour {
    private Rigidbody rbody;
    // Use this for initialization
    private float pushPower = 2.0f;
    private void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.back * 10f * Time.deltaTime);
            //transform.Rotate(Vector3.forward * moveSpeed * Time.deltaTime);

        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        rbody = GetComponent<Rigidbody>();
        //if (collision.collider.tag == "Bouncer")
        //{
            transform.Translate(Vector3.forward * 10f * Time.deltaTime);
        rbody.AddForce(8, 4, 0);
        //}
        //this.GetComponent<Rigidbody>().isKinematic = true;

        //this.GetComponent<Rigidbody>().velocity = Vector3.zero;
        //this.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }

}
