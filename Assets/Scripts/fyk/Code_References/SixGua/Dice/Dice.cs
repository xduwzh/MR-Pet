using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : Singleton<Dice>
{
    public Dictionary<int, Vector3> D_NumToRotation = new Dictionary<int, Vector3>();
    private bool isRotate = false;
    private Vector3 RotateAngle;
    public int RotateSpeed = 180;


    // Start is called before the first frame update
    void Start()
    {
        D_NumToRotation.Add(1, new Vector3(-30, 55, -35));
        D_NumToRotation.Add(2, new Vector3(30, 125, -215));
        D_NumToRotation.Add(3, new Vector3(30, -125, 215));
        D_NumToRotation.Add(4, new Vector3(-30, -55, 35));
        D_NumToRotation.Add(5, new Vector3(30, -125, 35));
        D_NumToRotation.Add(6, new Vector3(-30, -55, 215));
        D_NumToRotation.Add(7, new Vector3(-30, 55, -215));
        D_NumToRotation.Add(8, new Vector3(30, 125, -35));
    }

    // Update is called once per frame
    void Update()
    {
        if (isRotate == true)
        {
            this.transform.Rotate(RotateAngle * Time.deltaTime * RotateSpeed);
        }
    }

    public void GoRotate()
    {
        RotateAngle = new Vector3(25, 45, 10);
        isRotate = true;
    }
    public int GetResult()
    {
        isRotate = false;
        int result = Random.Range(1, 8);
        this.transform.localEulerAngles = D_NumToRotation[result];
        return result;
    }


}
