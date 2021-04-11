using System;
using System.Collections;
using System.Collections.Generic;
using RosSharp.RosBridgeClient.MessageTypes.Sensor;
using RosSharp.RosBridgeClient.MessageTypes.Geometry;
using RosSharp.RosBridgeClient.MessageTypes.Rtabmap;
using UnityEngine;
using UnityEngine.UI;
using Unity.Collections;
using System.Threading;
using OpenCvSharp;


namespace RosSharp.RosBridgeClient {
    public class MapDataSubscriber : UnitySubscriber<MessageTypes.Rtabmap.MapData> {
        public int width;
        public int height;
        public GameObject quad;

        private MapGraph mapGraph;
        private NodeData[] nodes;
        private bool isMessageReceived = false;
        private int size;
        private const byte FIRST_BYTE = 137;
        private NativeArray<short> decompressedDepth;

        private Dictionary<int, (Texture2D, Texture2D, UnityEngine.Vector3, UnityEngine.Quaternion)> globalMapDict;
        private int[] nodeIds;
        private UnityEngine.Vector3 baseToCameraTransform;


        protected override void Start() {
            base.Start();
            decompressedDepth = new NativeArray<short>(width * height, Allocator.Persistent);
            globalMapDict = new Dictionary<int, (Texture2D, Texture2D, UnityEngine.Vector3, UnityEngine.Quaternion)>();
        }

        public void Update() {
            if (isMessageReceived) {
                ProcessMessage();
            }
        }

        protected override void ReceiveMessage(MapData msg) {
            mapGraph = msg.graph;
            nodes = msg.nodes;
            isMessageReceived = true;
        }

        private void ProcessMessage() {
            Dictionary<int, MessageTypes.Geometry.Pose> poseDict = new Dictionary<int, MessageTypes.Geometry.Pose>(); ;
            for (int i = 0; i < mapGraph.posesId.Length; i++) {
                int id = mapGraph.posesId[i];
                MessageTypes.Geometry.Pose pose = mapGraph.poses[i];
                poseDict[id] = pose;
            }

            for (int i = 0; i < nodes.Length; i++) {
                int id = nodes[i].id;

                // Uncompress color image and apply to texture
                byte[] colorData = nodes[i].image;
                Texture2D colorTexture = new Texture2D(1, 1);
                if (colorData.Length == 0) {
                    continue;
                }
                colorTexture.LoadImage(colorData);
                colorTexture.Apply();
                quad.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", colorTexture);

                // Uncompress depth image and apply to texture
                byte[] depthData = nodes[i].depth;
                if (depthData.Length == 0) {
                    continue;
                }
                int first = GetFirstOccurance(depthData, FIRST_BYTE);
                if (first > 0) {
                    byte[] returnData = new byte[depthData.Length - first];
                    Array.Copy(depthData, first, returnData, 0, depthData.Length - first);
                    depthData = returnData;
                }

                Texture2D depthTexture = new Texture2D(width, height, TextureFormat.R16, false);
                Mat mat = Mat.ImDecode(depthData, ImreadModes.AnyDepth);
                short[] data = new short[width * height];
                mat.GetArray(0, 0, data);
                decompressedDepth.CopyFrom(data);
                depthTexture.LoadRawTextureData(decompressedDepth);
                depthTexture.Apply();

                // Get transform for node
                baseToCameraTransform = GetTransformPosition(nodes[i].localTransform[0]).Ros2Unity();
                UnityEngine.Vector3 position = GetPosePosition(poseDict[id]).Ros2Unity() + GetTransformPosition(mapGraph.mapToOdom).Ros2Unity() + baseToCameraTransform;
                UnityEngine.Quaternion rotation = GetPoseRotation(poseDict[id]).Ros2Unity() * GetTransformRotation(mapGraph.mapToOdom).Ros2Unity(); // Ensure why do not need the rotation of the /base_link->/camera_link;

                // Add id->(colorTexture, depthTexture, position, rotation) to dictionary
                globalMapDict[id] = (colorTexture, depthTexture, position, rotation.normalized);
            }

            // Update the postition and rotation of every node each time
            for (int i = 0; i < mapGraph.posesId.Length; i++) {
                //Debug.Log("posesId: " + mapGraph.posesId[i]);
                int id = mapGraph.posesId[i];

                if (globalMapDict.ContainsKey(id)){
                    (Texture2D, Texture2D, UnityEngine.Vector3, UnityEngine.Quaternion) nodeData = globalMapDict[id];
                    UnityEngine.Vector3 position = GetPosePosition(poseDict[id]).Ros2Unity() + GetTransformPosition(mapGraph.mapToOdom).Ros2Unity() + baseToCameraTransform;
                    UnityEngine.Quaternion rotation = GetPoseRotation(poseDict[id]).Ros2Unity() * GetTransformRotation(mapGraph.mapToOdom).Ros2Unity();
                    //if(nodeData.Item3 != position) {
                    //Debug.Log("PoseId: " + id + ", Updated from " + nodeData.Item3 + " to " + position);
                    //}
                    globalMapDict[id] = (nodeData.Item1, nodeData.Item2, position, rotation);
                }
            }

            // Remove nodes that are no longer in mapGraph
            foreach (KeyValuePair<int, (Texture2D, Texture2D, UnityEngine.Vector3, UnityEngine.Quaternion)> keyValue in globalMapDict) {
                int id = keyValue.Key;
                if (!poseDict.ContainsKey(id)) {
                    globalMapDict.Remove(id);
                }
            }

            isMessageReceived = false;
        }

        public Dictionary<int, (Texture2D, Texture2D, UnityEngine.Vector3, UnityEngine.Quaternion)> GetGlobalMapDict() {
            return globalMapDict;
        }

        public void DestroyArray() {
            decompressedDepth.Dispose();
        }

        public int GetFirstOccurance(byte[] array, byte element) {
            return Array.IndexOf(array, element);
        }

        private UnityEngine.Vector3 GetPosePosition(MessageTypes.Geometry.Pose pose) {
            return new UnityEngine.Vector3(
                (float)pose.position.x,
                (float)pose.position.y,
                (float)pose.position.z);
        }

        private UnityEngine.Quaternion GetPoseRotation(MessageTypes.Geometry.Pose pose) {
            return new UnityEngine.Quaternion(
                (float)pose.orientation.x,
                (float)pose.orientation.y,
                (float)pose.orientation.z,
                (float)pose.orientation.w);
        }

        private UnityEngine.Vector3 GetTransformPosition(MessageTypes.Geometry.Transform transform) {
            return new UnityEngine.Vector3(
                (float)transform.translation.x,
                (float)transform.translation.y,
                (float)transform.translation.z);
        }

        private UnityEngine.Quaternion GetTransformRotation(MessageTypes.Geometry.Transform transform) {
            return new UnityEngine.Quaternion(
                (float)transform.rotation.x,
                (float)transform.rotation.y,
                (float)transform.rotation.z,
                (float)transform.rotation.w);
        }
    }
}

