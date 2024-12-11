using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_ThrowableObj : MonoBehaviour
{
    private Rigidbody _rigidbody;
    public Transform parentObj;
    public Transform UIManager;
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = parentObj.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //if(_rigidbody.isKinematic == false)
        //{
        //    UIManager.GetComponent<C_UIManager>().CubeDebugger();
        //    _rigidbody.useGravity = true;
        //}
    }

    public void SetGravity()
    {
        _rigidbody.isKinematic = false ;
        _rigidbody.useGravity = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("hand"))
        {
            SetGravity();
        }
    }
}
