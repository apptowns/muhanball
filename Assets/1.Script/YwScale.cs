using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YwScale : MonoBehaviour
{
    public GameObject obj;
    Vector2 v;

    public bool ismove;
    public bool isloop;
    float x, y, z;

    float size;

    private void Start()
    {
        obj = this.gameObject;
        v = obj.transform.localScale;
        size = 1.3f;
        ismove = false;
        isloop = true;
    }

    public void ScaleStart()
    {
        ismove = true;
    }

    void Update()
    {        
        if (ismove)
            return;
        
        x = size;
        y = size;
        z = size;

        obj.transform.localScale = new Vector3(x, y,z);

        if (size > 1.0f)
        {
            size -= 0.4f  * Time.deltaTime;
        }
        else 
        {
            if (isloop)
                size = 1.3f;
            else
                size = 1.0f;
            //ismove = false;
        }
    }
}
