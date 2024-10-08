using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{
    public float moveAmount;    //移动速度
    public Camera myCamera;	//摄像机
    public bool edgeScrolling = true;	//移动开关

    private PlayerInput m_PlayerInput;
    private Vector2 wasdInputVec2;
    private Vector3 cameraPos;

    public static bool isCameraMove = false;

    //Rotate
    public enum RotationAxes
    {
        MouseXAndY = 0,
        MouseX = 1,
        MouseY = 2
    }
    public RotationAxes axes = RotationAxes.MouseXAndY;
    public float sensitivityHor = 1f;
    public float sensitivityVert = 1f;

    public float minmumVert = -45f;
    public float maxmumVert = 45f;

    private float _rotationX = 0;

    public float edgeSize;	//会产生移动效果的边缘宽度
    private Vector3 camFollowPos;	//用于给摄像机赋值
    void Start()
    {
        camFollowPos = myCamera.transform.position;
        //m_PlayerInput = GetComponent<PlayerInput>();

        //m_PlayerInput.onActionTriggered += callBack =>
        //{
        //    if (callBack.action.name == "CameraMovement")
        //    {
        //        wasdInputVec2 = callBack.ReadValue<Vector2>();
        //        cameraPos = myCamera.transform.position;
        //        Debug.Log(wasdInputVec2);
        //    }
        //};
        //Cursor.lockState = CursorLockMode.Locked;//锁定指针到视图中心
        //Cursor.visible = false;//隐藏指针

    }

    // Update is called once per frame
    void Update()
    {
        //if(wasdInputVec2 != Vector2.zero)
        //{
        //    Vector3 moveDirection = new Vector3(wasdInputVec2.x, 0, wasdInputVec2.y);
        //    cameraPos += moveDirection * Time.deltaTime * moveAmount;
        //    myCamera.transform.position = cameraPos;
        //}
        //if (axes == RotationAxes.MouseX)
        //{
        //    transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityHor, 0);
        //}
        //else if (axes == RotationAxes.MouseY)
        //{
        //    _rotationX = _rotationX - Input.GetAxis("Mouse Y") * sensitivityVert;
        //    _rotationX = Mathf.Clamp(_rotationX, minmumVert, maxmumVert);

        //    float rotationY = transform.localEulerAngles.y;

        //    transform.localEulerAngles = new Vector3(_rotationX, rotationY, 0);
        //}
        //else
        //{
        //    _rotationX -= Input.GetAxis("Mouse Y") * sensitivityVert;
        //    _rotationX = Mathf.Clamp(_rotationX, minmumVert, maxmumVert);

        //    float delta = Input.GetAxis("Mouse X") * sensitivityHor;
        //    float rotationY = transform.localEulerAngles.y + delta;

        //    transform.localEulerAngles = new Vector3(_rotationX, rotationY, 0);
        //}
        //屏幕左下角为坐标(0, 0)
        if (Input.mousePosition.x > Screen.width - edgeSize)//如果鼠标位置在右侧
        {
            camFollowPos.x += moveAmount * Time.deltaTime;//就向右移动
        }
        if (Input.mousePosition.x < edgeSize)
        {
            camFollowPos.x -= moveAmount * Time.deltaTime;
        }
        if (Input.mousePosition.y > Screen.height - edgeSize)
        {
            camFollowPos.z += moveAmount * Time.deltaTime;
        }
        if (Input.mousePosition.y < edgeSize)
        {
            camFollowPos.z -= moveAmount * Time.deltaTime;
        }
        if (Input.mouseScrollDelta.x > 0)
        {
            Debug.Log(Input.mouseScrollDelta.x + "," + Input.mouseScrollDelta.y);
        }
        myCamera.transform.position = camFollowPos;//刷新摄像机位置
    }
}
