using System;
using System.Collections;
using System.Collections.Generic;
using RosSharp.RosBridgeClient.MessageTypes.Nav;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;


namespace RosSharp.RosBridgeClient {

    public class GridMapSubscriber : UnitySubscriber<MessageTypes.Nav.OccupancyGrid> {
        private sbyte[] gridArray;
        private bool isMessageReceived = false;
        bool readyToProcessMessage = true;
        private int size;

        private Texture2D texture;
        private Color[] texture_colors;
        private Vector3[] vertices;
        private int[] triangles;
        private Vector2[] uv;

        int width;
        int height;
        Vector3 position;
        Quaternion rotation;
        float resolution;

        protected override void Start() {
            base.Start();
            texture = new Texture2D(width, height);
        }

        public void Update() {

            if (isMessageReceived) {
                GridMapRendering();
                isMessageReceived = false;
            }

            //MapOrigin.transform.position = position;
            //MapOrigin.transform.rotation = rotation;
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
            texture = new Texture2D(width, height);
            texture_colors = new Color[size];

            int cell_num;
            sbyte probability;
            Color color;

            // Determine colors of texture
            for (int row_num = 0; row_num < height; row_num++) {
                for (int col_num = 0; col_num < width; col_num++) {
                    cell_num = row_num * width + col_num;

                    probability = gridArray[cell_num];
                    if (probability == 0) {
                        color = new Color(1, 1, 1);
                    } else if (probability == 100) {
                        color = new Color(0, 0, 0);
                    } else {
                        color = new Color(.5f, .5f, .5f);
                    }
                    texture_colors[cell_num] = color;
                }
            }

            texture.SetPixels(texture_colors);
            texture.Apply();

            // Vertices for the 4 corners of the mesh
            vertices = new Vector3[4] {
                new Vector3(0, 0, 0),
                new Vector3(texture.width * resolution, 0, 0).Ros2Unity(),
                new Vector3(0, texture.height * resolution, 0).Ros2Unity(),
                new Vector3(texture.width * resolution, texture.height * resolution, 0).Ros2Unity()
            };

            triangles = new int[6] {
                0, 2, 1, // lower left triangle
                2, 3, 1  // upper right triangle
            };

            uv = new Vector2[4] {
                new Vector2(0, 0),
                new Vector2(1, 0),
                new Vector2(0, 1),
                new Vector2(1, 1)
            };
        }

        public Texture2D GetTexture() {
            return texture;
        }

        public Vector3[] GetVertices() {
            return vertices;
        }

        public int[] GetTriangles() {
            return triangles;
        }

        public Vector2[] GetUV() {
            return uv;
        }

        public Vector3 GetPosition() {
            return position;
        }

        public Quaternion GetRotation() {
            return rotation;
        }
    }
}
