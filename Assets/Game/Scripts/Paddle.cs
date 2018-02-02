using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Paddle : NetworkBehaviour {

	//public bool isPaddle1;
	public float speed = 5f;
	public GameObject ballprefab;
	[SyncVar]
	public bool ready = false;

	[Command]
	void CmdSpawnBall() {
		Quaternion rot = new Quaternion();
		rot.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
		Vector3 ballPos = (GameObject.Find("StartPos1").GetComponent<Transform>().position + GameObject.Find("StartPos2").GetComponent<Transform>().position) / 2.0f;
		var ball = (GameObject)Instantiate(ballprefab, ballPos, rot);
		float sx = 1.0f;
		float sy = 0.0f;
		//float sz = 0.0f;
		//float sy = Random.Range (0, 2) == 0 ? -1 : 1;
		float sz = -3.0f;
		ball.GetComponent<Rigidbody> ().velocity = new Vector3 (speed * sx, speed * sy, speed * sz);
		NetworkServer.Spawn (ball);
	}

	public override void OnStartClient() {
		//if (!isServer)
		//{
			Debug.Log("TEst");
			ready = true;
		//}
		var minimapp = gameObject.transform.Find("Plane").gameObject;
		Vector3 scale1 = minimapp.transform.localScale;
		float ratio = GameObject.Find("MinimapPlane").transform.localScale.x / GameObject.Find("MinimapPlane").transform.localScale.z;
		if (ratio < 1) minimapp.transform.localScale = new Vector3(scale1.x , scale1.y, scale1.z * ratio);
		else minimapp.transform.localScale = new Vector3(scale1.x / ratio, scale1.y, scale1.z);
		Debug.Log(ratio);


	}

	// Use this for initialization
	void Start ()
	{

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
		Vector3 centerPos = gameObject.GetComponent<Transform>().position;
		Vector3 ballPos = new Vector3();
		if (GameObject.Find("Ball(Clone)") != null) ballPos = GameObject.Find("Ball(Clone)").GetComponent<Transform>().position;
		if (isServer)
		{
			GameObject.Find("MyDrone").GetComponent<Transform>().position = new Vector3(1.0f, centerPos.y / 3, centerPos.z / 3 - 100.0f);
			GameObject.Find("BallIndicator").GetComponent<Transform>().position = new Vector3(0.0f, ballPos.y / 3, ballPos.z / 3 - 100.0f);
		}
		else
		{
			GameObject.Find("MyDrone").GetComponent<Transform>().position = new Vector3(1.0f, centerPos.y / 3, -centerPos.z / 3 - 100.0f);
			GameObject.Find("BallIndicator").GetComponent<Transform>().position = new Vector3(0.0f, ballPos.y / 3, -ballPos.z / 3 - 100.0f);
		}
		//if(isPaddle1){


		transform.Translate (Input.GetAxis ("Horizontal") * speed * Time.deltaTime, Input.GetAxis ("Vertical") * speed * Time.deltaTime, 0f);

		

		/*Vector3 mypos = GameObject.Find("ROS").GetComponent<OculusRiftTouchController>().pos;
		Vector3 myeul = GameObject.Find("ROS").GetComponent<OculusRiftTouchController>().eul;
		gameObject.transform.position = new Vector3(mypos.x, mypos.y, mypos.z);
		gameObject.transform.eulerAngles = new Vector3(myeul.x, myeul.y, myeul.z);

		if (!isServer)
		{
			gameObject.transform.position += GameObject.Find("StartPos2").GetComponent<Transform>().position;
			gameObject.transform.eulerAngles += GameObject.Find("StartPos2").GetComponent<Transform>().eulerAngles;
		}
		else
		{

			gameObject.transform.position += GameObject.Find("StartPos1").GetComponent<Transform>().position;
			gameObject.transform.eulerAngles += GameObject.Find("StartPos1").GetComponent<Transform>().eulerAngles;

		}*/
		

		var mainCam = gameObject.transform.Find("Camera").gameObject;
		Vector3 pos = new Vector3(mainCam.transform.position.x, mainCam.transform.position.y, mainCam.transform.position.z);
		Vector3 rot = new Vector3(mainCam.transform.eulerAngles.x, mainCam.transform.eulerAngles.y, mainCam.transform.eulerAngles.z);
		GameObject.Find("GoProPrefab").GetComponent<Transform>().position = pos;
		GameObject.Find("GoProPrefab").GetComponent<Transform>().eulerAngles = rot;
	}
}
