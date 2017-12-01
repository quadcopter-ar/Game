using System;

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
	
	public UnityEngine.Vector3 toYUp()
        {
	    // convert to Y-up coord. sys.
            return new UnityEngine.Vector3(x, z, -y);
        }

    }
    [Serializable]
    public class Orientation
    {
        public float x;
        public float y;
        public float z;
        public float w;
	
	// Returns the euler angles and with Y-up coordinate system in mind.
	public UnityEngine.Vector3 toYUp()
        {
            UnityEngine.Vector3 euler = (new UnityEngine.Quaternion(x, y, z, w)).eulerAngles;
            return new UnityEngine.Vector3(euler.x, euler.z, -euler.y); // convert to y-up.
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
