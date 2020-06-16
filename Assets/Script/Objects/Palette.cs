using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Palette : MonoBehaviour
{
    public Transform brush;
    public PotteryColoring pottery;
    public int colorNum;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "ColoringScene")
        {
            SphereCollider collider = this.GetComponent<SphereCollider>();
            float distance = Vector3.Distance(this.transform.position + collider.center, brush.transform.position);
            if (distance < collider.radius / 4)
            {
                pottery.colorNum = colorNum;
                Debug.Log("color set: " + colorNum);
            }
        }
    }
}
