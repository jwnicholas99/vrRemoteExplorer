using System;
using System.Collections;
using System.Collections.Generic;
using RosSharp.RosBridgeClient.MessageTypes.Nav;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;


namespace RosSharp.RosBridgeClient {

    public class PointCloud2Subscriber : UnitySubscriber<MessageTypes.Nav.OccupancyGrid> {
        private sbyte[] gridArray;
        private bool isMessageReceived = false;
        bool readyToProcessMessage = true;
        private int size;

        private Vector3[] grid;
        private Color[] grid_color;
        private int[] meshTriangles;

        int width;
        int height;
        Vector3 position;
        Quaternion rotation;
        float resolution;

        protected override void Start() {
            base.Start();
        }

        public void Update() {
            
            if (isMessageReceived) {
                GridMapRendering();
                isMessageReceived = false;
            }
            
        }

        protected override void ReceiveMessage(MessageTypes.Nav.OccupancyGrid message) {
            width = (int)message.info.width;
            height = (int)message.info.height;
            position = new Vector3(
                (float)message.info.origin.position.x,
                (float)message.info.origin.position.y,
                (float)message.info.origin.position.z
            );

            position = position.Ros2Unity();
            
            rotation = new Quaternion(
                (float)message.info.origin.orientation.x,
                (float)message.info.origin.orientation.y,
                (float)message.info.origin.orientation.z,
                (float)message.info.origin.orientation.w
            );
            rotation = rotation.Ros2Unity();

            size = width * height;
            resolution = message.info.resolution;

            // OccupancyGrid data is in row-major order
            gridArray = new sbyte[size];
            gridArray = message.data;

            isMessageReceived = true;
        }

        void GridMapRendering() {
            grid = new Vector3[size * 4]; // each cell has 4 vertices
            grid_color = new Color[size * 4];
            meshTriangles = new int[size * 6]; // each cell is a quad as 2 triangles with 3 vertices each

            // initialize trackers
            int v = 0;
            int t = 0;

            float x;
            float y;
            float z;
            sbyte probability;
            Color color;
     
            for (int row_num = 0; row_num < height; row_num++) {
                for (int col_num = 0; col_num < width; col_num++) {
                    x = col_num * resolution;
                    y = row_num * resolution;
                    z = 0;
                    grid[v] = new Vector3(x, y, z).Ros2Unity();
                    grid[v+1] = new Vector3(x, y + resolution, z).Ros2Unity();
                    grid[v+2] = new Vector3(x + resolution, y, z).Ros2Unity();
                    grid[v+3] = new Vector3(x + resolution, y + resolution, z).Ros2Unity();

                    meshTriangles[t] = v;
                    meshTriangles[t+1] = meshTriangles[t+4] = v+1;
                    meshTriangles[t+2] = meshTriangles[t+3] = v+2;
                    meshTriangles[t+5] = v+3;

                    probability = gridArray[row_num*width + col_num];
                    if (probability == 0) {
                        color = new Color(1, 1, 1);
                    } else if (probability == 100) {
                        color = new Color(0, 0, 0);
                    } else {
                        color = new Color(.5f, .5f, .5f);
                    }
                    grid_color[v] = color;
                    grid_color[v+1] = color;
                    grid_color[v+2] = color;
                    grid_color[v+3] = color;

                    v += 4;
                    t += 6;
                }
            }
        }

        public Vector3[] GetGrid() {
            return grid;
        }

        public Color[] GetGridColor() {
            return grid_color;
        }

        public Vector3 GetPosition() {
            return position;
        }

        public Quaternion GetRotation() {
            return rotation;
        }

        public int[] GetTriangles() {
            return meshTriangles;
        }
    }
}
