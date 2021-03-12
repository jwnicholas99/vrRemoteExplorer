﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RosSharp.RosBridgeClient {
    public class TFSubscriber : UnitySubscriber<MessageTypes.Tf2.TFMessage> {
        public Transform PublishedTransform;
        public string baseName;

        private Vector3 position;
        private Quaternion rotation;
        private Vector3 posCorrection;
        private Quaternion rotCorrection;
        private bool isMessageReceived;
        private bool isInitialPositionSet;

        protected override void Start() {
            base.Start();
        }

        private void Update() {
            if (isMessageReceived)
                ProcessMessage();
        }

        protected override void ReceiveMessage(MessageTypes.Tf2.TFMessage message) {
            if (message.transforms[0].child_frame_id == baseName) {
                position = GetPosition(message).Ros2Unity() + posCorrection;
                rotation = GetRotation(message).Ros2Unity() * rotCorrection;
            } else if (message.transforms[0].child_frame_id == "odom") {
                posCorrection = GetPosition(message).Ros2Unity();
                rotCorrection = GetRotation(message).Ros2Unity();
            }
            isMessageReceived = true;
        }
        private void ProcessMessage() {
            PublishedTransform.position = position;
            PublishedTransform.rotation = rotation;
        }

        private Vector3 GetPosition(MessageTypes.Tf2.TFMessage message) {
            return new Vector3(
                (float)message.transforms[0].transform.translation.x,
                (float)message.transforms[0].transform.translation.y,
                (float)message.transforms[0].transform.translation.z);
        }

        private Quaternion GetRotation(MessageTypes.Tf2.TFMessage message) {
            
            return new Quaternion(
                (float)message.transforms[0].transform.rotation.x,
                (float)message.transforms[0].transform.rotation.y,
                (float)message.transforms[0].transform.rotation.z,
                (float)message.transforms[0].transform.rotation.w);
        }
    }
}
