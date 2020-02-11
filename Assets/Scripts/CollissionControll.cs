using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollissionControll : MonoBehaviour
{
    float delay = 0f;
    private bool colorShouldChange = false;

    void Awake()
    {
        transform.GetComponent<Renderer>().material.color = Color.white;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            colorShouldChange = true;
            delay = Time.time + 0.3f;
        }
    }

    void ColorChange()
    {
        if (colorShouldChange)
        {
            transform.GetComponent<Renderer>().material.color = Color.red;
            if(Time.time > delay)
            {
                transform.GetComponent<Renderer>().material.color = Color.white;
                colorShouldChange = false;
            }
        }
    }

    void Update()
    {
        ColorChange();
    }
}
