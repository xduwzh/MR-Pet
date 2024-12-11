using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Meta.XR.MRUtilityKit.EffectMesh;

public class C_Seed : MonoBehaviour
{
    public Transform debugPanel;
    [HideInInspector]
    public Transform generateParent;

    public GameObject Tree1;
    public GameObject Tree2;
    public GameObject Tree3;
    public GameObject Tree4;
    public GameObject Tree5;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //debugPanel.GetComponent<TextMeshProUGUI>().text = other.gameObject.name;
        if (other.gameObject.name == "FLOOR_EffectMesh")
        {
            GameObject tree = Instantiate(Tree1, this.transform.position, Quaternion.identity);
            tree.transform.parent = generateParent;
            Destroy(this.transform.parent.gameObject);
        }
    }
}
