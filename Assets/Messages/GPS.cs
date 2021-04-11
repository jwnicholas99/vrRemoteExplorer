namespace RosSharp.RosBridgeClient.MessageTypes.Rtabmap {
    public class GPS : Message {
        public const string RosMessageName = "rtabmap_ros/GPS";

        public double stamp { get; set; }
        public double longtitude { get; set; }
        public double latitude { get; set; }
        public double altitude { get; set; }
        public double error { get; set; }
        public double bearing { get; set; }

        public GPS() {
            this.stamp = 0.0d;
            this.longtitude = 0.0d;
            this.latitude = 0.0d;
            this.altitude = 0.0d;
            this.error = 0.0d;
            this.bearing = 0.0d;
        }

        public GPS(double stamp, double longtitude, double latitude, double altitude, double error, double bearing) {
            this.stamp = stamp;
            this.longtitude = longtitude;
            this.latitude = latitude;
            this.altitude = altitude;
            this.error = error;
            this.bearing = bearing;
        }
    }
}
