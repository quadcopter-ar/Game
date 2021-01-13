using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public float pushForce = 10;
    public int maxBounces = 10;
    private int bounces = 0;

    [SerializeField]
    [Tooltip("Just for debugging, adds some velocity during OnEnable")]
    private Vector3 initialVelocity;

    [SerializeField]
    private float minVelocity;

    private Vector3 lastFrameVelocity;
    private Rigidbody rb;

    void Start()
    {
        // transform.position = new Vector3(0,30,0);
        // Vector3 newPos = transform.position;
        // newPos.x = newPos.x - Random.Range(0f, 10f);
        // newPos.y = newPos.y - Random.Range(0f, 10f);
        // newPos.z = newPos.z - Random.Range(0f, 10f);
        // transform.position = newPos;

    }

    private void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        //initialVelocity = new Vector3(Random.Range(-10f, 10f),Random.Range(-10f, 10f),Random.Range(-10f, 10f));
        rb.velocity = initialVelocity;
    }

    private void Update()
    {
        lastFrameVelocity = rb.velocity;

    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.collider.tag);
        Bounce(collision.contacts[0].normal);
    }

    private void Bounce(Vector3 collisionNormal)
    {

        var speed = lastFrameVelocity.magnitude;
        var direction = Vector3.Reflect(lastFrameVelocity.normalized, collisionNormal);

        if(bounces >= maxBounces){
            speed = 0;
        }
        Debug.Log("Out Direction: " + direction);
        rb.velocity = direction * Mathf.Max(speed, minVelocity);
        bounces++;

    }

}
