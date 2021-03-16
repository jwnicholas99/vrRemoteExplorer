using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using RosSharp.RosBridgeClient;
using Unity;

public class GridImageRenderer : MonoBehaviour {
    public GridImageSubscriber subscriber;

    Mesh mesh;
    MeshRenderer meshRenderer;
    MeshFilter mf;

    private Texture2D texture;

    void Start() {
        texture = new Texture2D(0, 0);

        meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.material = new Material(Shader.Find("Standard")); ;
        mf = gameObject.AddComponent<MeshFilter>();
        mesh = new Mesh();
    }

    void UpdateTexture() {
        texture = subscriber.GetTexture();
        if (texture == null) {
            return;
        }

        mesh.Clear();
        mesh.vertices = subscriber.GetVertices();
        meshRenderer.material.SetTexture("_MainTex", texture);
        mesh.triangles = subscriber.GetTriangles();
        mesh.uv = subscriber.GetUV();

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        mf.mesh = mesh;
    }

    void Update() {
        transform.position = subscriber.GetPosition() + new Vector3(0, 0.01f, 0);
        transform.rotation = subscriber.GetRotation();
        UpdateTexture();
    }
}