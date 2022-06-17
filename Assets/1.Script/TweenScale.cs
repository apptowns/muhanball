using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenScale : MonoBehaviour
{
    public Transform scale;
    public SpriteRenderer image;

    // Start is called before the first frame update
    void Start()
    {
        scale = this.transform;
        image = this.GetComponent<SpriteRenderer>();
    }
}
