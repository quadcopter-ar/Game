using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Paddle : NetworkBehaviour {

	//public bool isPaddle1;
	/*ROS Client*/
	private SceneConfiguration config;
	ROSClient rosClient;
	int[] buttons;
	float[] axes;
	string msg;

	/*[Header("Config")]
	public bool useROS = false;
	public bool seeOpponent = true;
	public bool singlePlayer = false;
	[Range(0.0f, 1.0f)]
	public float videoDistance = 1.0f;
	//public GameObject gameObject = ;
	[Header("ROS")]
	public Vector3 positionScale;
	public string remoteIP = "192.168.1.116";
	public string publishingTopic = "joy";
	public string subscribingTopic = "fiducial_pose_corrected";
	public bool isSimulation = false; // ROS simulation.
	public bool enableRotation = false;
	public bool enableLowPassFilter = false;
	public int nTaps = 51;
	public double Fs = 44.1, Fx = 2.0;
	public bool enableMyFilter = false;
	public int bufferSize = 5;*/

	/*Game*/
	Vector3 positionOffset;
	Vector3 orientationOffset;
	/*[Header("Game")]
	public float speed = 1f;*/
	[Range(0.0f, 1.0f)]
	public float colliderDist = 0.1f;
	public GameObject ballprefab;
	[SyncVar]
	public bool ready = false;

	private Vector3 scale1;
	private Vector3 scale2;

	[Command]
	void CmdSpawnBall() {
		Quaternion rot = new Quaternion();
		rot.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
		Vector3 Pos = (GameObject.Find("StartPos1").GetComponent<Transform>().position + GameObject.Find("StartPos2").GetComponent<Transform>().position) / 2.0f;
		Vector3 ballPos = new Vector3(Pos.x, Pos.y+0.2f, Pos.z);

		var ball = (GameObject)Instantiate(ballprefab, ballPos, rot);
		float sx = 0.0f;
		float sy = 0.0f;
		float sz = -1.0f;
		//float sx = Random.Range(-0.5f, 0.5f);
		//float sy = Random.Range(-0.5f, 0.5f);
		//float sz = Random.Range(-0.5f, 0.5f);
		ball.GetComponent<Rigidbody> ().velocity = new Vector3 (config.speed * sx, config.speed * sy, config.speed * sz);
		NetworkServer.Spawn (ball);
	}

	public override void OnStartClient() {
		config = GameObject.Find("Config").GetComponent<SceneConfiguration>();
		if (!isServer || config.singlePlayer)
		{
			Debug.Log("TEst");
			ready = true;
		}
	}

	void Start() 
	{
		if (!config.seeOpponent && !isLocalPlayer) gameObject.transform.Find("cockpit.785").gameObject.SetActive(false);
		if (!isLocalPlayer) return;
		/*adjust two minimaps*/
		var minimapp = gameObject.transform.Find("Plane").gameObject;
		scale1 = minimapp.transform.localScale;
		var minimapSide = gameObject.transform.Find("Plane2").gameObject;
		scale2 = minimapSide.transform.localScale;
		//float x = GameObject.Find("MinimapPlane").transform.localScale.x;
		//float z = GameObject.Find("MinimapPlane").transform.localScale.z;
		//minimapp.transform.localScale = new Vector3(scale1.x * z * 5.0f, scale1.y, scale1.z * x * 5.0f);

		//x = GameObject.Find("MinimapSidePlane").transform.localScale.x;
		//z = GameObject.Find("MinimapSidePlane").transform.localScale.z;
		//minimapSide.transform.localScale = new Vector3(scale1.x * z * 5.0f, scale1.y, scale1.z * x * 5.0f);


		float ratio = GameObject.Find("MinimapPlane").transform.localScale.x / GameObject.Find("MinimapPlane").transform.localScale.z;
		if (ratio < 1) minimapp.transform.localScale = new Vector3(scale1.x, scale1.y, scale1.z * ratio);
		else minimapp.transform.localScale = new Vector3(scale1.x / ratio, scale1.y, scale1.z);
		//Debug.Log(ratio);
		float ratio2 = GameObject.Find("MinimapSidePlane").transform.localScale.x / GameObject.Find("MinimapSidePlane").transform.localScale.z;
		if (ratio2 < 1) minimapSide.transform.localScale = new Vector3(scale2.x, scale2.y, scale2.z * ratio2);
		else minimapSide.transform.localScale = new Vector3(scale2.x / ratio2, scale2.y, scale2.z);
		//Debug.Log(ratio2);

		if (config.minimapOnPanel)
		{
			minimapp.transform.localPosition = new Vector3(-0.0581f, 0.1761f, 0.2448f);
			minimapSide.transform.localPosition = new Vector3(0.0648f, 0.1761f, 0.2448f);
			minimapp.transform.localEulerAngles = new Vector3(59.356f, 180.0f, 0.0f);
			minimapSide.transform.localEulerAngles = new Vector3(59.356f, 180.0f, 0.0f);
		}
		else
		{
			minimapp.transform.localPosition = new Vector3(0.0006f, 0.2793f, 0.215f);
			minimapp.transform.localEulerAngles = new Vector3(118.342f, 178.058f, -0.8109741f);
			minimapp.transform.localScale /= 1.2f;
			minimapSide.transform.localPosition = new Vector3(0.08f, 0.2625f, 0.1992f);
			minimapSide.transform.localEulerAngles = new Vector3(121.245f, 196.401f, -7.395996f);
			minimapSide.transform.localScale /= 1.2f;
		}


		/*start ROS*/
		if (config.useROS)
		{
			Debug.Log("Connecting to ROS master at " + config.remoteIP);
			rosClient = new ROSClient(config.remoteIP);
			if (config.enableMyFilter)
				rosClient.enableFilter(config.bufferSize);

			Debug.Log("Connected");
			rosClient.initSubscriber(config.subscribingTopic);
			rosClient.initPublisher(config.publishingTopic);
			buttons = new int[11];
			axes = new float[8];
		}
		/*read offset*/
		string offsetName;
		if (isServer) offsetName = "StartPos1";
		else offsetName = "StartPos2";
		positionOffset = GameObject.Find(offsetName).GetComponent<Transform>().position;
		orientationOffset = GameObject.Find(offsetName).GetComponent<Transform>().eulerAngles;
	}

	void MinimapControl()
	{
		Vector3 centerPos = gameObject.GetComponent<Transform>().position;
		Vector3 ballPos = new Vector3();
		if (GameObject.Find("Ball(Clone)") != null) ballPos = GameObject.Find("Ball(Clone)").GetComponent<Transform>().position;
		if (isServer)
		{
			GameObject.Find("MyDrone").GetComponent<Transform>().position = new Vector3(1.0f, centerPos.y / 3, centerPos.x / 3 - 100.0f);
			GameObject.Find("BallIndicator").GetComponent<Transform>().position = new Vector3(0.0f, ballPos.y / 3, ballPos.x / 3 - 100.0f);
			GameObject.Find("DroneSideView").GetComponent<Transform>().position = new Vector3(1.0f, centerPos.z / 3, centerPos.x / 3 - 200.0f);
			GameObject.Find("BallSideView").GetComponent<Transform>().position = new Vector3(0.0f, ballPos.z / 3, ballPos.x / 3 - 200.0f);

		}
		else
		{
			GameObject.Find("MyDrone").GetComponent<Transform>().position = new Vector3(1.0f, centerPos.y / 3, -centerPos.x / 3 - 100.0f);
			GameObject.Find("BallIndicator").GetComponent<Transform>().position = new Vector3(0.0f, ballPos.y / 3, -ballPos.x / 3 - 100.0f);
			GameObject.Find("DroneSideView").GetComponent<Transform>().position = new Vector3(1.0f, -centerPos.z / 3, -centerPos.x / 3 - 200.0f);
			GameObject.Find("BallSideView").GetComponent<Transform>().position = new Vector3(0.0f, -ballPos.z / 3, -ballPos.x / 3 - 200.0f);
		}

	

	}

	void ROSControl()
	{
		if (config.useROS)
		{
			OVRInput.Update(); // Has to be called at the beginning to interact with OVRInput.

			// Retrieve buttons status
			buttons[0] = OVRInput.Get(OVRInput.Button.One) ? 1 : 0;
			buttons[1] = OVRInput.Get(OVRInput.Button.Two) ? 1 : 0;
			buttons[2] = OVRInput.Get(OVRInput.Button.Three) ? 1 : 0;
			buttons[3] = OVRInput.Get(OVRInput.Button.Four) ? 1 : 0;
			buttons[9] = OVRInput.Get(OVRInput.Button.PrimaryThumbstick) ? 1 : 0;
			buttons[10] = OVRInput.Get(OVRInput.Button.SecondaryThumbstick) ? 1 : 0;

			// Retrieve axes status
			Vector2 axisStickLeft = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
			axes[0] = axisStickLeft.x;
			axes[1] = axisStickLeft.y;

			axes[2] = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger);

			Vector2 axisStickRight = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
			axes[3] = axisStickRight.x;
			axes[4] = axisStickRight.y;

			axes[5] = OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger);

			// Send buttons and axes to ROS.	
			rosClient.publish(buttons, 11, axes, 8);

			// Read position from ROS.	
			if (rosClient.isPoseAvailable())
			{
				ROS.Pose pose = rosClient.getPose();
				Debug.Log(JsonUtility.ToJson(pose));
				gameObject.transform.position = pose.position.toUnityCoordSys(config.positionScale);
				if (isServer) gameObject.transform.position += positionOffset;
				else
				{
					Vector3 pos = gameObject.transform.position;
					gameObject.transform.position = new Vector3(-pos.x, pos.y, -pos.z) + positionOffset;
				}
				if (config.enableRotation)
				{
					gameObject.transform.eulerAngles = pose.orientation.toUnityCoordSys();
					Debug.Log("Euler: " + gameObject.transform.eulerAngles.ToString());
					gameObject.transform.eulerAngles += orientationOffset;
				}
			}
		}
	}

	void CameraControl()
	{
		var mainCam = gameObject.transform.Find("Camera").gameObject;
		Vector3 pos = new Vector3(mainCam.transform.position.x, mainCam.transform.position.y, mainCam.transform.position.z);
		Vector3 rot = new Vector3(mainCam.transform.eulerAngles.x, mainCam.transform.eulerAngles.y, mainCam.transform.eulerAngles.z);
		GameObject.Find("GoProPrefab").GetComponent<Transform>().position = pos;
		GameObject.Find("GoProPrefab").GetComponent<Transform>().eulerAngles = rot;
	}
	
	// Update is called once per frame
	//if not myself/ doesn't belong to me, skip
	void Update () {
		if (!isLocalPlayer) return;
		if (ready)
		{
			CmdSpawnBall();
			ready = false;
		}
		transform.Translate (Input.GetAxis ("Horizontal") * config.speed * Time.deltaTime, Input.GetAxis ("Vertical") * config.speed * Time.deltaTime, 0f);
		MinimapControl();
		ROSControl();
		
		CameraControl();
		if (config.dynamicAdjust)
		{
			var minimapp = gameObject.transform.Find("Plane").gameObject;
			//scale1 = minimapp.transform.localScale;
			var minimapSide = gameObject.transform.Find("Plane2").gameObject;
			//scale2 = minimapSide.transform.localScale;
			//float x = GameObject.Find("MinimapPlane").transform.localScale.x;
			//float z = GameObject.Find("MinimapPlane").transform.localScale.z;
			//minimapp.transform.localScale = new Vector3(scale1.x * z * 5.0f, scale1.y, scale1.z * x * 5.0f);

			//x = GameObject.Find("MinimapSidePlane").transform.localScale.x;
			//z = GameObject.Find("MinimapSidePlane").transform.localScale.z;
			//minimapSide.transform.localScale = new Vector3(scale1.x * z * 5.0f, scale1.y, scale1.z * x * 5.0f);


			float ratio = GameObject.Find("MinimapPlane").transform.localScale.x / GameObject.Find("MinimapPlane").transform.localScale.z;
			if (ratio < 1) minimapp.transform.localScale = new Vector3(scale1.x, scale1.y, scale1.z * ratio);
			else minimapp.transform.localScale = new Vector3(scale1.x / ratio, scale1.y, scale1.z);
			//Debug.Log(ratio);
			float ratio2 = GameObject.Find("MinimapSidePlane").transform.localScale.x / GameObject.Find("MinimapSidePlane").transform.localScale.z;
			if (ratio2 < 1) minimapSide.transform.localScale = new Vector3(scale2.x, scale2.y, scale2.z * ratio2);
			else minimapSide.transform.localScale = new Vector3(scale2.x / ratio2, scale2.y, scale2.z);

			if (config.minimapOnPanel)
			{
				minimapp.transform.localPosition = new Vector3(-0.0581f, 0.1761f, 0.2448f);
				minimapSide.transform.localPosition = new Vector3(0.0648f, 0.1761f, 0.2448f);
				minimapp.transform.localEulerAngles = new Vector3(59.356f, 180.0f, 0.0f);
				minimapSide.transform.localEulerAngles = new Vector3(59.356f, 180.0f, 0.0f);
			}
			else
			{
				minimapp.transform.localPosition = new Vector3(0.0006f, 0.2793f, 0.215f);
				minimapp.transform.localEulerAngles = new Vector3(118.342f, 178.058f, -0.8109741f);
				minimapp.transform.localScale /= 1.2f;
				minimapSide.transform.localPosition = new Vector3(0.08f, 0.2625f, 0.1992f);
				minimapSide.transform.localEulerAngles = new Vector3(121.245f, 196.401f, -7.395996f);
				minimapSide.transform.localScale /= 1.2f;
			}
		}
		gameObject.GetComponent<BoxCollider>().center = new Vector3(0.0f, 0.0f, 3.0f * colliderDist);
	}
}

