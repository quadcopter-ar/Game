using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// For type IntPtr.
using System;
// For DllImport.
using System.Runtime.InteropServices;

using System.Text;


public class OculusRiftTouchController : MonoBehaviour {
    [DllImport("ROSClient.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr _ROSClient_init(IntPtr ip);

    [DllImport("ROSClient.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    public static extern void _ROSClient_initPublisher(IntPtr client, IntPtr topic);

    [DllImport("ROSClient.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    public static extern void _ROSClient_publish(IntPtr client, int[] buttons, int buttons_length, float[] axes, int axes_length);

    [DllImport("ROSClient.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    public static extern void _ROSClient_initSubscriber(IntPtr client, IntPtr topic);

    [DllImport("ROSClient.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    public static extern bool _ROSClient_isMsgAvailable(IntPtr client);

    [DllImport("ROSClient.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr _ROSClient_getMsg(IntPtr client);

    [DllImport("ROSClient.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    public static extern void _ROSClient_clearMsg(IntPtr client);

    IntPtr ROSClient;
	int[] buttons;
	float[] axes;
	string msg;

        public GameObject gameObject;
        public float positionScale = 1.0f;
        public string remoteIP = "192.168.1.175";
        public string publishingTopic = "joy";
        public string subscribingTopic = "mavros/local_position/pose"; 

	// Use this for initialization
	void Start () {
		Debug.Log("Connecting to ROS master at " + remoteIP);
		ROSClient = _ROSClient_init(Marshal.StringToHGlobalAnsi(remoteIP));
		Debug.Log("Connected");
		_ROSClient_initPublisher(ROSClient, Marshal.StringToHGlobalAnsi(publishingTopic));
		_ROSClient_initSubscriber(ROSClient, Marshal.StringToHGlobalAnsi(subscribingTopic));
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
		_ROSClient_publish(ROSClient, buttons, 11, axes, 8);
        
		// Read position from ROS.	
		if (_ROSClient_isMsgAvailable(ROSClient))
		{
		    msg = Marshal.PtrToStringAnsi(_ROSClient_getMsg(ROSClient));
		    ROS.PoseStamped pose = new ROS.PoseStamped();
		    JsonUtility.FromJsonOverwrite(msg, pose);
		    _ROSClient_clearMsg(ROSClient);
		    Debug.Log(JsonUtility.ToJson(pose));

		    gameObject.transform.position = pose.pose.position.toYUp() * positionScale;
		    gameObject.transform.eulerAngles = pose.pose.orientation.toYUp();

		}
	}
}
