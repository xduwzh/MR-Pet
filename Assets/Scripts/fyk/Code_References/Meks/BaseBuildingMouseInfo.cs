using UnityEngine;
using UnityEngine.EventSystems;

public class BaseBuildingMouseInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Font font;
    public int fontSize = 30;
    [Multiline]//�����������
    public string text = " Ĭ���ı� ";
    public PlaceableObjectSO placeableObjectSO;

    private bool showText = false;
    private GUIStyle style;

    public void Start()
    {
        style = new GUIStyle("box");
        if(placeableObjectSO.buildingMaterialCost != 0)
        {
            text += "\n�������Ľ�������*" + placeableObjectSO.buildingMaterialCost;
        }
        if (placeableObjectSO.seedsCost != 0)
        {
            text += " ����*" + placeableObjectSO.seedsCost;
        }
        if (placeableObjectSO.attribute == Attribute.Farm)
        {
            text += "\nÿ�����ʳ��+" + placeableObjectSO.foodProduceSpeed;
        }
        if(placeableObjectSO.KnowledgeNeeded != 0)
        {
            text += "\n����Ҫ��>" + placeableObjectSO.KnowledgeNeeded;
        }
        if(placeableObjectSO.MouseNeeded != 0)
        {
            text += "\n��Ҫ��������*" + placeableObjectSO.MouseNeeded;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        showText = true;
        Controller.Instance.audioS.PlayOneShot(Controller.Instance.MouseOnUI);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        showText = false;
    }
    public void OnGUI()
    {
        style.font = font;
        style.fontSize = fontSize;
        style.normal.textColor = new Color(255, 255, 255);



        var vt = style.CalcSize(new GUIContent(text));
        if (showText)
            GUI.Box(new Rect(Input.mousePosition.x, Screen.height - Input.mousePosition.y - vt.y, vt.x, vt.y), text, style);
    }
}

