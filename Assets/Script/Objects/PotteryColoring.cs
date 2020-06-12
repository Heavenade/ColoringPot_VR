using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Globalization;

public class PotteryColoring : MonoBehaviour
{
    Mesh potteryMesh;
    Vector3[] potteryVertices, brushVertices;

    public string potteryName;
    public Transform brush;

    // Start is called before the first frame update
    void Start()
    {
        //call mesh
        potteryMesh = (Mesh)AssetDatabase.LoadAssetAtPath("Assets/SavedPottery/" + potteryName + ".asset", typeof(Mesh));
        brushVertices = brush.GetComponent<MeshFilter>().mesh.vertices;
        InitializeMeshColor();
        DrawMesh();
        
    }

    // Update is called once per frame
    void Update()
    {
        Coloring(brush, brushVertices);
        DrawMesh();
    }

    void DrawMesh()
    {
        GetComponent<MeshFilter>().mesh = potteryMesh;
        potteryMesh.RecalculateNormals();
    }

    void InitializeMeshColor()
    {
        Vector3[] vertices = potteryMesh.vertices;
        Color[] colors = new Color[vertices.Length];
        for (int i = 0; i < vertices.Length; i++)
        {
            colors[i] = Color.white;
        }
        
        potteryMesh.colors = colors;
    }

    void Coloring(Transform brush, Vector3[] brushVertices)
    {
        Vector3 brushPosition;
        Vector3[] potteryVertices = potteryMesh.vertices;
        Color[] colors = potteryMesh.colors;

        for (int i = 0; i < brushVertices.Length ; i++)
        {
            brushPosition = brush.TransformPoint(brushVertices[i]) - this.GetComponent<Transform>().position;
            float x = brushPosition.x;
            float y = brushPosition.y;
            float z = brushPosition.z;

            for(int j = 0; j < potteryVertices.Length; j++)
            {
                if (potteryVertices[j].x - x <= 0.01f && potteryVertices[j].x - x >= -0.01f && potteryVertices[j].y - y <= 0.01f && potteryVertices[j].y - y >= -0.01f && potteryVertices[j].z - z <= 0.01f && potteryVertices[j].z - z >= -0.01f)
                {
                    colors[j] = Color.red;
                }
            }
        }
        potteryMesh.colors = colors;
    }


}
