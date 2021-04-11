using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using RosSharp.RosBridgeClient;

public class RGBDCloudRenderer : MonoBehaviour {
    public RGBDCloudSubscriber subscriber;
    public Material material;

    // Mesh stores the positions and colours of every point in the cloud
    // The renderer and filter are used to display it
    Mesh mesh;
    MeshRenderer meshRenderer;
    MeshFilter mf;

    // The size, positions and colours of each of the pointcloud
    public float pointSize = 1f;

    [Header("MAKE SURE THESE LISTS ARE MINIMISED OR EDITOR WILL CRASH")]
    private Vector3[] positions = new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 1, 0) };
    private Color[] colours = new Color[] { new Color(1f, 0f, 0f), new Color(0f, 1f, 0f) };

    void Start() {
        meshRenderer = gameObject.AddComponent<MeshRenderer>();
        mf = gameObject.AddComponent<MeshFilter>();
        //meshRenderer.material = new Material(Shader.Find("Custom/PointCloudShader"));
        //meshRenderer.material = new Material(Shader.Find("Unlit/PointCloud"));
        meshRenderer.material = material;

        mesh = new Mesh {
            indexFormat = UnityEngine.Rendering.IndexFormat.UInt32
        };
    }

    void UpdateMesh() {
        positions = subscriber.GetPCL();
        colours = subscriber.GetPCLColor();
        if (positions == null) {
            return;
        }
        mesh.Clear();
        mesh.vertices = positions;
        mesh.colors = colours;
        int[] indices = new int[positions.Length];

        for (int i = 0; i < positions.Length; i++) {
            indices[i] = i;
        }

        mesh.SetIndices(indices, MeshTopology.Points, 0);
        mf.mesh = mesh;
    }

    // Update is called once per frame
    void Update() {
        transform.position = subscriber.GetPosition();
        transform.rotation = subscriber.GetRotation();
        //meshRenderer.material.SetFloat("_PointSize", pointSize);
        UpdateMesh();
    }
}
