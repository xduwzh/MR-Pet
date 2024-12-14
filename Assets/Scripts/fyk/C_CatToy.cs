using Meta.XR.ImmersiveDebugger.UserInterface;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class C_CatToy : MonoBehaviour
{
    public GameObject Pet;
    public Transform debugPanel;

    public AudioClip toySound;
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
        debugPanel.GetComponent<TextMeshProUGUI>().text = other.name;
        //debugPanel.GetComponent<TextMeshProUGUI>().text = other.gameObject.name;
        if (other.gameObject.name == "FLOOR_EffectMesh")
        {
            Pet.GetComponent<C_cat>().startGame();
        }
        else if (other.gameObject.name == "PinchPointRange")
        {
            AudioSource.PlayClipAtPoint(toySound, this.transform.position);
            Pet.GetComponent<C_cat>().StartPlayGame(this.gameObject);
        }
    }
}
