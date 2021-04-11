namespace RosSharp.RosBridgeClient.MessageTypes.Rtabmap {
    public class Point2f : Message {
        public const string RosMessageName = "rtabmap_ros/Point2f";

        public float x { get; set; }
        public float y{ get; set; }

        public Point2f() {
            this.x = 0.0f;
            this.y = 0.0f;
        }

        public Point2f(float x, float y) {
            this.x = x;
            this.y = y;
        }
    }
}
