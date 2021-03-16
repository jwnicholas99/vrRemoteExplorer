using RosSharp.RosBridgeClient.MessageTypes.Nav;
using RosSharp.RosBridgeClient.MessageTypes.Sensor;

namespace RosSharp.RosBridgeClient.MessageTypes.Nav {
    public class GridImage : Message {
        public const string RosMessageName = "slam/GridImage";

        public MapMetaData info { get; set; }

        public CompressedImage image { get; set; }

        public GridImage() {
            this.info = new MapMetaData();
            this.image = new CompressedImage();
        }

        public GridImage(MapMetaData info, CompressedImage image) {
            this.info = info;
            this.image = image;
        }
    }
}
