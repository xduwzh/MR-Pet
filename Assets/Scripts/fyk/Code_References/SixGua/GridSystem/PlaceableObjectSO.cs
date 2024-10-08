using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Attribute
{
    UnderBuilding,
    UpBuilding,
    Block,
    Resources,
    Farm
}

[CreateAssetMenu()]
public class PlaceableObjectSO : ScriptableObject
{
    [Header("PlaceableObjectSO属性相关")]
    public string NameString;
    public Attribute attribute; 
    public PlacaebleObjectCategories category;
    [Header("消耗居民数量")]
    public int MouseNeeded;
    [Header("建筑消耗资源")]
    public int buildingMaterialCost;
    public int seedsCost;
    [Header("UI显示图片")]
    public Image buildingImg;
    [Header("上层建筑行动")]
    public List<MotionClass> buildingMotions;
    [Header("食物生产速度*/天")]
    public int foodProduceSpeed;
    [Header("增长智商*/天")]
    public int KnowledgeIncrease;
    [Header("智商需求")]
    public int KnowledgeNeeded;
    [Header("下层单个资源点资源储量")]
    public int FoodAmount;
    public int ConstructionMaterialAmount;
    [Header("是否可行走")]
    public bool isWalkable = false;

    public Transform Prefab;
    public int Width;
    public int Height;
    //pf
    private int pf_Width;
    private int pf_Height;

    public static Dir GetNextDir(Dir dir)
    {
        switch (dir)
        {
            default:
            case Dir.Down: return Dir.Left;
            case Dir.Left: return Dir.Up;
            case Dir.Up: return Dir.Right;
            case Dir.Right: return Dir.Down;
        }
    }

    public enum Dir
    {
        Down,
        Left,
        Up,
        Right,
    }
    public int GetRotationAngle(Dir dir)
    {
        switch (dir)
        {
            default:
            case Dir.Down: return 0;
            case Dir.Left: return 90;
            case Dir.Up: return 180;
            case Dir.Right: return 270;
        }
    }

    public Vector2Int GetRotationOffset(Dir dir)
    {
        switch (dir)
        {
            default:
            case Dir.Down: return new Vector2Int(0, 0);
            case Dir.Left: return new Vector2Int(0, Width);
            case Dir.Up: return new Vector2Int(Width, Height);
            case Dir.Right: return new Vector2Int(Height, 0);
        }
    }

    public List<Vector2Int> GetGridPositionList(Vector2Int offset, Dir dir)
    {
        List<Vector2Int> gridPositionList = new List<Vector2Int>();
        switch (dir)
        {
            default:
            case Dir.Down:
            case Dir.Up:
                for (int x = 0; x < Width; x++)
                {
                    for (int y = 0; y < Height; y++)
                    {
                        gridPositionList.Add(offset + new Vector2Int(x, y));
                    }
                }
                break;
            case Dir.Left:
            case Dir.Right:
                for (int x = 0; x < Height; x++)
                {
                    for (int y = 0; y < Width; y++)
                    {
                        gridPositionList.Add(offset + new Vector2Int(x, y));
                    }
                }
                break;
        }
        return gridPositionList;
    }
    public List<Vector2Int> pf_GetGridPositionList(Vector2Int offset, Dir dir)
    {
        Vector2Int pf_offset = new Vector2Int(offset.x * 2 + 1, offset.y * 2 + 1);
        pf_Width = 2 * Width - 1;
        pf_Height = 2 * Height - 1;
        List<Vector2Int> gridPositionList = new List<Vector2Int>();
        switch (dir)
        {
            default:
            case Dir.Down:
            case Dir.Up:
                for (int x = 0; x < pf_Width; x++)
                {
                    for (int y = 0; y < pf_Height; y++)
                    {
                        gridPositionList.Add(pf_offset + new Vector2Int(x, y));
                    }
                }
                break;
            case Dir.Left:
            case Dir.Right:
                for (int x = 0; x < pf_Height; x++)
                {
                    for (int y = 0; y < pf_Width; y++)
                    {
                        gridPositionList.Add(pf_offset + new Vector2Int(x, y));
                    }
                }
                break;
        }
        return gridPositionList;
    }
}

