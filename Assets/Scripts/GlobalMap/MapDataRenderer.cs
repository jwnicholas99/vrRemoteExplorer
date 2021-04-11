using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using RosSharp.RosBridgeClient;
using RosSharp.RosBridgeClient.MessageTypes.Std;
using RosSharp.RosBridgeClient.MessageTypes.Sensor;

public class MapDataRenderer : MonoBehaviour {
    public MapDataSubscriber mapDataSubscriber;

    // id to textures and transform dictionary
    Dictionary<int, (Texture2D, Texture2D, Vector3, Quaternion)> globalMapDict;

    // Mesh stores the positions and colours of every point in the cloud
    // The renderer and filter are used to display it
    Dictionary<int, GameObject> nodeMeshDict;

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
        nodeMeshDict = new Dictionary<int, GameObject>();
    }

    void UpdateGlobalMap() {
        globalMapDict = mapDataSubscriber.GetGlobalMapDict();
        foreach(KeyValuePair<int, (Texture2D, Texture2D, Vector3, Quaternion)> keyValue in globalMapDict) {
            int id = keyValue.Key;
            (Texture2D, Texture2D, Vector3, Quaternion) node = keyValue.Value;
            GameObject nodeMesh;

            if (!nodeMeshDict.ContainsKey(id)) {
                nodeMesh = new GameObject();
                Mesh mesh;
                MeshRenderer meshRenderer;
                MeshFilter mf;

                meshRenderer = nodeMesh.AddComponent<MeshRenderer>();
                meshRenderer.material = new Material(Shader.Find("Unlit/PointCloud"));
                meshRenderer.material.SetInt("_Width", meshWidth);
                meshRenderer.material.SetInt("_Height", meshHeight);

                mf = nodeMesh.AddComponent<MeshFilter>();
                mesh = new Mesh {
                    indexFormat = UnityEngine.Rendering.IndexFormat.UInt32
                };

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

                nodeMeshDict[id] = nodeMesh;
            }

            nodeMesh = nodeMeshDict[id];
            nodeMesh.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", node.Item1);
            nodeMesh.GetComponent<MeshRenderer>().material.SetTexture("_Depth", node.Item2);
            nodeMesh.GetComponent<MeshRenderer>().material.SetInt("_Mesh", 1);
            nodeMesh.transform.position = node.Item3;
            nodeMesh.transform.rotation = node.Item4;
        }
    }

    // Update is called once per frame
    void Update() {
        UpdateGlobalMap();
    }

    void OnDestroy() {
        mapDataSubscriber.DestroyArray();
    }
}
