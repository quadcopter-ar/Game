using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneConfiguration : MonoBehaviour 
{
	[Header("Config")]
	public bool useROS = false;
	public bool dynamicAdjust = false;
	
	//public GameObject gameObject = ;
	[Header("ROS")]
	public Vector3 positionScale;
	public string remoteIP = "192.168.1.116";
	public string publishingTopic = "joy";
	public string subscribingTopic = "fiducial_pose_corrected";
	public bool enableRotation = true;
	public bool enableMyFilter = false;
	public int bufferSize = 5;

	/*Game*/
	[Header("Game")]
	//public float speed = 1f;
	[Range(0.0f, 1.0f)]
	public float videoDistance = 1.0f;
	public float width = 1.0f;
	public float height = 1.0f;
	public float depth = 1.0f;

	// Use this for initialization
	void Start () 
	{
		if(dynamicAdjust)
		{	
			//side walls
			GameObject.Find("Wall6").GetComponent<Transform>().position = new Vector3(width / 2, 0.0f, 0.0f);
			GameObject.Find("Wall6").GetComponent<Transform>().localScale = new Vector3(0.01f, height, depth);
			GameObject.Find("Wall5").GetComponent<Transform>().position = new Vector3(-width / 2, 0.0f, 0.0f);
			GameObject.Find("Wall5").GetComponent<Transform>().localScale = new Vector3(0.01f, height, depth);

			GameObject.Find("Wall4").GetComponent<Transform>().position = new Vector3(0.0f, 0.0f, depth / 2);
			GameObject.Find("Wall4").GetComponent<Transform>().localScale = new Vector3(width, height, 0.01f);
			GameObject.Find("Wall3").GetComponent<Transform>().position = new Vector3(0.0f, 0.0f, -depth / 2);
			GameObject.Find("Wall3").GetComponent<Transform>().localScale = new Vector3(width, height, 0.01f);


			//ceiling and floor
			GameObject.Find("Wall2").GetComponent<Transform>().position = new Vector3(0.0f, height / 2, 0.0f);
			GameObject.Find("Wall2").GetComponent<Transform>().localScale = new Vector3(width,  0.01f, depth);
			GameObject.Find("Wall1").GetComponent<Transform>().position = new Vector3(0.0f, -height / 2, 0.0f);
			GameObject.Find("Wall1").GetComponent<Transform>().localScale = new Vector3(width,  0.01f, depth);

		}
		

		//go pro video setting
		GameObject.Find("GoProPrefab").transform.Find("GoPro").GetComponent<Transform>().localPosition = new Vector3(0.0f, 0.0f, 5.0f * videoDistance);
	}
	
	// Update is called once per frame
	void Update () 
	{
		//go pro video setting
		if (dynamicAdjust)
		{
			GameObject.Find("Wall6").GetComponent<Transform>().position = new Vector3(width / 2, 0.0f, 0.0f);
			GameObject.Find("Wall6").GetComponent<Transform>().localScale = new Vector3(0.01f, height, depth);
			GameObject.Find("Wall5").GetComponent<Transform>().position = new Vector3(-width / 2, 0.0f, 0.0f);
			GameObject.Find("Wall5").GetComponent<Transform>().localScale = new Vector3(0.01f, height, depth);

			GameObject.Find("Wall4").GetComponent<Transform>().position = new Vector3(0.0f, 0.0f, depth / 2);
			GameObject.Find("Wall4").GetComponent<Transform>().localScale = new Vector3(width, height, 0.01f);
			GameObject.Find("Wall3").GetComponent<Transform>().position = new Vector3(0.0f, 0.0f, -depth / 2);
			GameObject.Find("Wall3").GetComponent<Transform>().localScale = new Vector3(width, height, 0.01f);


			//ceiling and floor
			GameObject.Find("Wall2").GetComponent<Transform>().position = new Vector3(0.0f, height / 2, 0.0f);
			GameObject.Find("Wall2").GetComponent<Transform>().localScale = new Vector3(width,  0.01f, depth);
			GameObject.Find("Wall1").GetComponent<Transform>().position = new Vector3(0.0f, -height / 2, 0.0f);
			GameObject.Find("Wall1").GetComponent<Transform>().localScale = new Vector3(width,  0.01f, depth);

			//go pro video setting
			GameObject.Find("GoProPrefab").transform.Find("GoPro").GetComponent<Transform>().localPosition = new Vector3(0.0f, 0.0f, 5.0f * videoDistance);
		}
	}
}
