using RosSharp.RosBridgeClient.MessageTypes.Std;
using RosSharp.RosBridgeClient.MessageTypes.Geometry;
using RosSharp.RosBridgeClient.MessageTypes.Rtabmap;

namespace RosSharp.RosBridgeClient.MessageTypes.Rtabmap {
    public class MapGraph : Message {
        public const string RosMessageName = "rtabmap_ros/MapGraph";

        public Header header { get; set; }
        public Transform mapToOdom { get; set; }
        public int[] posesId { get; set; }
        public Pose[] poses { get; set; }
        public Link[] links { get; set; }

        public MapGraph() {
            this.header = new Header();
            this.mapToOdom = new Transform();
            this.posesId = new int[0];
            this.poses = new Pose[0];
            this.links = new Link[0];
        }

        public MapGraph(Header header, Transform mapToOdom, int[] posesId, Pose[] poses, Link[] links) {
            this.header = header;
            this.mapToOdom = mapToOdom;
            this.posesId = posesId;
            this.poses = poses;
            this.links = links;
        }
    }
}
