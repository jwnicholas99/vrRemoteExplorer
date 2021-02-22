using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

namespace RosSharp.RosBridgeClient {
    public class RobotController : UnityPublisher<MessageTypes.Sensor.Joy> { 
        public SteamVR_Action_Vector2 input;
        public float speed = 1;

        public string FrameId = "Unity";

        private MessageTypes.Sensor.Joy message;

        protected override void Start() {
            base.Start();
            InitializeMessage();
        }

        private void Update() {
            UpdateMessage();
        }

        private void InitializeMessage() {
            message = new MessageTypes.Sensor.Joy();
            message.header.frame_id = FrameId;
            message.axes = new float[2];
            message.buttons = new int[0];
        }

        private void UpdateMessage() {
            message.header.Update();

            message.axes[0] = input.axis.x;
            message.axes[1] = input.axis.y;

            Publish(message);
        }
    }
}