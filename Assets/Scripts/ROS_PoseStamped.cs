using System;

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
        public double x;
        public double y;
        public double z;
    }
    [Serializable]
    public class Orientation
    {
        public double x;
        public double y;
        public double z;
        public double w;
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
