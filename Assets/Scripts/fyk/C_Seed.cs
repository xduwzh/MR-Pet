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

    public GameObject[] Trees;
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
            int index = Random.Range(0, Trees.Length);
            GameObject tree = Instantiate(Trees[index], this.transform.position, Quaternion.identity);
            tree.transform.parent = generateParent;
            Destroy(this.transform.parent.gameObject);
        }
    }


}
