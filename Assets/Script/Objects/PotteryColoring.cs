using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Globalization;
using System.IO;

public class PotteryColoring : MonoBehaviour
{
    Mesh potteryMesh;
    Vector3[] potteryVertices, brushVertices;

    public Transform brush;
    public int colorNum = 1;

    // Start is called before the first frame update
    void Start()
    {
        //call mesh
        if (GameManager.instance.potteryMesh == null)
        {
            potteryMesh = (Mesh)AssetDatabase.LoadAssetAtPath("Assets/Resources/Prefab/SamplePottery.asset", typeof(Mesh));
        }
        else
        {
            potteryMesh = GameManager.instance.potteryMesh;
        }
        brushVertices = brush.GetComponent<MeshFilter>().mesh.vertices;
        InitializeMeshColor();
        DrawMesh();

    }

    // Update is called once per frame
    void Update()
    {
        Coloring(brush, brushVertices, colorNum);
        DrawMesh();
        if (Input.GetKeyDown("f"))
        {
            SavePotteryEx();
        }
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

    //colorNum: 1->red, 2->blue, 3->green, 4->yellow, 5->white, 6->black
    void Coloring(Transform brush, Vector3[] brushVertices, int colorNum)
    {
        Vector3 brushPosition;
        Vector3[] potteryVertices = potteryMesh.vertices;
        Color[] colors = potteryMesh.colors;

        for (int i = 0; i < brushVertices.Length; i++)
        {
            brushPosition = brush.TransformPoint(brushVertices[i]) - this.GetComponent<Transform>().position;
            float x = brushPosition.x;
            float y = brushPosition.y;
            float z = brushPosition.z;

            for (int j = 0; j < potteryVertices.Length; j++)
            {
                if (potteryVertices[j].x - x <= 0.01f && potteryVertices[j].x - x >= -0.01f && potteryVertices[j].y - y <= 0.01f && potteryVertices[j].y - y >= -0.01f && potteryVertices[j].z - z <= 0.01f && potteryVertices[j].z - z >= -0.01f)
                {
                    if (colorNum == 1)
                    {
                        colors[j] = Color.red;
                    }
                    else if (colorNum == 2)
                    {
                        colors[j] = Color.blue;
                    }
                    else if (colorNum == 3)
                    {
                        colors[j] = Color.green;
                    }
                    else if (colorNum == 4)
                    {
                        colors[j] = Color.yellow;
                    }
                    else if (colorNum == 5)
                    {
                        colors[j] = Color.white;
                    }
                    else if (colorNum == 6)
                    {
                        colors[j] = Color.black;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
        potteryMesh.colors = colors;
    }

    //file saved- > true, not saved -> false
    public bool SavePottery()
    {
        bool isSaved = false;
        string savingPotteryPath;
        string sourcePotteryPath = "Assets/SavedPottery/workingPottery.asset";

        //max number of storable pottery
        int maxStorablePotteryNum = 3;

        for (int i = 1; i <= maxStorablePotteryNum; i++)
        {
            savingPotteryPath = "Assets/SavedPottery/pottery" + i + ".asset";

            FileInfo isPotteryFile = new FileInfo(savingPotteryPath);
            if (isPotteryFile.Exists == false)
            {
                //파일저장
                AssetDatabase.CopyAsset(sourcePotteryPath, savingPotteryPath);
                isSaved = true;
                return isSaved;
            }
        }

        return isSaved;
    }

    public void SavePotteryEx()
    {
        string potteryName = "workingPottery";
        PrefabUtility.SaveAsPrefabAsset(this.gameObject, "Assets/SavedPottery/" + potteryName + ".prefab");
    }
}
