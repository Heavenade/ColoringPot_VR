using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using UnityEngine;
using UnityEditor;
using Valve.VR;

public class PotteryMesh : MonoBehaviour
{
    Mesh mesh;
    Vector3[] vertices;
    int[] triangles;
    float[] radius;

    static int verticesFloorNum = 100; //number of vertices floor - 1
    static int verticesPerFloor = 20; //number of vertices per floor
    public float eachHeight = 0.01f; //height between two vertice floors

    float angle = 360f / (float)verticesPerFloor;
    double radian = Math.PI / 180;

    public float defaultRadius = 0.5f;

    int innerVerticesNum = verticesPerFloor * (verticesFloorNum + 1);
    int innerTrianglesNum = 3 * verticesPerFloor * 2 * verticesFloorNum;
    int bottomTrianglesNum = 3 * verticesPerFloor * 2;

    public SteamVR_Action_Boolean interactionUI;


    //static float x = 0.5f, y = 0.5f, z = 0.5f;

    public string potteryName;
    public Transform craftPointer;
    public Transform leftHand;
    public Transform rightHand;
    public Transform debugLeft;
    public Transform debugRight;
    private Transform[] debugLeftPointers;
    private Transform[] debugRightPointers;

    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        vertices = new Vector3[innerVerticesNum * 2 + 2];
        debugLeftPointers = new Transform[31];
        debugRightPointers = new Transform[31];
        InitializeRadius();
        SetVertices();
        CreateTriangles();
        DrawMesh();

        for (int i = 0; i < 31; i++)
        {
            debugLeftPointers[i] = Instantiate(craftPointer, new Vector3(0, 0, 0), Quaternion.identity);
            debugLeftPointers[i].transform.parent = debugLeft.transform;
            debugRightPointers[i] = Instantiate(craftPointer, new Vector3(0, 0, 0), Quaternion.identity);
            debugRightPointers[i].transform.parent = debugRight.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("f"))
        {
            SaveAsset();
        }

        // SteamVR의 Interact UI 버튼을 누르고 있을 시에만 작동하게 합니다.
        bool isPressingTrigger = interactionUI.GetState(SteamVR_Input_Sources.Any);
        if (isPressingTrigger)
        {
            MoldPottery(leftHand);
            MoldPottery(rightHand);
        }
    }

    void MoldPottery(Transform hand)
    {
        Vector3 handPosition;
        SteamVR_Behaviour_Skeleton skeleton;
        Transform[] debugPointers;
        try
        {
            if (hand == leftHand)
            {
                skeleton = hand.Find("LeftRenderModel(Clone)").Find("vr_glove_left(Clone)").GetComponent<SteamVR_Behaviour_Skeleton>();
                debugPointers = debugLeftPointers;
            }
            else
            {
                skeleton = hand.Find("RightRenderModel(Clone)").Find("vr_glove_right(Clone)").GetComponent<SteamVR_Behaviour_Skeleton>();
                debugPointers = debugRightPointers;
            }
            for (int i = 1; i < 26; i++)
            {
                handPosition = skeleton.GetBonePosition(i);
                float x = handPosition.x - this.transform.position.x;
                float y = handPosition.y - this.transform.position.y;
                float z = handPosition.z - this.transform.position.z;
                int floor = (int)(y * (1 / eachHeight));

                // debug
                //debugPointers[i].position = new Vector3(handPosition.x, handPosition.y, handPosition.z);

                if (floor < 0 || floor > verticesFloorNum || (float)Math.Sqrt(x * x + z * z) <= defaultRadius * 0.6)
                {
                    continue;
                }
                else if (radius[floor] > (float)Math.Sqrt(x * x + z * z))
                {
                    SetRadius(x, y, z);
                    SetVertices();
                    DrawMesh();
                }
            }
        }
        catch
        {
        }
    }

    void InitializeRadius()
    {
        radius = new float[verticesFloorNum + 1];
        for (int i = 0; i <= verticesFloorNum; i++)
        {
            radius[i] = defaultRadius;
        }
    }

    //need to fix pottery bottom
    void SetVertices()
    {

        //outside
        for (int i = 0; i <= verticesFloorNum; i++)
        {
            for (int j = 0; j < verticesPerFloor; j++)
            {
                vertices[i * verticesPerFloor + j] = new Vector3((float)Math.Sin(angle * j * radian) * radius[i], (float)i * eachHeight, (float)Math.Cos(angle * j * radian) * radius[i]);
            }
        }

        //inside
        for (int i = 0; i <= verticesFloorNum; i++)
        {
            for (int j = 0; j < verticesPerFloor; j++)
            {
                vertices[i * verticesPerFloor + j + innerVerticesNum] = new Vector3((float)Math.Sin(angle * j * radian) * (radius[i] * 0.6f), (float)i * eachHeight, (float)Math.Cos(angle * j * radian) * (radius[i] * 0.6f));
            }
        }

        vertices[2 * innerVerticesNum] = new Vector3(0f, 0f, 0f);
        vertices[2 * innerVerticesNum + 1] = new Vector3(0f, eachHeight, 0f);
    }

    void SetRadius(float x, float y, float z)
    {
        //need to be changed for VR input
        //need more exeption check

        for (int i = 0; i <= (int)((defaultRadius - (float)Math.Sqrt(x * x + z * z)) * (int)(1 / eachHeight)); i++)
        {
            if ((int)(y * (int)(1 / eachHeight)) + i > verticesFloorNum)
            {
                break;
            }

            if (radius[(int)(y * (int)(1 / eachHeight)) + i] > (float)Math.Sqrt(x * x + z * z) + eachHeight * (float)i)
            {
                if ((float)Math.Sqrt(x * x + z * z) + eachHeight * (float)i > defaultRadius * 0.4)
                {
                    radius[(int)(y * (int)(1 / eachHeight)) + i] = (float)Math.Sqrt(x * x + z * z) + eachHeight * (float)i;
                }
            }
        }
        for (int i = 0; i <= (int)((defaultRadius - (float)Math.Sqrt(x * x + z * z)) * (int)(1 / eachHeight)); i++)
        {
            if ((int)(y * (int)(1 / eachHeight)) - i < 0)
            {
                break;
            }

            if (radius[(int)(y * (int)(1 / eachHeight)) - i] > (float)Math.Sqrt(x * x + z * z) + eachHeight * (float)i)
            {
                if ((float)Math.Sqrt(x * x + z * z) + eachHeight * (float)i > defaultRadius * 0.4)
                {
                    radius[(int)(y * (int)(1 / eachHeight)) - i] = (float)Math.Sqrt(x * x + z * z) + eachHeight * (float)i;
                }
            }
        }
    }

    void CreateTriangles()
    {
        triangles = new int[(innerTrianglesNum + bottomTrianglesNum) * 2];

        //ouside
        for (int i = 0; i < verticesFloorNum; i++)
        {
            for (int j = 0; j < verticesPerFloor; j++)
            {
                triangles[6 * j + 6 * verticesPerFloor * i] = verticesPerFloor * i + j;
                triangles[6 * j + 6 * verticesPerFloor * i + 1] = verticesPerFloor * i + (j + 1) % verticesPerFloor;
                triangles[6 * j + 6 * verticesPerFloor * i + 2] = verticesPerFloor * i + j + verticesPerFloor;

                triangles[6 * j + 6 * verticesPerFloor * i + 3] = triangles[6 * j + 6 * verticesPerFloor * i + 2];
                triangles[6 * j + 6 * verticesPerFloor * i + 4] = triangles[6 * j + 6 * verticesPerFloor * i + 1];
                triangles[6 * j + 6 * verticesPerFloor * i + 5] = verticesPerFloor * i + verticesPerFloor + (j + 1) % verticesPerFloor;

            }

        }

        //inside
        for (int i = 0; i < verticesFloorNum; i++)
        {
            for (int j = 0; j < verticesPerFloor; j++)
            {
                triangles[6 * j + 6 * verticesPerFloor * i + innerTrianglesNum] = verticesPerFloor * i + j + innerVerticesNum;
                triangles[6 * j + 6 * verticesPerFloor * i + 1 + innerTrianglesNum] = verticesPerFloor * i + j + verticesPerFloor + innerVerticesNum;
                triangles[6 * j + 6 * verticesPerFloor * i + 2 + innerTrianglesNum] = verticesPerFloor * i + (j + 1) % verticesPerFloor + innerVerticesNum;

                triangles[6 * j + 6 * verticesPerFloor * i + 3 + innerTrianglesNum] = triangles[6 * j + 6 * verticesPerFloor * i + 2 + innerTrianglesNum];
                triangles[6 * j + 6 * verticesPerFloor * i + 4 + innerTrianglesNum] = triangles[6 * j + 6 * verticesPerFloor * i + 1 + innerTrianglesNum];
                triangles[6 * j + 6 * verticesPerFloor * i + 5 + innerTrianglesNum] = verticesPerFloor * i + verticesPerFloor + (j + 1) % verticesPerFloor + innerVerticesNum;

            }

        }

        //top
        for (int i = 0; i < verticesPerFloor; i++)
        {
            triangles[6 * i + 2 * innerTrianglesNum] = verticesPerFloor * verticesFloorNum + i;
            triangles[6 * i + 1 + 2 * innerTrianglesNum] = verticesPerFloor * verticesFloorNum + (i + 1) % verticesPerFloor;
            triangles[6 * i + 2 + 2 * innerTrianglesNum] = verticesPerFloor * (verticesFloorNum * 2 + 1) + (i + 1) % verticesPerFloor;

            triangles[6 * i + 3 + 2 * innerTrianglesNum] = triangles[6 * i + 2 * innerTrianglesNum];
            triangles[6 * i + 4 + 2 * innerTrianglesNum] = triangles[6 * i + 2 + 2 * innerTrianglesNum];
            triangles[6 * i + 5 + 2 * innerTrianglesNum] = verticesPerFloor * (verticesFloorNum * 2 + 1) + i;
        }


        //buttom(need to be change..maybe?)
        for (int i = 0; i < verticesPerFloor; i++)
        {
            triangles[6 * i + 2 * innerTrianglesNum + bottomTrianglesNum] = 2 * innerVerticesNum;
            triangles[6 * i + 1 + 2 * innerTrianglesNum + bottomTrianglesNum] = (i + 1) % verticesPerFloor;
            triangles[6 * i + 2 + 2 * innerTrianglesNum + bottomTrianglesNum] = i;

            triangles[6 * i + 3 + 2 * innerTrianglesNum + bottomTrianglesNum] = verticesPerFloor * (verticesFloorNum + 3) + (i + 1) % verticesPerFloor;
            triangles[6 * i + 4 + 2 * innerTrianglesNum + bottomTrianglesNum] = 2 * innerVerticesNum + 1;
            triangles[6 * i + 5 + 2 * innerTrianglesNum + bottomTrianglesNum] = verticesPerFloor * (verticesFloorNum + 3) + i;

        }

    }

    void DrawMesh()
    {
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        GetComponent<MeshFilter>().mesh = mesh;
        mesh.RecalculateNormals();
    }

    void SaveAsset()
    {
        AssetDatabase.CreateAsset(mesh, "Assets/SavedPottery/" + potteryName + ".asset");
    }
}
