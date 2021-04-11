using RosSharp.RosBridgeClient.MessageTypes.Std;

namespace RosSharp.RosBridgeClient.MessageTypes.Rtabmap {
    public class GlobalDescriptor : Message {
        public const string RosMessageName = "rtabmap_ros/GlobalDescriptor";

        public Header header { get; set; }
        public int type { get; set; }
        public byte[] info { get; set; }
        public byte[] data { get; set; }

        public GlobalDescriptor() {
            this.header = new Header();
            this.type = 0;
            this.info = new byte[0];
            this.data = new byte[0];
        }

        public GlobalDescriptor(Header header, int type, byte[] info, byte[] data) {
            this.header = header;
            this.type = type;
            this.info = info;
            this.data = data;
        }
    }
}
