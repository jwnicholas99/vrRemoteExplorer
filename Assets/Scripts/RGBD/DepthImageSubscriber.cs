using System;
using System.Collections;
using System.Collections.Generic;
using RosSharp.RosBridgeClient.MessageTypes.Std;
using RosSharp.RosBridgeClient.MessageTypes.Sensor;
using UnityEngine;
using UnityEngine.UI;
using Unity.Collections;
using System.Threading;
using OpenCvSharp;


namespace RosSharp.RosBridgeClient {
    public class DepthImageSubscriber : UnitySubscriber<MessageTypes.Sensor.CompressedImage> {
        public int width;
        public int height;
        private Texture2D texture;
        private Header imgHeader;
        private byte[] imageData;
        private bool isMessageReceived;
        private const byte FIRST_BYTE = 137;

        private NativeArray<short> decompressedDepth;

        protected override void Start() {
            base.Start();
            decompressedDepth = new NativeArray<short>(width * height, Allocator.Persistent);
            texture = new Texture2D(width, height, TextureFormat.R16, false);
        }
        private void Update() {
            if (isMessageReceived)
                ProcessMessage();
        }

        protected override void ReceiveMessage(MessageTypes.Sensor.CompressedImage msg) {
            imgHeader = msg.header;
            imageData = msg.data;

            // first indicates where the start of the data is after the header
            int first = GetFirstOccurance(imageData, FIRST_BYTE);
            if (first > 0) {
                byte[] returnData = new byte[imageData.Length - first];
                Array.Copy(imageData, first, returnData, 0, imageData.Length - first);
                imageData = returnData;
            }

            isMessageReceived = true;
        }

        private void ProcessMessage() {
            Mat mat = Mat.ImDecode(imageData, ImreadModes.AnyDepth);
            short[] data = new short[width * height];
            mat.GetArray(0, 0, data);
            decompressedDepth.CopyFrom(data);

            texture.LoadRawTextureData(decompressedDepth);
            texture.Apply();

            isMessageReceived = false;
        }

        public void DestroyArray() {
            decompressedDepth.Dispose();
        }

        public Header GetHeader() {
            return imgHeader;
        }

        public Texture2D GetTexture() {
            return texture;
        }

        public int GetFirstOccurance(byte[] array, byte element) {
            return Array.IndexOf(array, element);
        }
    }
}
