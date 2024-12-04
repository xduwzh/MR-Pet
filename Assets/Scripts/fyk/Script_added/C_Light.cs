using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class C_Light : MonoBehaviour
{
    public Transform[] positionList;
    private Transform target;
    public float moveSpeed;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        while (true)
        {
            foreach (var movePoint in positionList)
            {
                target = movePoint;
                yield return new WaitForSeconds(1.35f);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
    }
}
