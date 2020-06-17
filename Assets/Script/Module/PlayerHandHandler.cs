using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Valve.VR.Extras;
using UnityEngine.SceneManagement;

public class PlayerHandHandler : MonoBehaviour
{

    public GameObject curClick;
    public GameObject beforeClick;

    [Space]
    public GameObject UI;
    public GameObject arrow;

    public SteamVR_LaserPointer leftPointer;
    public SteamVR_LaserPointer rightPointer;

    private ExhibitionTable table;

    // Start is called before the first frame update
    void Start()
    {
        leftPointer.PointerIn += PointerInside;
        leftPointer.PointerOut += PointerOutside;
        leftPointer.PointerClick += PointerClick;

        rightPointer.PointerIn += PointerInside;
        rightPointer.PointerOut += PointerOutside;
        rightPointer.PointerClick += PointerClick;

        table = GameObject.Find("MainScript").GetComponent<ExhibitionTable>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            Debug.Log("Input G");
            DoInteract();
        }
    }

    public void PointerClick(object sender, PointerEventArgs e)
    {
        beforeClick = curClick;
        curClick = e.target.gameObject;
        DoInteract();

    }

    public void PointerInside(object sender, PointerEventArgs e)
    {
    }

    public void PointerOutside(object sender, PointerEventArgs e)
    {
        Debug.Log("pointer exited: " + e.target.name);
    }

    public void DoInteract()
    {
        Debug.Log("Call DoInteract");

        if (beforeClick != null && curClick != null)
        {
            // 도자기 삭제
            if (curClick.tag == "Interactor" && beforeClick.name == "delete")
            {
                Debug.Log("Do Delete");
                table.DeleteDojagi(curClick);
                ClearClickObject();
            }

            // 도자기 위치 변경
            else if (curClick.tag == "Interactor" && beforeClick.tag == "Interactor")
            {
                Debug.Log("Do Locate");
                table.ChangeLocation(beforeClick, curClick);
                ClearClickObject();
                EffectManager.instance.Play("button1");
            }

            // 갤러리 퇴장
            else if (curClick.name == "exit")
            {
                Debug.Log("Do Exit");
                SceneManager.LoadScene(sceneName: "StartScene");
            }

        }
        else if (curClick != null)
        {
            // 갤러리 퇴장
            if (curClick.name == "exit")
            {
                Debug.Log("Do Exit");
                SceneManager.LoadScene(sceneName: "StartScene");
            }
        }

        // 클릭한 물체
        if (curClick != null && (curClick.tag == "Interactor" || curClick.tag == "UI") )
        {
            arrow.transform.parent = curClick.transform;
            arrow.transform.localPosition = Vector3.zero;
            arrow.transform.localRotation = Quaternion.identity;
        }
        else
        {
            arrow.transform.parent = UI.transform;
            arrow.transform.localPosition = Vector3.zero;
        }
    }

    private void ClearClickObject()
    {
        curClick = null;
        beforeClick = null;
    }
}
