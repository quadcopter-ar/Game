using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneConfiguration : MonoBehaviour {
	public float width = 1.0f;
	public float height = 1.0f;
	//public float depth = 1.0f;
	public Vector3 pos_offset;
	public Vector3 eul_offset;

	// Use this for initialization
	void Start () {
		//float depth = System.Math.Abs(GameObject.Find("StartPos1").transform.position.z - GameObject.Find("StartPos2").transform.position.z);
		float depth = 9.18f;
		GameObject.Find("MinimapPlane").GetComponent<Transform>().localScale = new Vector3(height / 30.0f, 1.0f, width / 30.0f);
		GameObject.Find("MinimapSidePlane").GetComponent<Transform>().localScale = new Vector3(depth / 30.0f, 1.0f, width / 30.0f);
		//GameObject.Find("StartPos1").GetComponent<Transform>().position = new Vector3(0.0f, 0.0f, 0.0f);
		//GameObject.Find("StartPos2").GetComponent<Transform>().position = pos_offset;
		//GameObject.Find("StartPos2").GetComponent<Transform>().eulerAngles = eul_offset;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
