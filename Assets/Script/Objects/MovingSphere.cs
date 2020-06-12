using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingSphere : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 position = this.transform.position;
        if (Input.GetKey(KeyCode.D))
        {
            position.x = position.x + 0.01f;
        }

        if (Input.GetKey(KeyCode.A))
        {
            position.x = position.x - 0.01f;
        }

        if (Input.GetKey(KeyCode.W))
        {
            position.y = position.y + 0.01f;
        }

        if (Input.GetKey(KeyCode.S))
        {
            position.y = position.y - 0.01f;
        }

        if (Input.GetKey(KeyCode.Q))
        {
            position.z = position.z - 0.01f;
        }

        if (Input.GetKey(KeyCode.E))
        {
            position.z = position.z + 0.01f;
        }
        this.transform.position = position;
    }
}
