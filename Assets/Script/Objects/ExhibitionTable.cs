using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using Valve.VR.InteractionSystem;
using Valve.VR;
using UnityEngine.Rendering;

class Dojagi
{
    public string fileName;
    public GameObject gameobject;
    public int location;    // -1일 시, 창고에 박아둠
}

public class ExhibitionTable : MonoBehaviour
{
    // 갤러리에서 보이는 각 조형물의 객체
    // 
    // 기능
    // - 조형물이 존재할 경우, 조형물을 씬 안에 생성함
    // - 조형물의 삭제 기능
    // - 조형물의 위치 이동 기능
    //

    public SteamVR_Action_Vector2 touchpadControl;
    public float moveSpeed = 2.0f;

    public GameObject cube;
    private string dojagiPath = "Assets/SavedPottery/";
    private string savePath = "Assets/SaveGallery/";

    private List<GameObject> dojagiPrefabs;
    private List<Dojagi> dojagiInfos;

    private GameObject player;
    private string[] datas;
    private GameObject tables; // 각 도자기들의 부모는 table. table 들의 부모는 tables.

    // MonoBehavior
    private void Awake()
    {

        dojagiPrefabs = new List<GameObject>();
        dojagiInfos = new List<Dojagi>();
        tables = GameObject.Find("Tables");
        player = GameObject.Find("Player");

        InitSaveInfos();
        InitDisplayDojagi();
    }

    private void Update()
    {
        // 플레이어 이동
        Vector2 t = touchpadControl.GetAxis(SteamVR_Input_Sources.Any);
        if (t != Vector2.zero)
        {
            Vector3 dir = Camera.main.transform.localRotation * Vector3.forward;
            Vector3 dirt = new Vector3(dir.x, 0, dir.z);

            if (t.y >= 0)
            {
                //바라보는 시점 방향으로 이동합니다.
                player.transform.position += dirt * moveSpeed * Time.deltaTime;
            }
            else
            {
                player.transform.position += -dirt * moveSpeed * Time.deltaTime;
            }
        }
    }

    // Private Func
    private void InitSaveInfos()
    {
        // 파일 path 확인
        if (string.IsNullOrEmpty(dojagiPath))
        {
            Debug.LogError("ExhibitionTable Path is NULL: " + dojagiPath);
            return;
        }

        if (string.IsNullOrEmpty(savePath))
        {
            Debug.LogError("ExhibitionTable Path is NULL: " + savePath);
            return;
        }

        DirectoryInfo di = new DirectoryInfo(dojagiPath);
        if (di.Exists == false)
        {
            Debug.LogError("ExhibitionTable Dict is Null");
            return;
        }

        // 도자기 파일들 불러오기
        List<FileInfo> fi = di.GetFiles().ToList();
        for (int i = 0; i < fi.Count; i++)
        {
            if (fi[i].Name.Contains(".meta")) continue;
            else if (fi[i].Name.EndsWith(".prefab"))
            {
                string fileName = fi[i].Name.Replace(".prefab", "");
                GameObject a = AssetDatabase.LoadAssetAtPath<GameObject>(dojagiPath + fileName + ".prefab");
                Mesh b = AssetDatabase.LoadAssetAtPath<Mesh>(dojagiPath + fileName + ".asset");
                a.transform.localPosition = Vector3.zero;
                a.GetComponent<MeshFilter>().mesh = b;
                dojagiPrefabs.Add(a);
            }
        }

        // 세이브 파일 불러오기
        datas = System.IO.File.ReadAllLines(savePath + "save.txt");
        for (int i = 0; i < datas.Length; i++)
        {
            string[] data = datas[i].Split(',');

            Dojagi temp = new Dojagi();
            temp.fileName = data[0].ToString();
            temp.location = int.Parse(data[1].ToString());
            dojagiInfos.Add(temp);

        }

    }

    private void InitDisplayDojagi()
    {
        // 도자기들을 실제로 배치하는 함수
        for (int i = 0; i < dojagiPrefabs.Count; i++)
        {
            GameObject target = Instantiate(dojagiPrefabs[i]);
            target.name = target.name.Replace("(Clone)", "");

            foreach (Dojagi dt in dojagiInfos)
            {
                if (dt.fileName == target.name)
                {
                    if (dt.location == -1) continue;

                    Destroy(tables.transform.GetChild(dt.location).GetChild(0).gameObject);

                    target.transform.parent = tables.transform.GetChild(dt.location);
                    target.transform.localPosition = Vector3.zero;
                    target.transform.localScale = Vector3.one;
                    target.transform.localRotation = Quaternion.identity;
                    target.tag = "Interactor";
                    target.AddComponent<MeshCollider>();
                    break;
                }

            }

        }
        foreach (Transform table in tables.transform)
        {
            if (table.childCount == 0)
            {
                Instantiate(cube, table);
            }
        }
    }

    public void DeleteDojagi(GameObject target)
    {
        // 선택한 도자기 오브젝트를 삭제하는 스크립트
        Debug.Log("Call DeleteDojagi");

        // cube는 삭제하지 않는다.
        if (target == cube) return;

        // 인자는 raycast 등으로 선택한 도자기 오브젝트
        int index = -1;
        for (int i = 0; i < dojagiInfos.Count; i++)
        {
            Debug.Log("i " + i.ToString());
            if (target.name == dojagiInfos[i].fileName)
            {
                index = i;
            }
        }

        if (index == -1)
        {
            Debug.Log("삭제할 대상이 없음");
            return;
        }

        Destroy(target);
        System.IO.File.Delete(dojagiPath + dojagiInfos[index].fileName + ".prefab");
        System.IO.File.Delete(dojagiPath + dojagiInfos[index].fileName + ".prefab.meta");

        Instantiate(cube, tables.transform.GetChild(dojagiInfos[index].location));
        dojagiInfos.RemoveAt(index);
    }

    public void ChangeLocation(GameObject targetA, GameObject targetB)
    {
        // 두 Interactor 오브젝트 간 위치를 바꾼다.
        Debug.Log("Call ChangeLocation");

        Transform aParent = targetA.transform.parent;
        Transform bParent = targetB.transform.parent;
        Vector3 aLocalPos = targetA.transform.localPosition;
        Vector3 bLocalPos = targetB.transform.localPosition;

        //
        targetB.transform.SetParent(aParent);
        targetA.transform.SetParent(bParent);
        targetA.transform.localPosition = aLocalPos;
        targetB.transform.localPosition = bLocalPos;
        targetA.transform.localRotation = Quaternion.identity;
        targetB.transform.localRotation = Quaternion.identity;
    }

}