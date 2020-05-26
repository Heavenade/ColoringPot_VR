using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningWheel : MonoBehaviour
{
    // reference
    Transform spinningArea;

    // variables
    float rotationSpeed = 0.0f;
    float rotationAccelation = 0.0f;

    private void Awake()
    {
        spinningArea = this.transform.Find("SpinningArea");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.R))
        {
            rotationAccelation += 0.006f;
            if (rotationAccelation >= 5)
            {
                Debug.Log("종단속도 도달했음.");
                rotationAccelation = 5;
            }
        } else
        {
            rotationAccelation -= 0.006f;
            if (rotationAccelation <= 0) rotationAccelation = 0;
        }

        //rotationSpeed += rotationAccelation;
        spinningArea.Rotate(new Vector3(0, -rotationAccelation, 0));
        

    }
}
