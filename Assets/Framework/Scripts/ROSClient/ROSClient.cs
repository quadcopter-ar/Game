// For type IntPtr.
using System;
// For DllImport.
using System.Runtime.InteropServices;

using UnityEngine;

public class ROSClient
{
    [DllImport("ROSClient", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr _ROSClient_init(IntPtr ip);

    [DllImport("ROSClient", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    public static extern void _ROSClient_initPublisher(IntPtr client, IntPtr topic);

    [DllImport("ROSClient", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    public static extern void _ROSClient_publish(IntPtr client, int[] buttons, int buttons_length, float[] axes, int axes_length);

    [DllImport("ROSClient", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    public static extern void _ROSClient_initSubscriber(IntPtr client, IntPtr topic, bool sim);

    [DllImport("ROSClient", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    public static extern bool _ROSClient_isMsgAvailable(IntPtr client);

    [DllImport("ROSClient", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr _ROSClient_getMsg(IntPtr client);

    [DllImport("ROSClient", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    public static extern void _ROSClient_clearMsg(IntPtr client);

    [DllImport("ROSClient", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    public static extern int _ROSClient_initLowPassFilter(IntPtr client, int nTaps, double Fs, double Fx);

    [DllImport("ROSClient", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    public static extern int _ROSClient_initMyFilter(IntPtr client, int bufferSize);

    IntPtr p_mROSClient;

    public ROSClient(string remoteIP)
    {
        p_mROSClient = _ROSClient_init(Marshal.StringToHGlobalAnsi(remoteIP));
    }

    public void enableFilter(int bufferSize)
    {
        _ROSClient_initMyFilter(p_mROSClient, bufferSize);
    }
	
    public void initPublisher(string publishingTopic)
    {
        _ROSClient_initPublisher(p_mROSClient, Marshal.StringToHGlobalAnsi(publishingTopic));
    }

    public void publish(int[] buttons, int buttons_length, float[] axes, int axes_length)
    {
	_ROSClient_publish(p_mROSClient, buttons, buttons_length, axes, axes_length); 
    }

    public void initSubscriber(string subscribingTopic)
    {
        _ROSClient_initSubscriber(p_mROSClient, Marshal.StringToHGlobalAnsi(subscribingTopic), false);
    }

    private bool isMsgAvailable()
    {
        return _ROSClient_isMsgAvailable(p_mROSClient);
    }

    private string getMsg()
    {
        return Marshal.PtrToStringAnsi(_ROSClient_getMsg(p_mROSClient));
    }

    private void clearMsg()
    {
        _ROSClient_clearMsg(p_mROSClient);
    }

    public bool isPoseAvailable()
    {
        return isMsgAvailable();
    }

    public ROS.Pose getPose()
    {
        ROS.Pose pose = new ROS.Pose();
        string msg = getMsg();	
        JsonUtility.FromJsonOverwrite(msg, pose);
        clearMsg();

        // Debug.Log(JsonUtility.ToJson(pose));
        return pose;
    }

}