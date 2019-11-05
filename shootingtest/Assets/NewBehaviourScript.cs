using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
Debug.Log("here");
	OVRInput.Update();
					if(OVRInput.Get(OVRInput.Button.One))
			{
				Debug.Log("fire");
			}
	}
}
