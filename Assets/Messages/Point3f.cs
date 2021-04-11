namespace RosSharp.RosBridgeClient.MessageTypes.Rtabmap {
    public class Point3f : Message {
        public const string RosMessageName = "rtabmap_ros/Point3f";

        public float x { get; set; }
        public float y{ get; set; }
        public float z { get; set; }

        public Point3f() {
            this.x = 0.0f;
            this.y = 0.0f;
            this.z = 0.0f;
        }

        public Point3f(float x, float y, float z) {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }
}
