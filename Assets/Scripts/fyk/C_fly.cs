using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_fly : MonoBehaviour
{
    public Transform target;
    public float speed = 100.0f;

    public float amplitude = 0.001f;  // 运动幅度
    public float frequency = 5.0f;

    private void Start()
    {

    }

    private void Update()
    {
        float offset = Mathf.Sin(frequency * Time.time) * amplitude;
        this.transform.RotateAround(target.transform.position, Vector3.up, speed * Time.deltaTime);
        this.transform.position += new Vector3(0, offset, 0); 
    }
}
