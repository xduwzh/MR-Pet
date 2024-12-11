using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_ButtonControllers : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Lizard;
    //cat things
    public GameObject CatPrefab;
    public GameObject light;
    //dog things
    public GameObject DogPrefab;
    public GameObject dogBone;

    private GameObject currentPet;
    private int usingPetIndex = -1;
    private int petCount = 3;

    public Transform UIManager;
    void Start()
    {
        currentPet = Lizard;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void clicked()
    {
        //GeneralFood.SetActive(true);
        //Lizard.GetComponent<GeckoController_Full>().target = GeneralFood.transform;
    }

    public void changePet()
    {
        ReplacePet((usingPetIndex + 1) % petCount);
    }

    private void CreatePet()
    {
        //currentPet = Instantiate(Lizard, transform.position, Quaternion.identity);
        Lizard.SetActive(true);
        //usingPetIndex = 1;
    }

    private void ReplacePet(int index)
    {
        switch (index)
        {
            case (0):
                Lizard.SetActive(true); DogPrefab.SetActive(false);dogBone.SetActive(false); usingPetIndex = 0;
                UIManager.GetComponent<C_UIManager>().ChangePet(PetType.Lizard);
                break;
            case (1):
                CatPrefab.SetActive(true);light.SetActive(true);Lizard.SetActive(false); usingPetIndex = 1;
                UIManager.GetComponent<C_UIManager>().ChangePet(PetType.Cat);
                break;
            case (2):
                DogPrefab.SetActive(true);dogBone.SetActive(true); CatPrefab.SetActive(false); light.SetActive(false); usingPetIndex = 2;
                UIManager.GetComponent<C_UIManager>().ChangePet(PetType.Dog);
                break;
        }
    }
    private void GenerateFood()
    {

    }

    private void DeletePet()
    {
        if (currentPet != null)
        {
            Destroy(currentPet);
            currentPet = null;
        }
    }
}
