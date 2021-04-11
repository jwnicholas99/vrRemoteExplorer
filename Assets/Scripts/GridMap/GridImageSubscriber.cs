/*
© Siemens AG, 2017-2018
Author: Dr. Martin Bischoff (martin.bischoff@siemens.com)

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at
<http://www.apache.org/licenses/LICENSE-2.0>.
Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

using UnityEngine;

namespace RosSharp.RosBridgeClient {
    [RequireComponent(typeof(RosConnector))]
    public class GridImageSubscriber : UnitySubscriber<MessageTypes.Nav.GridImage> {
        private sbyte[] gridArray;
        private bool isMessageReceived = false;
        private int size;

        private Texture2D texture;
        private byte[] imageData;
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
            texture = new Texture2D(1, 1);
        }
        private void Update() {
            if (isMessageReceived)
                ProcessMessage();
        }

        protected override void ReceiveMessage(MessageTypes.Nav.GridImage gridImage) {
            position = new Vector3(
                (float)gridImage.info.origin.position.x,
                (float)gridImage.info.origin.position.y,
                (float)gridImage.info.origin.position.z
            );
            position = position.Ros2Unity();

            rotation = new Quaternion(
                (float)gridImage.info.origin.orientation.x,
                (float)gridImage.info.origin.orientation.y,
                (float)gridImage.info.origin.orientation.z,
                (float)gridImage.info.origin.orientation.w
            );
            rotation = rotation.Ros2Unity();

            width = (int)gridImage.info.width;
            height = (int)gridImage.info.height;
            resolution = gridImage.info.resolution;

            imageData = gridImage.image.data;

            isMessageReceived = true;
        }

        private void ProcessMessage() {
            texture = new Texture2D(width, height);
            texture.LoadImage(imageData);
            texture.Apply();

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

            isMessageReceived = false;
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

