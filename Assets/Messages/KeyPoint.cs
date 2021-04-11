using RosSharp.RosBridgeClient.MessageTypes.Rtabmap;

namespace RosSharp.RosBridgeClient.MessageTypes.Rtabmap {
    public class KeyPoint : Message {
        public const string RosMessageName = "rtabmap_ros/KeyPoint";

        public Point2f pt { get; set; }
        public float size { get; set; }
        public float angle { get; set; }
        public float response { get; set; }
        public int octave { get; set; }
        public int class_id { get; set; }


        public KeyPoint() {
            this.pt = new Point2f();
            this.size = 0.0f;
            this.angle = 0.0f;
            this.response = 0.0f;
            this.octave = 0;
            this.class_id = 0;
        }

        public KeyPoint(Point2f pt, float size, float angle, float response, int octave, int class_id) {
            this.pt = pt;
            this.size = size;
            this.angle = angle;
            this.response = response;
            this.octave = octave;
            this.class_id = class_id;
        }
    }
}
