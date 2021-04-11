using System;
using System.Collections;
using System.Collections.Generic;
using RosSharp.RosBridgeClient.MessageTypes.Std;
using RosSharp.RosBridgeClient.MessageTypes.Sensor;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;


namespace RosSharp.RosBridgeClient {
    public class RGBImageSubscriber : UnitySubscriber<MessageTypes.Sensor.CompressedImage> {
        private Texture2D texture;
        private Header imgHeader;
        private byte[] imageData;
        private bool isMessageReceived;

        protected override void Start() {
            base.Start();
            texture = new Texture2D(1, 1);
        }
        private void Update() {
            if (isMessageReceived)
                ProcessMessage();
        }

        protected override void ReceiveMessage(CompressedImage msg) {
            imgHeader = msg.header;
            imageData = msg.data;
            isMessageReceived = true;
        }

        private void ProcessMessage() {
            texture.LoadImage(imageData);
            texture.Apply();
            isMessageReceived = false;
        }

        public Header GetHeader() {
            return imgHeader;
        }

        public Texture2D GetTexture() {
            return texture;
        }
    }
}
