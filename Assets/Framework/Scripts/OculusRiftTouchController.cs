using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// For type IntPtr.
using System;
// For DllImport.
using System.Runtime.InteropServices;

using System.Text;

public class OculusRiftTouchController : MonoBehaviour {
    ROSClient rosClient;
	int[] buttons;
	float[] axes;
	string msg;

        //public GameObject gameObject = ;
        public Vector3 positionScale;
        public string remoteIP = "192.168.1.2";
        public string publishingTopic = "joy";
        public string subscribingTopic = "fiducial_pose_corrected";
        public bool isSimulation = false; // ROS simulation.
        public bool enableRotation = false;
	public bool enableLowPassFilter= false;
	public int nTaps = 51;
	public double Fs = 44.1, Fx = 2.0;
	public bool enableMyFilter = false;
	public int bufferSize = 5;

	// Use this for initialization
	void Start () {
		Debug.Log("Connecting to ROS master at " + remoteIP);
		rosClient = new ROSClient(remoteIP);
		if (enableMyFilter)
		    rosClient.enableFilter(bufferSize); 
		
		Debug.Log("Connected");
		rosClient.initSubscriber(subscribingTopic);
        	rosClient.initPublisher(publishingTopic);
		buttons = new int[11];
		axes = new float[8];
	}

	// Update is called once per frame
	void Update () {
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
		    gameObject.transform.position = pose.position.toUnityCoordSys(positionScale);
		    if (enableRotation)
		    {
			gameObject.transform.eulerAngles = pose.orientation.toUnityCoordSys();
			Debug.Log("Euler: " + gameObject.transform.eulerAngles.ToString());
		    }
		}
	}
}
