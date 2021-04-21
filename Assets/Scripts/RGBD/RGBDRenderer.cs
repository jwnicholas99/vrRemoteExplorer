using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using RosSharp.RosBridgeClient;
using RosSharp.RosBridgeClient.MessageTypes.Std;
using RosSharp.RosBridgeClient.MessageTypes.Sensor;

public class RGBDRenderer : MonoBehaviour {
    public RGBImageSubscriber rgbSubscriber;
    public DepthImageSubscriber depthSubscriber;
    public Transform CameraOrigin;
    public Material material;
    public GameObject quad;

    // rgb and depth images from the subscribers
    private Header rgbHeader;
    private Header depthHeader;

    private Texture2D rgbTexture;
    private Texture2D depthTexture;

    // Mesh stores the positions and colours of every point in the cloud
    // The renderer and filter are used to display it
    Mesh mesh;
    MeshRenderer meshRenderer;
    MeshFilter mf;

    // The size, positions and colours of each of the pointcloud
    public int meshWidth;
    public int meshHeight;
    public int cameraWidth;
    public int cameraHeight;
    public float focalX;
    public float focalY;

    [Header("MAKE SURE THESE LISTS ARE MINIMISED OR EDITOR WILL CRASH")]
    private Vector3[] positions = new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 1, 0) };
    private Color[] colours = new Color[] { new Color(1f, 0f, 0f), new Color(0f, 1f, 0f) };

    void Start() {
        meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.material = material;
        meshRenderer.material.SetInt("_Width", meshWidth);
        meshRenderer.material.SetInt("_Height", meshHeight);

        mf = gameObject.AddComponent<MeshFilter>();
        mesh = new Mesh {
            indexFormat = UnityEngine.Rendering.IndexFormat.UInt32
        };

        // Set-up mesh for RGBD as mesh params should stay the same (same camera)
        // This was written in reference to brownvc/movo-teleop's PointCloud.cs
        Vector3[] vertices = new Vector3[meshWidth * meshHeight * 4];
        Vector2[] uv = new Vector2[meshWidth * meshHeight * 4];

        int[] triangles = new int[meshWidth * meshHeight * 6];
        int[] quad = new int[] { 0, 1, 2, 1, 3, 2 };
        for (int i = 0; i < meshWidth; i++) {
            for (int j = 0; j < meshHeight; j++) {
                float u = ((float)i + 0.5f) / meshWidth;
                float v = ((float)j + 0.5f) / meshHeight;
                float x = u * 2 - 1;
                float y = v * 2 - 1;
                int index = i * meshHeight + j;
                for (int k = 0; k < 2; k++) {
                    for (int l = 0; l < 2; l++) {
                        Vector3 imageCoords = new Vector3(x - (k - 0.5f) * (2f / meshWidth), y - (l - 0.5f) * (2f / meshHeight), 1);
                        Vector3 worldCoords = new Vector3(imageCoords.x * cameraWidth / 2 * 1 / focalX, imageCoords.y * cameraHeight / 2 * 1 / focalY, 1);
                        vertices[index * 4 + k * 2 + l] = worldCoords;
                        uv[index * 4 + k * 2 + l] = new Vector2(k + u, l + v);
                    }
                }
                for (int k = 0; k < 6; k++) {
                    triangles[index * 6 + k] = index * 4 + quad[k];
                }
            }
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        mesh.bounds = new Bounds(transform.position, new Vector3(1000, 1000, 1000));
        mf.mesh = mesh;
    }

    void UpdateMesh() {
        //quad.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", rgbSubscriber.GetTexture());
        quad.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", depthSubscriber.GetTexture());

        rgbHeader = rgbSubscriber.GetHeader();
        depthHeader = depthSubscriber.GetHeader();

        if (rgbHeader == null || depthHeader == null) {
            return;
        }

        if (rgbHeader.seq != depthHeader.seq) {
            return;
        }

        material = meshRenderer.material;
        material.SetTexture("_MainTex", rgbSubscriber.GetTexture());
        material.SetTexture("_Depth", depthSubscriber.GetTexture());
        material.SetInt("_Mesh", 1);
    }

    // Update is called once per frame
    void Update() {
        transform.position = CameraOrigin.position;
        transform.rotation = CameraOrigin.rotation;
        UpdateMesh();
    }

    void OnDestroy() {
        depthSubscriber.DestroyArray();
    }
}
