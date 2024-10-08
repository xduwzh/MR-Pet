using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainPropertiesUI : MonoBehaviour
{

    public TextMeshProUGUI Hate;


    public TextMeshProUGUI Population;
    public TextMeshProUGUI material;
    public TextMeshProUGUI food;
    public TextMeshProUGUI SeedStore;
    public TextMeshProUGUI DiceNum;
    public TextMeshProUGUI knowledgeLevel;


    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    Model.Instance.HateValue += 10;
        //}

        Hate.text = "³ðºÞÖµ:  " + Model.Instance.HateValue + " / 100";


        Population.text = Model.Instance.Mouse_List.Count + "/" + Model.Instance.MouseCount.ToString();
        material.text = Model.Instance.BuildingMaterials.ToString();
        food.text = Model.Instance.FoodStore + " / " + Model.Instance.FoodStoreLimit;
        knowledgeLevel.text = Model.Instance.KnowledgeLevel.ToString();
        DiceNum.text = Model.Instance.DiceNum.ToString();
        SeedStore.text = Model.Instance.SeedStore.ToString();
    }


    
}
