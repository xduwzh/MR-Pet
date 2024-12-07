using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_button1 : MonoBehaviour
{
    public GameObject food;
    public GameObject Pet1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void clicked()
    {
        food.SetActive(true);
        //Pet1.GetComponent<GeckoController_Full>().target = food.transform;
    }
}
