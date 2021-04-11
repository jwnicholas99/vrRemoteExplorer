using RosSharp.RosBridgeClient.MessageTypes.Std;
using RosSharp.RosBridgeClient.MessageTypes.Sensor;
using RosSharp.RosBridgeClient.MessageTypes.Rtabmap;

namespace RosSharp.RosBridgeClient.MessageTypes.Rtabmap {
    public class RGBDImage : Message {
        public const string RosMessageName = "rtabmap_ros/RGBDImage";

        public Header header { get; set; }
        public CameraInfo rgb_camera_info { get; set; }
        public CameraInfo depth_camera_info { get; set; }
        public Image rgb { get; set; }
        public Image depth { get; set; }
        public CompressedImage rgb_compressed { get; set; }
        public CompressedImage depth_compressed { get; set; }
        public KeyPoint[] key_points { get; set; }
        public Point3f[] points { get; set; }
        public byte[] descriptors { get; set; }
        public GlobalDescriptor global_descriptor { get; set; }

        public RGBDImage() {
            this.header = new Header();
            this.rgb_camera_info = new CameraInfo();
            this.depth_camera_info = new CameraInfo();
            this.rgb = new Image();
            this.depth = new Image();
            this.rgb_compressed = new CompressedImage();
            this.depth_compressed = new CompressedImage();
            this.key_points = new KeyPoint[0];
            this.descriptors = new byte[0];
            this.global_descriptor = new GlobalDescriptor();
        }

        public RGBDImage(Header header, CameraInfo rgb_camera_info, CameraInfo depth_camera_info,
                         Image rgb, Image depth, CompressedImage rgb_compressed, CompressedImage depth_compressed,
                         KeyPoint[] key_points, Point3f[] points, byte[] descriptors, GlobalDescriptor global_descriptor) {
            this.header = header;
            this.rgb_camera_info = rgb_camera_info;
            this.depth_camera_info = depth_camera_info;
            this.rgb = rgb;
            this.depth = depth;
            this.rgb_compressed = rgb_compressed;
            this.depth_compressed = depth_compressed;
            this.key_points = key_points;
            this.descriptors = descriptors;
            this.global_descriptor = global_descriptor;
        }
    }
}
