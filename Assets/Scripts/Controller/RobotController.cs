using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

namespace RosSharp.RosBridgeClient {
    public class RobotController : UnityPublisher<MessageTypes.Sensor.Joy> { 
        public SteamVR_Action_Vector2 input;
        public SteamVR_Action_Boolean IsRotRobot;
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
            message.axes = new float[3];
            message.buttons = new int[1];
        }

        private void UpdateMessage() {
            message.header.Update();
            message.axes[0] = input.axis.x;
            message.axes[1] = input.axis.y;
            message.buttons[0] = Convert.ToInt32(IsRotRobot.state);

            Publish(message);
        }
    }
}