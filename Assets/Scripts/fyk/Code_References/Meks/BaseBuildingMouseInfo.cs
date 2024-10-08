using UnityEngine;
using UnityEngine.EventSystems;

public class BaseBuildingMouseInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Font font;
    public int fontSize = 30;
    [Multiline]//允许多行输入
    public string text = " 默认文本 ";
    public PlaceableObjectSO placeableObjectSO;

    private bool showText = false;
    private GUIStyle style;

    public void Start()
    {
        style = new GUIStyle("box");
        if(placeableObjectSO.buildingMaterialCost != 0)
        {
            text += "\n建造消耗建筑材料*" + placeableObjectSO.buildingMaterialCost;
        }
        if (placeableObjectSO.seedsCost != 0)
        {
            text += " 种子*" + placeableObjectSO.seedsCost;
        }
        if (placeableObjectSO.attribute == Attribute.Farm)
        {
            text += "\n每天产出食物+" + placeableObjectSO.foodProduceSpeed;
        }
        if(placeableObjectSO.KnowledgeNeeded != 0)
        {
            text += "\n智力要求>" + placeableObjectSO.KnowledgeNeeded;
        }
        if(placeableObjectSO.MouseNeeded != 0)
        {
            text += "\n需要老鼠数量*" + placeableObjectSO.MouseNeeded;
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

