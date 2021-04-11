using RosSharp.RosBridgeClient.MessageTypes.Geometry;
using RosSharp.RosBridgeClient.MessageTypes.Sensor;
using RosSharp.RosBridgeClient.MessageTypes.Rtabmap;

namespace RosSharp.RosBridgeClient.MessageTypes.Rtabmap {
    public class NodeData : Message {
        public const string RosMessageName = "rtabmap_ros/NodeData";

        public int id { get; set; }
        public int mapId { get; set; }
        public int weight { get; set; }
        public double stamp { get; set; }
        public string label { get; set; }
        public Pose pose { get; set; }
        public Pose groundTruthPose { get; set; }
        public GPS gps { get; set; }
        public byte[] image { get; set; }
        public byte[] depth { get; set; }
        public float[] fx { get; set; }
        public float[] fy { get; set; }
        public float[] cx { get; set; }
        public float[] cy { get; set; }
        public float[] width { get; set; }
        public float[] height { get; set; }
        public float baseline { get; set; }
        public Transform[] localTransform { get; set; }
        public byte[] laserScan { get; set; }
        public int laserScanMaxPts { get; set; }
        public float laserScanMaxRange { get; set; }
        public int laserScanFormat { get; set; }
        public Transform laserScanLocalTransform { get; set; }
        public byte[] userData { get; set; }
        public byte[] grid_ground { get; set; }
        public byte[] grid_obstacles { get; set; }
        public byte[] grid_empty_cells { get; set; }
        public float grid_cell_size { get; set; }
        public Point3f grid_view_point { get; set; }
        public int[] wordIds { get; set; }
        public KeyPoint[] wordKpts { get; set; }
        public Point3f[] wordPts { get; set; }
        public byte[] wordDescriptors { get; set; }
        public GlobalDescriptor[] globalDescriptors { get; set; }
        public EnvSensor[] env_sensors { get; set; }

        public NodeData() {
            this.id = 0;
            this.mapId = 0;
            this.weight = 0;
            this.stamp = 0.0d;
            this.label = "";
            this.pose = new Pose();
            this.groundTruthPose = new Pose();
            this.gps = new GPS();
            this.image = new byte[0];
            this.depth = new byte[0];
            this.fx = new float[0];
            this.fy = new float[0];
            this.cx = new float[0];
            this.cy = new float[0];
            this.width = new float[0];
            this.height = new float[0];
            this.baseline = 0.0f;
            this.localTransform = new Transform[0];
            this.laserScan = new byte[0];
            this.laserScanMaxPts = 0;
            this.laserScanMaxRange = 0.0f;
            this.laserScanFormat = 0;
            this.laserScanLocalTransform = new Transform();
            this.userData = new byte[0];
            this.grid_ground = new byte[0];
            this.grid_obstacles = new byte[0];
            this.grid_empty_cells = new byte[0];
            this.grid_cell_size = 0.0f;
            this.grid_view_point = new Point3f();
            this.wordIds = new int[0];
            this.wordKpts = new KeyPoint[0];
            this.wordPts = new Point3f[0];
            this.wordDescriptors = new byte[0];
            this.globalDescriptors = new GlobalDescriptor[0];
            this.env_sensors = new EnvSensor[0];
        }

        public NodeData(int id, int mapId, int weight, double stamp, string label, Pose pose, Pose groundTruthPose, GPS gps, byte[] image, byte[] depth, 
            float[] fx, float[] fy, float[] cx, float[] cy, float[] width, float[] height, float baseline, Transform[] localTransform, byte[] laserScan, 
            int laserScanMaxPts, float laserScanMaxRange, int laserScanFormat, Transform laserScanLocalTransform, byte[] userData, byte[] grid_ground, 
            byte[] grid_obstacles, byte[] grid_empty_cells, float grid_cell_size, Point3f grid_view_point, int[] wordIds, KeyPoint[] wordKpts, 
            Point3f[] wordPts, byte[] wordDescriptors, GlobalDescriptor[] globalDescriptors, EnvSensor[] env_sensors) {

            this.id = id;
            this.mapId = mapId;
            this.weight = weight;
            this.stamp = stamp;
            this.label = label;
            this.pose = pose;
            this.groundTruthPose = groundTruthPose;
            this.gps = gps;
            this.image = image;
            this.depth = depth;
            this.fx = fx;
            this.fy = fy;
            this.cx = cx;
            this.cy = cy;
            this.width = width;
            this.height = height;
            this.baseline = baseline;
            this.localTransform = localTransform;
            this.laserScan = laserScan;
            this.laserScanMaxPts = laserScanMaxPts;
            this.laserScanMaxRange = laserScanMaxRange;
            this.laserScanFormat = laserScanFormat;
            this.laserScanLocalTransform = laserScanLocalTransform;
            this.userData = userData;
            this.grid_ground = grid_ground;
            this.grid_obstacles = grid_obstacles;
            this.grid_empty_cells = grid_empty_cells;
            this.grid_cell_size = grid_cell_size;
            this.grid_view_point = grid_view_point;
            this.wordIds = wordIds;
            this.wordKpts = wordKpts;
            this.wordPts = wordPts;
            this.wordDescriptors = wordDescriptors;
            this.globalDescriptors = globalDescriptors;
            this.env_sensors = env_sensors;
        }
    }
}
