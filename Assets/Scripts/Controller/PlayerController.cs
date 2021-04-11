using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class PlayerController : MonoBehaviour
{
    public SteamVR_Action_Vector2 input;
    public SteamVR_Action_Boolean IsRotPlayer;
    public float TransSpeed = 1;
    public float RotSpeed = 2;

    // Update is called once per frame
    void Update(){
        if (IsRotPlayer.state) {
            if(input.axis.x < 0) {
                transform.rotation *= Quaternion.Euler(0, -RotSpeed, 0);
            } else if (input.axis.x > 0) {
                transform.rotation *= Quaternion.Euler(0, RotSpeed, 0);
            }
        } else {
            Vector3 direction = Player.instance.hmdTransform.TransformDirection(new Vector3(input.axis.x, 0, input.axis.y));
            transform.position += TransSpeed * Time.deltaTime * Vector3.ProjectOnPlane(direction, Vector3.up);
        }
        
    }
}
