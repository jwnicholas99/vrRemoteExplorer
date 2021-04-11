using RosSharp.RosBridgeClient.MessageTypes.Geometry;

namespace RosSharp.RosBridgeClient.MessageTypes.Rtabmap {
    public class Link : Message {
        public const string RosMessageName = "rtabmap_ros/Link";

        public int fromId { get; set; }
        public int toId { get; set; }
        public int type { get; set; }
        public Transform transform { get; set; }
        public double[] information { get; set; }


        public Link() {
            this.fromId = 0;
            this.toId = 0;
            this.type = 0;
            this.transform = new Transform();
            this.information = new double[36];
        }

        public Link(int fromId, int toId, int type, Transform transform, double[] information) {
            this.fromId = fromId;
            this.toId = toId;
            this.type = type;
            this.transform = transform;
            this.information = information;
        }
    }
}
