using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Controller : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject catPrefab;
    public GameObject birdPrefab;
    public GameObject dogPrefab;

    private GameObject currentPet; // ��ǰ���ڵĳ���
    private int usingPetIndex = -1; // ��ǰ����Prefab��״̬
    private int petCount = 3;

    void Start()
    {

    }

    void Update()
    {
        // ����G���������滻����
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (currentPet == null)
            {
                // ��������в����ڳ������һ���µĳ���
                CreatePet();
            }
            else
            {
                // ����������Ѵ��ڳ���滻Ϊ��һ������
                ReplacePet((usingPetIndex + 1) % petCount);
            }
        }

        // ����X��ɾ����ǰ���ڵĳ���
        if (Input.GetKeyDown(KeyCode.X))
        {
            DeletePet();
        }
    }

    // �����µĳ���
    private void CreatePet()
    {
        currentPet = Instantiate(catPrefab, transform.position, Quaternion.identity);
        usingPetIndex = 1;
    }

    // �滻���г���Ϊ��һ������
    private void ReplacePet(int index)
    {
        // ɾ����ǰ���ڵĳ���
        Destroy(currentPet);
        // ���ݵ�ǰ״̬�����µĳ���
        switch (index) {
            case (0):
                currentPet = Instantiate(catPrefab, transform.position, Quaternion.identity); usingPetIndex = 0; break;
            case (1):
                currentPet = Instantiate(birdPrefab, transform.position, Quaternion.identity); usingPetIndex = 1; break;
            case (2):
                currentPet = Instantiate(dogPrefab, transform.position, Quaternion.identity); usingPetIndex = 2; break;
        }
    }

    // ɾ����ǰ���ڵĳ���
    private void DeletePet()
    {
        if (currentPet != null)
        {
            Destroy(currentPet);
            currentPet = null;
        }
    }
}
