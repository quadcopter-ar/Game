using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneConfiguration : MonoBehaviour {
	[Header("Config")]
	public bool useROS = false;
	public bool collisionDetection = true;
	public bool seeOpponent = true;
	public bool singlePlayer = false;
	public bool dynamicAdjust = false;
	public bool minimapOnPanel = false;
	
	//public GameObject gameObject = ;
	[Header("ROS")]
	public Vector3 positionScale;
	public string remoteIP = "192.168.1.250";
	public string publishingTopic = "joy";
	public string subscribingTopic = "fiducial_pose_corrected";
	public bool isSimulation = false; // ROS simulation.
	public bool enableRotation = false;
	public bool enableLowPassFilter = false;
	public int nTaps = 51;
	public double Fs = 44.1, Fx = 2.0;
	public bool enableMyFilter = false;
	public int bufferSize = 5;

	/*Game*/
	[Header("Game")]
	public float speed = 1f;
	[Range(0.0f, 1.0f)]
	public float videoDistance = 1.0f;
	public float width = 1.0f;
	public float height = 1.0f;
	public float depth = 1.0f;
	public Vector3 pos_offset;
	public Vector3 eul_offset;
	public RenderTexture minimapTexture;
	public RenderTexture sideMinimapTexture;

	// Use this for initialization
	void Start () {
		//float depth = System.Math.Abs(GameObject.Find("StartPos1").transform.position.z - GameObject.Find("StartPos2").transform.position.z);
		//float depth = 9.18f;
		//QualitySettings.masterTextureLimit = 1;
		//side walls
		if(dynamicAdjust)
		{
			GameObject.Find("Wall6").GetComponent<Transform>().position = new Vector3(width / 2, 0.0f, 0.0f);
			GameObject.Find("Wall6").GetComponent<Transform>().localScale = new Vector3(depth, height, 0.01f);
			GameObject.Find("Wall5").GetComponent<Transform>().position = new Vector3(-width / 2, 0.0f, 0.0f);
			GameObject.Find("Wall5").GetComponent<Transform>().localScale = new Vector3(depth, height, 0.01f);

			GameObject.Find("Wall8").GetComponent<Transform>().position = new Vector3(0.0f, 0.0f, depth / 2);
			GameObject.Find("Wall8").GetComponent<Transform>().localScale = new Vector3(width, height, 0.01f);
			GameObject.Find("Wall7").GetComponent<Transform>().position = new Vector3(0.0f, 0.0f, -depth / 2);
			GameObject.Find("Wall7").GetComponent<Transform>().localScale = new Vector3(width, height, 0.01f);

			//ceiling and floor
			GameObject.Find("Wall2").GetComponent<Transform>().position = new Vector3(0.0f, height / 2, 0.0f);
			GameObject.Find("Wall2").GetComponent<Transform>().localScale = new Vector3(depth, 0.01f, width);
			GameObject.Find("Wall").GetComponent<Transform>().position = new Vector3(0.0f, -height / 2, 0.0f);
			GameObject.Find("Wall").GetComponent<Transform>().localScale = new Vector3(depth, 0.01f, width);
		}
		


		//minimap setting
		GameObject.Find("MinimapPlane").GetComponent<Transform>().localScale = new Vector3(height / 30.0f, 1.0f, width / 30.0f);
		GameObject.Find("MinimapCamera").GetComponent<Camera>().rect = new Rect(0.0f, 0.0f, width, height);
		minimapTexture.height = (int)(height * 100);
		minimapTexture.width = (int)(width * 100);
		//minimapTexture = new RenderTexture((int)(width * 100), (int)(height * 100), 0);
		//GameObject.Find("MinimapCamera").GetComponent<Camera>().targetTexture = minimapTexture;
		GameObject.Find("MinimapSidePlane").GetComponent<Transform>().localScale = new Vector3(depth / 30.0f, 1.0f, width / 30.0f);
		GameObject.Find("MinimapSideCamera").GetComponent<Camera>().rect = new Rect(0.0f, 0.0f, width, depth);
		sideMinimapTexture.height = (int)(depth * 100);
		sideMinimapTexture.width = (int)(width * 100);

		float goalpos1 = GameObject.Find("goal1").GetComponent<Transform>().position.z;
		float goalpos2 = GameObject.Find("goal2").GetComponent<Transform>().position.z;
		GameObject.Find("goalindicator1").GetComponent<Transform>().position = new Vector3(0.0f, goalpos1 / 3.0f,-200.0f);
		GameObject.Find("goalindicator1").GetComponent<Transform>().localScale = new Vector3(0.01f, 0.04f, width / 3.0f);
		GameObject.Find("goalindicator2").GetComponent<Transform>().position = new Vector3(0.0f, goalpos2 / 3.0f, -200.0f);
		GameObject.Find("goalindicator2").GetComponent<Transform>().localScale = new Vector3(0.01f, 0.04f, width / 3.0f);

		//go pro video setting
		GameObject.Find("GoProPrefab").transform.Find("GoPro").GetComponent<Transform>().localPosition = new Vector3(0.0f, 0.0f, 3.0f * videoDistance);
	}
	
	// Update is called once per frame
	void Update () {
		//go pro video setting
		if (dynamicAdjust)
		{
			//side walls
			GameObject.Find("Wall6").GetComponent<Transform>().position = new Vector3(width / 2, 0.0f, 0.0f);
			GameObject.Find("Wall6").GetComponent<Transform>().localScale = new Vector3(depth, height, 0.01f);
			GameObject.Find("Wall5").GetComponent<Transform>().position = new Vector3(-width / 2, 0.0f, 0.0f);
			GameObject.Find("Wall5").GetComponent<Transform>().localScale = new Vector3(depth, height, 0.01f);

			GameObject.Find("Wall8").GetComponent<Transform>().position = new Vector3(0.0f, 0.0f, depth / 2);
			GameObject.Find("Wall8").GetComponent<Transform>().localScale = new Vector3(width, height, 0.01f);
			GameObject.Find("Wall7").GetComponent<Transform>().position = new Vector3(0.0f, 0.0f, -depth / 2);
			GameObject.Find("Wall7").GetComponent<Transform>().localScale = new Vector3(width, height, 0.01f);

			//ceiling and floor
			GameObject.Find("Wall2").GetComponent<Transform>().position = new Vector3(0.0f, height / 2, 0.0f);
			GameObject.Find("Wall2").GetComponent<Transform>().localScale = new Vector3(depth, 0.01f, width);
			GameObject.Find("Wall").GetComponent<Transform>().position = new Vector3(0.0f, -height / 2, 0.0f);
			GameObject.Find("Wall").GetComponent<Transform>().localScale = new Vector3(depth, 0.01f, width);


			//minimap setting
			GameObject.Find("MinimapPlane").GetComponent<Transform>().localScale = new Vector3(height / 30.0f, 1.0f, width / 30.0f);
			GameObject.Find("MinimapCamera").GetComponent<Camera>().rect = new Rect(0.0f, 0.0f, width, height);
			minimapTexture.height = (int)(height * 100);
			minimapTexture.width = (int)(width * 100);


			GameObject.Find("MinimapSidePlane").GetComponent<Transform>().localScale = new Vector3(depth / 30.0f, 1.0f, width / 30.0f);
			GameObject.Find("MinimapSideCamera").GetComponent<Camera>().rect = new Rect(0.0f, 0.0f, width, depth);
			sideMinimapTexture.height = (int)(depth * 100);
			sideMinimapTexture.width = (int)(width * 100);

			//go pro video setting
			GameObject.Find("GoProPrefab").transform.Find("GoPro").GetComponent<Transform>().localPosition = new Vector3(0.0f, 0.0f, 3.0f * videoDistance);
		}
	}
}
