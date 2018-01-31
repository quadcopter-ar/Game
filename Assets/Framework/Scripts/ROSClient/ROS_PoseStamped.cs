using System;
using UnityEngine;

// ROS coordinate system is Z-up.
namespace ROS
{
    [Serializable]
    public class Time
    {
        public int nsec;
        public int sec;
    }
    [Serializable]
    public class Header
    {
        public int seq;
        public Time time;
        public string frame_id;
    }
    [Serializable]
    public class Position
    {
        public float x;
        public float y;
        public float z;

	public UnityEngine.Vector3 getROSAdjustedCoord()
        {
            // ROS change
	    // (yNew xNew)^T = ROT(-90) * (yRos, xRos)^T = (-yRos, xRos)
            float yNew = x;
            float xNew = -y;
            return new UnityEngine.Vector3(xNew, yNew, z);
        }
	
	public UnityEngine.Vector3 toYUp(UnityEngine.Vector3 pos)
        {
            // convert to Y-up coord. sys.
            return new UnityEngine.Vector3(pos.x, pos.z, pos.y);
        }

        public UnityEngine.Vector3 toUnityCoordSys(UnityEngine.Vector3 scale)
        {
            return UnityEngine.Vector3.Scale(toYUp(getROSAdjustedCoord()), scale);
        }

    }
    [Serializable]
    public class Orientation
    {
        public float x;
        public float y;
        public float z;
        public float w;
	
	    // Returns the euler angles for Unity.
	    public UnityEngine.Vector3 toUnityCoordSys()
        {
            // From ROS, the coordinate is in Right-hand-rule and Z-up.
            UnityEngine.Vector3 euler = (new UnityEngine.Quaternion(x, y, z, w)).eulerAngles;
	   
	    // Change of orientation in ROS.	
            //UnityEngine.Vector3 euler = Quaternion.Euler(0, 0, -90) * (new UnityEngine.Quaternion(x, y, z, w)).eulerAngles;

            // Debug.Log(euler);
            // Ref: https://stackoverflow.com/questions/31191752/right-handed-euler-angles-xyz-to-left-handed-euler-angles-xyz 

            // Unity uses Left-hand-rule and Y-up.
            
            // RHR to LHR
            // z' = -z
            // x' = -x
            // y' = y
            
            // Z-up to Y-up
            // P" = ROT_x(-90) * P'
            // P" = [1 0 0; 0 0 1; 0 -1 0] * (x', y', z')
            // P" = (x', z', -y') = (-x, -z, -y)

            // Furthermore, the original angle around the up-axis is -180 in stationary pose.
            // so (-x, -z - 180, -y)
            // return new UnityEngine.Vector3(-euler.x, -euler.z - 180 , -euler.y);
            return new UnityEngine.Vector3(-euler.x, -euler.z  , -euler.y);
        }
    }

    [Serializable]
    public class Pose
    {
        public Position position;
        public Orientation orientation;
    }

    [Serializable]
    public class PoseStamped
    {
        public Header header;
        public Pose pose;
    }
}
