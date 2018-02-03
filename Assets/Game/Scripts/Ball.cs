using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ball : MonoBehaviour {

	public float speed = 5f;
	//private int count2;
	//public Text winText;
	public Text countText;
	private int count1;

	public AudioClip goalSound;
	public AudioClip wallSound;
	public AudioClip paddleSound;
	public float vol = 0.5f;

	private AudioSource source;

	// Use this for initialization
	void Start () {

		/*float sx = Random.Range (0, 2) == 0 ? -1 : 1;
		float sy = Random.Range (0, 2) == 0 ? -1 : 1;
		float sz = Random.Range (0f, 1f) == 0f ? -.5f : .5f;

		GetComponent<Rigidbody> ().velocity = new Vector3 (speed * sx, speed * sy, speed * sz);
		//winText.text = "";
		count1 = 0;*/

		//SetCountText ();
	}

	private void Awake()
	{
		source = GetComponent<AudioSource>();
	}

	// Update is called once per frame
	void Update () {
		//Debug.Log(gameObject.GetComponent<Rigidbody>().velocity.magnitude);
		//Vector3 v = gameObject.GetComponent<Rigidbody>().velocity;
		//gameObject.GetComponent<Rigidbody>().velocity = v.normalized * speed;
	}

	void OnTriggerEnter(Collider other)
	{
		
		if(other.gameObject.CompareTag ("goal1")) 
		{
			
			//count1 = count1 + 1;
			GetComponent<Rigidbody> ().position = Vector3.zero;
			//SetCountText ();

		}
		if (other.gameObject.CompareTag ("goal2")) 
		{
			//count2 = count2 + 1;
			GetComponent<Rigidbody> ().position = Vector3.zero;
		}
		source.PlayOneShot(goalSound, vol);
		float sx = Random.Range(-1.0f, 1.0f);
		float sy = Random.Range(-(float)System.Math.Sqrt(1-sx*sx), (float)System.Math.Sqrt(1 - sx * sx));
		//float sz = Random.Range(-0.5f, 0.5f);
		float sz = ((Random.Range(0.0f, 1.0f)>0.5f)?-1.0f:1.0f) *(float)System.Math.Sqrt(1 - sx * sx - sy * sy);
		//float sx = 1.0f;
		//float sy = 1.0f;
		//float sz = -1.0f;
		gameObject.GetComponent<Rigidbody>().velocity = new Vector3(speed * sx, speed * sy, speed * sz);
	}

	private void OnCollisionEnter(Collision collision)
	{
		//Debug.Log(gameObject.GetComponent<Rigidbody>().velocity.magnitude);
		//Debug.Log(gameObject.GetComponent<Rigidbody>().velocity.magnitude);
		Debug.Log(gameObject.GetComponent<Rigidbody>().velocity);
		if (collision.collider.gameObject.CompareTag("wall")) source.PlayOneShot(wallSound, vol);
		if (collision.collider.gameObject.CompareTag("paddle")) source.PlayOneShot(paddleSound, vol);
	}
	private void OnCollisionExit(Collision collision)
	{

	}


	//void SetCountText ()
	//{
	//countText.text = "Count 1: " + count1.ToString ();
	/*if (count1 >= 3)
	{
		winText.text = "You Win!";
	}*/
	//}
}
