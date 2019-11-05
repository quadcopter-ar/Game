using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System;

public class PlayerController : NetworkBehaviour
{
	public GameObject bulletPrefab;
	public Transform bulletSpawn;

	private SceneConfiguration config;
	private AudioSource shotSound;
	ROSClient rosClient;

	private TcpClient socketConnection;
	private Thread clientReceiveThread;
	private int frameCount = 0;

	// All following variables are using Unity coord conventions
	private Vector3 current_position = new Vector3(0, 0, 0);
	private Vector3 current_orientation = new Vector3(0, 0, 0);
	private Vector3 target_displacement = new Vector3(0, 0, 0);
	private Vector3 target_position = new Vector3(0, 0, 0);
	private Vector3 target_orientation = new Vector3(0, 0, 0);

	private float[] controller_axes = new float[4];
	private Vector3 controller_displacement_threshold = new Vector3((float).5, (float).5, (float).5); // Action below this value will not be considered
	private Vector3 controller_displacement = new Vector3(0, 0, 0);
	private float controller_rotation = 0;
	
	private Vector3 moving_speed = new Vector3((float).7, (float).5, (float).7);
	private float rotation_speed = (float)1;

	void Start()
	{
		/*
		if (isServer)
			pose = GameObject.Find("startpoint1").GetComponent<Transform>().position;
		else
			pose = GameObject.Find("startpoint2").GetComponent<Transform>().position;
			*/

		shotSound = GetComponent<AudioSource>();
		config = GameObject.Find("Config").GetComponent<SceneConfiguration>();

		if (config.useROS)
		{
			ConnectToTcpServer();

			//buttons = new int[4];
			//axes = new float[4];

			Debug.Log("Connecting to ROS master at " + config.remoteIP);
			rosClient = new ROSClient(config.remoteIP);
			if (config.enableMyFilter)
				rosClient.enableFilter(config.bufferSize);

			//Debug.Log("Connected");
			rosClient.initSubscriber(config.subscribingTopic);
			rosClient.initPublisher(config.publishingTopic);

		}


	}

	void CameraControl()
	{
		var mainCam = gameObject.transform.Find("Camera").gameObject;
		Vector3 cam_pos = new Vector3(mainCam.transform.position.x, mainCam.transform.position.y, mainCam.transform.position.z);
		Vector3 cam_rot = new Vector3(mainCam.transform.eulerAngles.x, mainCam.transform.eulerAngles.y, mainCam.transform.eulerAngles.z);
		GameObject.Find("GoProPrefab").GetComponent<Transform>().position = cam_pos;
		GameObject.Find("GoProPrefab").GetComponent<Transform>().eulerAngles = cam_rot;
	}

	void UpdateControllerInfo()
	{
		/*
		 * Controller
		 * LThumbX: right - Unity: X
		 * LThumbY: forward - Unity Z
		 * RThumbX: rotate 
		 * RThumbY: up - Unity Y
		 * 
		*/
		// Retrieve axes status  (LThumbstick Correct, RTsX is trigger, RTsY is RTsX) see Input Manager by edit->project settings->
		controller_axes[0] = Input.GetAxis("Oculus_GearVR_LThumbstickX");
		Debug.Log("Oculus_GearVR_LThumbstickX:" + controller_axes[0]);
		controller_axes[1] = Input.GetAxis("Oculus_GearVR_LThumbstickY");
		controller_axes[2] = Input.GetAxis("Oculus_GearVR_DpadX"); // fixed, invert unchecked
		controller_axes[3] = Input.GetAxis("Oculus_GearVR_RThumbstickY"); // invert checked

		// string[] axes_str = { controller_axes[0].ToString(), controller_axes[1].ToString(), controller_axes[2].ToString(), controller_axes[3].ToString() };
		// Debug.Log(string.Join(",", axes_str));

		controller_displacement.x = controller_axes[0];
		controller_displacement.y = controller_axes[2];
		controller_displacement.z = controller_axes[1];

		controller_rotation = controller_axes[3];

		// Keyboard Control
		// Direction: WASD
		// Rotation: QE
		// Up: LeftShift
		// Down: LeftCtrl
		if (Input.GetKey(KeyCode.W))
		{
			controller_displacement.z = 1;
		}
		if (Input.GetKey(KeyCode.S))
		{
			controller_displacement.z = -1;
		}
		if (Input.GetKey(KeyCode.A))
		{
			controller_displacement.x = -1;
		}
		if (Input.GetKey(KeyCode.D))
		{
			controller_displacement.x = 1;
		}
		if (Input.GetKey(KeyCode.E))
		{
			controller_rotation = 1;
		}
		if (Input.GetKey(KeyCode.Q))
		{
			controller_rotation = -1;
		}
		if (Input.GetKey(KeyCode.LeftShift))
		{
			controller_displacement.y = 1;
		}
		if (Input.GetKey(KeyCode.LeftControl))
		{
			controller_displacement.y = -1;
		}
		if (Input.GetKeyDown(KeyCode.Space))
		{
			CmdFire();
		}


		target_displacement.y = controller_displacement.y * moving_speed.y;

		// Unity: Left handed orientation system
		float theta = -transform.eulerAngles.y / 180 * Mathf.PI;

		float x = controller_displacement.x * moving_speed.x;
		float z = controller_displacement.z * moving_speed.z;

		target_displacement.x = x * Mathf.Cos(theta) - z * Mathf.Sin(theta);
		target_displacement.z = x * Mathf.Sin(theta) + z * Mathf.Cos(theta);
		target_orientation.y = current_orientation.y + controller_rotation * rotation_speed;
	}

	void Update()
	{
    	if(!isLocalPlayer) return;

		

		if (config.useROS)
		{
			if ((Input.GetKeyDown(KeyCode.JoystickButton0)))
			{
				Debug.Log("fire");
				CmdFire();
			}

			transform.position = current_position;
			transform.eulerAngles = current_orientation;

			UpdateControllerInfo();
			FinalizeTarget();

			SendFinalTarget();
		}

		else //move use keyboard, play on unity
		{
			moving_speed = new Vector3((float).07, (float).05, (float).07);

			current_position = transform.position;
			current_orientation = transform.eulerAngles;

			// This will calculate target displacement and orientation
			UpdateControllerInfo();

			// This will add displacement to current position so that we have the final target
			FinalizeTarget();

			transform.position = target_position;
			transform.eulerAngles = target_orientation;

			if ((Input.GetKeyDown(KeyCode.JoystickButton0)))
			{
				Debug.Log("fire");
				CmdFire();
			}
		}


		CameraControl();
	}



    [Command]
    void CmdFire()
	{
		shotSound.Play();
		// Create the Bullet from the Bullet Prefab
		var bullet = (GameObject)Instantiate (bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);

		// Add velocity to the bullet
		bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 10;

		// Spawn the bullet on the Clients
		NetworkServer.Spawn(bullet);

		// Destroy the bullet after 2 seconds
		Destroy(bullet, 2.0f);
	}


	private void ConnectToTcpServer()
	{
		try
		{
			clientReceiveThread = new Thread(new ThreadStart(ListenForData));
			clientReceiveThread.IsBackground = true;
			clientReceiveThread.Start();
		}
		catch (Exception e)
		{
			Debug.Log("On client connect exception " + e);
		}
	}

	private void ListenForData()
	{
		try
		{
			socketConnection = new TcpClient(config.remoteIP, 13579);
			Byte[] bytes = new Byte[1024];
			if (socketConnection.Connected)
				Debug.Log("TCP Server connected.");
			while (true)
			{
				// Get a stream object for reading 				
				using (NetworkStream stream = socketConnection.GetStream())
				{
					int length;
					// Read incomming stream into byte arrary. 					
					while ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
					{
						var incommingData = new byte[length];
						Array.Copy(bytes, 0, incommingData, 0, length);
						// Convert byte array to string message. 						
						string serverMessage = Encoding.UTF8.GetString(incommingData);
						string[] pose_str = serverMessage.Split(',');
						float x = float.Parse(pose_str[0]);
						float y = float.Parse(pose_str[1]);
						float z = float.Parse(pose_str[2]);
						Vector3 ros_current_position = new Vector3(x, y, z);

						// Coord conversion from ROS to Unity should have been done here.
						// SOLO & SIMULATION
						current_position.x = ros_current_position.x;
						current_position.y = ros_current_position.z;
						current_position.z = ros_current_position.y;

						// ORB SLAM
						//current_position.x = 10 * ros_current_position.x;
						//current_position.y = 10 * -ros_current_position.y;
						//current_position.z = 10 * ros_current_position.z;

						float w = float.Parse(pose_str[3]);
						x = float.Parse(pose_str[4]);
						y = float.Parse(pose_str[5]);
						z = float.Parse(pose_str[6]);
						
						Vector3 ros_current_euler_orientation = (new Quaternion(x, y, z, w)).eulerAngles;
						// From ROS_PoseStamped.cs euler.y, -euler.z  , -euler.x
						// SOLO & SIMULATION
						current_orientation.x = ros_current_euler_orientation.y;
						current_orientation.y = -ros_current_euler_orientation.z;
						current_orientation.z = -ros_current_euler_orientation.x;

						// ORB SLAM
						//current_orientation.x = -ros_current_euler_orientation.x;
						//current_orientation.y = ros_current_euler_orientation.y;
						//current_orientation.z = -ros_current_euler_orientation.z;

						// OPTITRACK
						current_orientation.x = ros_current_euler_orientation.y;
						current_orientation.y = -ros_current_euler_orientation.z;
						current_orientation.z = -ros_current_euler_orientation.x;

						//Debug.Log("server message received as: " + pose_str[0] + ',' + pose_str[1] + ',' + pose_str[2]);
						//Debug.Log("server message received as: " + pose_str[3] + ',' + pose_str[4] + ',' + pose_str[5] + ',' + pose_str[6]);
					}
				}
			}
		}
		catch (SocketException socketException)
		{
			Debug.Log("Socket exception: " + socketException);
		}
	}

	private void SendFinalTarget()
	{
		if (socketConnection == null)
		{
			return;
		}
		try
		{
			// Get a stream object for writing. 			
			NetworkStream stream = socketConnection.GetStream();
			if (stream.CanWrite)
			{
				// Coord conversion from Unity to ROS should have been done here.
				Vector3 ros_final_target_position = new Vector3();

				ros_final_target_position.x = target_position.x;
				ros_final_target_position.y = target_position.z;
				ros_final_target_position.z = target_position.y;

				Vector3 ros_final_target_euler_orientation = new Vector3();

				ros_final_target_euler_orientation.x = -target_orientation.z;
				ros_final_target_euler_orientation.y = target_orientation.x;
				ros_final_target_euler_orientation.z = -target_orientation.y;

				Quaternion ros_final_target_quaternion_orientation = Quaternion.Euler(ros_final_target_euler_orientation);

				string[] msg_str_array = new string[7];
				msg_str_array[0] = ros_final_target_position.x.ToString("f5");
				msg_str_array[1] = ros_final_target_position.y.ToString("f5");
				msg_str_array[2] = ros_final_target_position.z.ToString("f5");
				msg_str_array[3] = ros_final_target_quaternion_orientation.w.ToString("f5");
				msg_str_array[4] = ros_final_target_quaternion_orientation.x.ToString("f5");
				msg_str_array[5] = ros_final_target_quaternion_orientation.y.ToString("f5");
				msg_str_array[6] = ros_final_target_quaternion_orientation.z.ToString("f5");

				string msg_str = String.Join(",", msg_str_array) + ",";
				//Debug.Log(final_target_orientation);
				// Convert string message to byte array.                 
				byte[] clientMessageAsByteArray = Encoding.UTF8.GetBytes(msg_str);
				// Write byte array to socketConnection stream.                 
				stream.Write(clientMessageAsByteArray, 0, clientMessageAsByteArray.Length);
				Debug.Log(msg_str);
			}
		}
		catch (SocketException socketException)
		{
			Debug.Log("Socket exception: " + socketException);
		}
	}

	private void FinalizeTarget()
	{
		// TODO: COLLISION DETECTION HERE

		if(VerifyTarget())
		{
			target_position.x = current_position.x + target_displacement.x;
			target_position.y = current_position.y + target_displacement.y;
			target_position.z = current_position.z + target_displacement.z;
		}

		// COLLISION DETECTION END

		if (Mathf.Abs(controller_displacement.x) < controller_displacement_threshold.x && Mathf.Abs(controller_displacement.z) < controller_displacement_threshold.z)
		{
			// Idle Drift Prevention
			// If don't have last target, stay at current position; Else, stay at last target
			if (target_position.x == 0)
				target_position.x = current_position.x;
			if (target_position.z == 0)
				target_position.z = current_position.z;
		}

		if (Mathf.Abs(controller_displacement.y) < controller_displacement_threshold.y)
			if (target_position.y == 0)
				target_position.y = current_position.y;
	}

	private bool VerifyTarget()
	{
		Vector3 checkCollisionPoint = new Vector3(0.0f, 0.0f, 0.0f);
		float target_displacement_length = target_displacement.magnitude;
		// Debug.Log("length is " + target_displacement_length);
		checkCollisionPoint.x = (float)((target_displacement.x / target_displacement_length) + target_displacement.x) + current_position.x;
		checkCollisionPoint.y = (float)((target_displacement.y / target_displacement_length) + target_displacement.y) + current_position.y;
		checkCollisionPoint.z = (float)((target_displacement.z / target_displacement_length) + target_displacement.z) + current_position.z;

		return (CheckPointInsideBox(checkCollisionPoint));
	}

	private bool CheckPointInsideBox(Vector3 point)
	{
		float xMin = GameObject.Find("Wall5").GetComponent<Transform>().position.x;
		float xMax = GameObject.Find("Wall6").GetComponent<Transform>().position.x;


		float yMin = GameObject.Find("Wall1").GetComponent<Transform>().position.y;
		float yMax = GameObject.Find("Wall2").GetComponent<Transform>().position.y;

		float zMin = GameObject.Find("Wall4").GetComponent<Transform>().position.z;
		float zMax = GameObject.Find("Wall3").GetComponent<Transform>().position.z;

		if (point.x > xMin && point.x < xMax && point.y > yMin && point.y < yMax && point.z > zMin && point.z < zMax)
		{
			return true;
		}

		return false;
	}

	void OnApplicationQuit()
	{
		if(socketConnection.Connected)
			socketConnection.Close();
	}
}
