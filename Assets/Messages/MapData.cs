using RosSharp.RosBridgeClient.MessageTypes.Std;
using RosSharp.RosBridgeClient.MessageTypes.Rtabmap;

namespace RosSharp.RosBridgeClient.MessageTypes.Rtabmap {
    public class MapData : Message {
        public const string RosMessageName = "rtabmap_ros/MapData";

        public Header header { get; set; }
        public MapGraph graph { get; set; }
        public NodeData[] nodes { get; set; }

        public MapData() {
            this.header = new Header();
            this.graph = new MapGraph();
            this.nodes = new NodeData[0];
        }

        public MapData(Header header, MapGraph graph, NodeData[] nodes) {
            this.header = header;
            this.graph = graph;
            this.nodes = nodes;
        }
    }
}
