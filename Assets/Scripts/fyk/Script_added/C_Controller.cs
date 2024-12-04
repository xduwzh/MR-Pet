using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Controller : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject catPrefab;
    public GameObject birdPrefab;
    public GameObject dogPrefab;

    private GameObject currentPet; // 当前存在的宠物
    private int usingPetIndex = -1; // 当前宠物Prefab的状态
    private int petCount = 3;

    void Start()
    {

    }

    void Update()
    {
        // 按下G键创建或替换宠物
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (currentPet == null)
            {
                // 如果场景中不存在宠物，创建一个新的宠物
                CreatePet();
            }
            else
            {
                // 如果场景中已存在宠物，替换为另一个宠物
                ReplacePet((usingPetIndex + 1) % petCount);
            }
        }

        // 按下X键删除当前存在的宠物
        if (Input.GetKeyDown(KeyCode.X))
        {
            DeletePet();
        }
    }

    // 创建新的宠物
    private void CreatePet()
    {
        currentPet = Instantiate(catPrefab, transform.position, Quaternion.identity);
        usingPetIndex = 1;
    }

    // 替换现有宠物为另一个宠物
    private void ReplacePet(int index)
    {
        // 删除当前存在的宠物
        Destroy(currentPet);
        // 根据当前状态创建新的宠物
        switch (index) {
            case (0):
                currentPet = Instantiate(catPrefab, transform.position, Quaternion.identity); usingPetIndex = 0; break;
            case (1):
                currentPet = Instantiate(birdPrefab, transform.position, Quaternion.identity); usingPetIndex = 1; break;
            case (2):
                currentPet = Instantiate(dogPrefab, transform.position, Quaternion.identity); usingPetIndex = 2; break;
        }
    }

    // 删除当前存在的宠物
    private void DeletePet()
    {
        if (currentPet != null)
        {
            Destroy(currentPet);
            currentPet = null;
        }
    }
}
