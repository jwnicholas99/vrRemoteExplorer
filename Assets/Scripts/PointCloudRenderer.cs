using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using RosSharp.RosBridgeClient;
using Unity;

public class PointCloud2Renderer : MonoBehaviour
{
    public PointCloud2Subscriber subscriber;

    // Mesh stores the positions and colours of every point in the cloud
    // The renderer and filter are used to display it
    Mesh mesh;
    MeshRenderer meshRenderer;
    MeshFilter mf;
    public Material Material;

    // The size, positions and colours of each of the pointcloud
    public float pointSize = 1f;

    private Vector3[] vertices = new Vector3[] {};
    private Color[] colours = new Color[] {};

    public Transform offset; // Put any gameobject that faciliatates adjusting the origin of the pointcloud in VR. 

    void Start()
    {
        // Give all the required components to the gameObject
        meshRenderer = gameObject.AddComponent<MeshRenderer>();
        mf = gameObject.AddComponent<MeshFilter>();
        meshRenderer.material = Material;
        //meshRenderer.material = new Material(Shader.Find("Custom/PointCloudShader"));
        mesh = new Mesh {
            // This uses 32 bit integers instead of 16-bit (which would limit the mesh to only 65536 vertices)
            indexFormat = UnityEngine.Rendering.IndexFormat.UInt32
        };

        transform.position = offset.position;
        transform.rotation = offset.rotation;
    }

    void UpdateMesh() {

        vertices = subscriber.GetGrid();
        colours = subscriber.GetGridColor();
        if (vertices == null) {
            return;
        }
        mesh.Clear();
        mesh.vertices = vertices;
        //mesh.colors = colours;

        /*
        int[] indices = new int[vertices.Length];
        for (int i = 0; i < vertices.Length; i++) {
            indices[i] = i;
        }
        mesh.SetIndices(indices, MeshTopology.Points, 0);
        */

        mf.mesh = mesh;

    }

    void Update() {
        transform.position = subscriber.GetPosition();
        transform.rotation = subscriber.GetRotation();
        //meshRenderer.material.SetFloat("_PointSize", pointSize);
        UpdateMesh();
    }
}
