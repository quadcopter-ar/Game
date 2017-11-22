using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour {

	public bool isPaddle1;
	public float speed = 5f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(isPaddle1){
			transform.Translate (Input.GetAxis ("Horizontal") * speed * Time.deltaTime, Input.GetAxis ("Vertical") * speed * Time.deltaTime, 0f);

		}
		else {
			transform.Translate (Input.GetAxis ("Horizontal2") * speed * Time.deltaTime, Input.GetAxis ("Vertical2") * speed * Time.deltaTime, 0f);
		}
	}

}
