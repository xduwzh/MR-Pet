using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_ButtonControllers : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject food;
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
        food.SetActive(true);
        Lizard.GetComponent<GeckoController_Full>().target = food.transform;
    }

    public void changePet()
    {
        //if(cat.active == false)
        //{
        //    cat.SetActive(true);
        //    light.SetActive(true);
        //    Lizard.SetActive(false);
        //}
        //else
        //{
        //    cat.SetActive(false);
        //    light.SetActive(false);
        //    Lizard.SetActive(true);
        //}

        //if (currentPet == null)
        //{
        //    CreatePet();
        //}
        //else
        //{
        //    ReplacePet((usingPetIndex + 1) % petCount);
        //}
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
                Lizard.SetActive(true);food.SetActive(true);DogPrefab.SetActive(false);dogBone.SetActive(false); usingPetIndex = 0; break;
            case (1):
                CatPrefab.SetActive(true);light.SetActive(true);Lizard.SetActive(false); food.SetActive(false); usingPetIndex = 1; break;
            case (2):
                DogPrefab.SetActive(true);dogBone.SetActive(true); CatPrefab.SetActive(false); light.SetActive(false); usingPetIndex = 2; break;
        }
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
