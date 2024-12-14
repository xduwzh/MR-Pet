using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PetInfo
{
    public string name;
    public float hunger;
    public float comfort;
    public float mood;
    public Transform Prefab;
    public Sprite imageSource;

    public PetInfo(string n, float h, float c, float m,Transform p, Sprite i)
    {
        this.name = n;
        this.hunger = h;
        this.comfort = c;
        this.mood = m;
        this.Prefab = p;
        this.imageSource = i;
    }
}
public enum PetType 
{ 
    Lizard,
    Cat,
    Dog
}

public class C_UIManager : MonoBehaviour
{
    // Start is called before the first frame update
    [HideInInspector]
    public Dictionary<PetType, PetInfo> Pets = new Dictionary<PetType, PetInfo>();
    [HideInInspector]
    public PetInfo curPet;
    [HideInInspector]
    public PetType curPetType;

    public Transform hungerBar;
    public Transform comfortBar;
    public Transform moodBar;
    public Transform petName;
    public Transform petImage;

    public Transform DogPrefab;
    public Transform CatPrefab;
    public Transform LizardPrefab;

    [Header("Images")]
    public Sprite LizardImage;
    public Sprite CatImage;
    public Sprite DogImage;

    [Header("FoodPrefab")]
    public GameObject CatFoodPrefab;
    public GameObject DogFoodPrefab;
    public GameObject LizardFoodPrefab;

    [Header("CleaningProps")]
    public GameObject brush;
    public GameObject sprayBottle;
    public Transform brushPosition;
    public Transform sprayBottlePosition;

    public GameObject[] DogBubbles;
    public GameObject[] CatBubbles;
    public GameObject[] LizadBubbles;

    public Transform foodGeneratePosition;

    //变换宠物时的变量
    private int usingPetIndex = -1;
    private int petCount = 3;

    [Header("Debugger")]
    public GameObject debugger;

    [Header("Seed")]
    public GameObject SeedPrefab;
    public Transform SeedGeneratePosition;

    public Transform SeedsParent;
    public Transform TreesParent;
    public Transform FoodParent;

    public Transform SeedsBowl;

    [Header("ParticleEffect")]
    public GameObject HeartsEffect;

    public Transform DebugPanel;

    public AudioClip foodGenerate;
    public AudioClip Change;
    public AudioClip MoodIncrease;

    [Header("PlayGame")]
    public GameObject DogToy;
    public GameObject CatToy;
    public GameObject LizardToy;

    void Start()
    {
        Pets.Add(PetType.Lizard, new PetInfo("YellowLizard", 100.0f, 100.0f, 100.0f, LizardPrefab, LizardImage));
        Pets.Add(PetType.Cat, new PetInfo("BlackCat", 90.0f, 90.0f, 90.0f, CatPrefab, CatImage));
        Pets.Add(PetType.Dog, new PetInfo("GoldenRetriever", 80.0f, 80.0f, 80.0f, DogPrefab, DogImage));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateFood()
    {
        if(FoodParent.gameObject.active == false)
        {
            FoodParent.gameObject.SetActive(true);
        }
        GameObject tmp = new GameObject();
        switch (usingPetIndex)
        {
            case (0):
                tmp = Instantiate(LizardFoodPrefab, foodGeneratePosition.position, Quaternion.identity);
                LizardPrefab.GetComponent<GeckoController_Full>().target = tmp.transform;
                break;
            case (1):
                tmp = Instantiate(CatFoodPrefab, foodGeneratePosition.position, Quaternion.identity);
                Vector3 lookPositionCat = new Vector3(tmp.transform.position.x, CatPrefab.transform.position.y, tmp.transform.position.z);
                CatPrefab.transform.LookAt(lookPositionCat);
                if (CatPrefab.GetComponent<C_cat>().isSleeping)
                {
                    CatPrefab.GetComponent<C_cat>().showFood(tmp.transform.position);
                }
                break;
            case (2):
                tmp = Instantiate(DogFoodPrefab, foodGeneratePosition.position, Quaternion.identity);
                if (DogPrefab.GetComponent<C_cat>().currentState != DogPrefab.GetComponent<C_cat>().states[StateType.Sit])
                {
                    DogPrefab.GetComponent<C_cat>().TransitionState(StateType.Sit);
                }
                Vector3 lookPositionDog = new Vector3(tmp.transform.position.x, DogPrefab.transform.position.y, tmp.transform.position.z);
                DogPrefab.transform.LookAt(lookPositionDog);
                break;
        }
        AudioSource.PlayClipAtPoint(foodGenerate, this.transform.position);
        tmp.transform.SetParent(FoodParent);
    }
    public void cleaning()
    {
        brush.SetActive(true);
        sprayBottle.SetActive(true);
        brush.transform.position = brushPosition.position;
        sprayBottle.transform.position = sprayBottlePosition.position;
    }

    public void cleaningEnd()
    {
        brush.SetActive(false);
        sprayBottle.SetActive(false);
    }

    public void ChangePet1()
    {
        if(usingPetIndex != -1)
        {
            //curPet.Prefab.gameObject.SetActive(false);
            Pets[curPetType].Prefab.gameObject.SetActive(false);
        }
        usingPetIndex = (usingPetIndex + 1) % petCount;
        switch (usingPetIndex)
        {
            case (0):
                ChangePet(PetType.Lizard);
                break;
            case (1):
                ChangePet(PetType.Cat);
                break;
            case (2):
                ChangePet(PetType.Dog);
                break;
        }
        //curPet.Prefab.gameObject.SetActive(true);
        Pets[curPetType].Prefab.gameObject.SetActive(true);
        AudioSource.PlayClipAtPoint(Change, this.transform.position);
    }

    public void ChangePet(PetType type)
    {
        //curPet = Pets[type];
        curPetType = type;
        updateUI();
    }

    public void updateUI()
    {
        hungerBar.GetComponent<Scrollbar>().size = Pets[curPetType].hunger / 100.0f;
        comfortBar.GetComponent<Scrollbar>().size = Pets[curPetType].comfort / 100.0f;
        moodBar.GetComponent<Scrollbar>().size = Pets[curPetType].mood / 100.0f;
        petImage.GetComponent<Image>().sprite = Pets[curPetType].imageSource;
        petName.GetComponent<TextMeshProUGUI>().text = Pets[curPetType].name;
    }

    public void petEat()
    {
        switch (usingPetIndex)
        {
            case (0):
                if (Pets[curPetType].hunger < 100 && Pets[curPetType].hunger >= 80)
                {
                    Pets[curPetType].hunger = 100;
                }else if(Pets[curPetType].hunger < 80)
                {
                    Pets[curPetType].hunger += 20;
                }
                AudioSource.PlayClipAtPoint(C_AudioManager.instance.Eat, transform.position);
                //C_AudioManager.instance.PlaySound(myAudioClip);
                //C_AudioManager.instance.playSoundOnce("Eat");
                break;
            case (1):
                if (Pets[curPetType].hunger < 100 && Pets[curPetType].hunger >= 90)
                {
                    Pets[curPetType].hunger = 100;
                }
                else if (Pets[curPetType].hunger < 90)
                {
                    Pets[curPetType].hunger += 10;
                }
                AudioSource.PlayClipAtPoint(C_AudioManager.instance.Eat, transform.position);
                break;
            case (2):
                if (Pets[curPetType].hunger < 100 && Pets[curPetType].hunger >= 70)
                {
                    Pets[curPetType].hunger = 100;
                }
                else if (Pets[curPetType].hunger < 70)
                {
                    Pets[curPetType].hunger += 30;
                }
                AudioSource.PlayClipAtPoint(C_AudioManager.instance.DogEat, transform.position);
                break;
        }
        if (Pets[curPetType].mood < 100 && Pets[curPetType].mood >= 95)
        {
            Pets[curPetType].mood = 100;
        }
        else if (Pets[curPetType].mood < 95)
        {
            Pets[curPetType].mood += 5;
        }
        
        StartCoroutine(playHeartEffect(2.0f));
        hungerBar.GetComponent<Scrollbar>().size = Pets[curPetType].hunger / 100.0f;
        moodBar.GetComponent<Scrollbar>().size = Pets[curPetType].mood / 100.0f;
    }
    public void petCleaned()
    {
        switch (usingPetIndex)
        {
            case (0):
                if (Pets[curPetType].comfort < 100 && Pets[curPetType].comfort >= 80)
                {
                    Pets[curPetType].comfort = 100;
                }
                else if (Pets[curPetType].comfort < 80)
                {
                    Pets[curPetType].comfort += 20;
                }
                break;
            case (1):
                if (Pets[curPetType].comfort < 100 && Pets[curPetType].comfort >= 90)
                {
                    Pets[curPetType].comfort = 100;
                }
                else if (Pets[curPetType].comfort < 90)
                {
                    Pets[curPetType].comfort += 10;
                }
                break;
            case (2):
                if (Pets[curPetType].comfort < 100 && Pets[curPetType].comfort >= 70)
                {
                    Pets[curPetType].comfort = 100;
                }
                else if (Pets[curPetType].comfort < 70)
                {
                    Pets[curPetType].comfort += 30;
                }
                break;
        }
        if (Pets[curPetType].mood < 100 && Pets[curPetType].mood >= 90)
        {
            Pets[curPetType].mood = 100;
        }
        else if (Pets[curPetType].mood < 90)
        {
            Pets[curPetType].mood += 10;
        }
        StartCoroutine(playHeartEffect(2.0f));
        hungerBar.GetComponent<Scrollbar>().size = Pets[curPetType].comfort / 100.0f;
        moodBar.GetComponent<Scrollbar>().size = Pets[curPetType].mood / 100.0f;
    }

    public void PetGetTouched()
    {
        if (Pets[curPetType].mood < 100 && Pets[curPetType].mood >= 98)
        {
            Pets[curPetType].mood = 100;
        }
        else if (Pets[curPetType].mood < 98)
        {
            Pets[curPetType].mood += 2;
        }
        StartCoroutine(playHeartEffect(2.0f));
        moodBar.GetComponent<Scrollbar>().size = Pets[curPetType].mood / 100.0f;
    }

    public void PetPlayed()
    {
        if (Pets[curPetType].mood < 100 && Pets[curPetType].mood >= 70)
        {
            Pets[curPetType].mood = 100;
        }
        else if (Pets[curPetType].mood < 70)
        {
            Pets[curPetType].mood += 30;
        }
        StartCoroutine(playHeartEffect(2.0f));
        moodBar.GetComponent<Scrollbar>().size = Pets[curPetType].mood / 100.0f;
    }

    public void PetGetEnvironment()
    {
        if (Pets[curPetType].mood < 100 && Pets[curPetType].mood >= 90)
        {
            Pets[curPetType].mood = 100;
        }
        else if (Pets[curPetType].mood < 90)
        {
            Pets[curPetType].mood += 10;
        }
        moodBar.GetComponent<Scrollbar>().size = Pets[curPetType].mood / 100.0f;
    }

    public void GenerateBubbles()
    {
        switch (usingPetIndex)
        {
            case (0):
                foreach (GameObject j in LizadBubbles)
                {
                    j.SetActive(true);
                }
                break;
            case (1):
                foreach (GameObject j in CatBubbles)
                {
                    j.SetActive(true);
                }
                break;
            case (2):
                foreach (GameObject j in DogBubbles)
                {
                    j.SetActive(true);
                }
                break;
        }
    }
    public void GenerateOneBubble()
    {
        switch (usingPetIndex)
        {
            case (0):
                foreach (GameObject j in LizadBubbles)
                {
                    if(j.active == false)
                    {
                        j.SetActive(true);
                        break;
                    }
                }
                break;
            case (1):
                foreach (GameObject j in CatBubbles)
                {
                    if (j.active == false)
                    {
                        j.SetActive(true);
                        break;
                    }
                }
                break;
            case (2):
                foreach (GameObject j in DogBubbles)
                {
                    if (j.active == false)
                    {
                        j.SetActive(true);
                        break;
                    }
                }
                break;
        }
    }

    public void DestroyBubbles()
    {
        switch (usingPetIndex)
        {
            case (0):
                foreach (GameObject j in LizadBubbles)
                {
                    j.SetActive(false);
                }
                break;
            case (1):
                foreach (GameObject j in CatBubbles)
                {
                    j.SetActive(false);
                }
                break;
            case (2):
                foreach (GameObject j in DogBubbles)
                {
                    j.SetActive(false);
                }
                break;
        }
    }

    public void CubeDebugger()
    {
        if (debugger.active)
        {
            debugger.SetActive(false);
        }
        else
        {
            debugger.SetActive(true);
        }
    }

    public void GenerateSeed()
    {
        if(SeedsBowl.gameObject.active == false)
        {
            SeedsBowl.gameObject.SetActive(true);
        }
        if(SeedsParent.gameObject.active == false)
        {
            SeedsParent.gameObject.SetActive(true);
        }
        GameObject seed =  Instantiate(SeedPrefab, SeedGeneratePosition.position, Quaternion.identity);
        seed.transform.parent = SeedsParent;
        seed.GetComponentInChildren<C_Seed>().generateParent = TreesParent;
    }

    public void HideSeeds()
    {
        SeedsBowl.gameObject.SetActive(false);
        SeedsParent.gameObject.SetActive(false);
    }

    public void ClearScene()
    {
        cleaningEnd();
        //TreesParent.gameObject.SetActive(false);
        //FoodParent.gameObject.SetActive(false);
        for (int i = 0; i < TreesParent.childCount; i++)
        {
            Destroy(TreesParent.GetChild(i).gameObject);
        }
        for (int i = 0; i < FoodParent.childCount; i++)
        {
            Destroy(FoodParent.GetChild(i).gameObject);
        }
        DogToy.SetActive(false);
        CatToy.SetActive(false);
        LizardToy.SetActive(false);
    }

    private IEnumerator playHeartEffect(float time)
    {
        AudioSource.PlayClipAtPoint(MoodIncrease, this.transform.position);
        GameObject effect = Instantiate(HeartsEffect, foodGeneratePosition.position, Quaternion.identity);
        switch (usingPetIndex)
        {
            case (0):
                effect.transform.position = LizadBubbles[0].transform.position;
                break;
            case (1):
                effect.transform.position = CatBubbles[0].transform.position;
                break;
            case (2):
                effect.transform.position = DogBubbles[0].transform.position;
                break;
        }
        yield return new WaitForSeconds(time);
        Destroy(effect);
    }

    public void DebugEvent()
    {
        C_AudioManager.instance.playSoundOnce("DogToy");
    }

    public void PlayGame()
    {
        CatToy.SetActive(false);
        DogToy.SetActive(false);
        switch (usingPetIndex)
        {
            case (0):
                
                break;
            case (1):
                if (CatToy.active == false)
                {
                    CatToy.SetActive(true);
                }
                Pets[curPetType].Prefab.GetComponent<C_cat>().StartPlayGame(CatToy);
                break;
            case (2):
                if(DogToy.active == false)
                {
                    DogToy.SetActive(true);
                }
                Pets[curPetType].Prefab.GetComponent<C_Dog>().StartPlayGame(DogToy);
            break;
        }
    }


}
