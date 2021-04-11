using RosSharp.RosBridgeClient.MessageTypes.Std;

namespace RosSharp.RosBridgeClient.MessageTypes.Rtabmap {
    public class EnvSensor : Message {
        public const string RosMessageName = "rtabmap_ros/EnvSensor";

        public Header header { get; set; }
        public int type { get; set; }
        public double value { get; set; }

        public EnvSensor() {
            this.header = new Header();
            this.type = 0;
            this.value = 0.0d;
        }

        public EnvSensor(Header header, int type, double value) {
            this.header = header;
            this.type = type;
            this.value = value;
        }
    }
}
