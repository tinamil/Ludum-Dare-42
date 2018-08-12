using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrinkOverTime : MonoBehaviour
{

    public float minSize = .1f;
    public float timeToMin = 480;

    private float startTime;

    void Start()
    {

        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        var time = Time.time - startTime;
        if (time <= timeToMin)
        {
            transform.localScale = Vector3.Lerp(Vector3.one, new Vector3(minSize, 1, minSize), time / timeToMin);
        }
    }
}
