using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffAfterSecond : MonoBehaviour
{
    float t = 5f;

    // Start is called before the first frame update
    void OnEnable()
    {
        
        Invoke("ActiveShow", t);

    }

    private void ActiveShow()
    {
        this.gameObject.SetActive(false);

    }
}
