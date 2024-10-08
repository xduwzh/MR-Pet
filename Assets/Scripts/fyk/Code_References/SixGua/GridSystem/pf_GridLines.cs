using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Grid;

public class pf_GridLines : MonoBehaviour
{
    private int Width;
    private int Height;
    private Transform[] WidthLines;
    private Transform[] HeightLines;

    private LineRenderer[] WidthLineRenders;
    private LineRenderer[] HeightLineRenders;

    public Transform Line_phototype;

    private bool tmp = false;
    void Start()
    {
        Width = GridBuildingSystem.Instance.pf_rowCount;
        Height = GridBuildingSystem.Instance.pf_columnCount;

        WidthLines = new Transform[Width];
        HeightLines = new Transform[Height];

        WidthLineRenders = new LineRenderer[Width];
        HeightLineRenders = new LineRenderer[Height];

        //GridBuildingSystem.Instance.OnBuildingModeChanged += BuildingModeChangedHandler;
        for (int i = 0; i < Width; i++)
        {
            WidthLines[i] = Instantiate(Line_phototype, this.transform);
            WidthLineRenders[i] = WidthLines[i].GetComponent<LineRenderer>();
            WidthLineRenders[i].positionCount = 2;
            WidthLineRenders[i].startWidth = 0.2f;
            WidthLineRenders[i].endWidth = 0.2f;
            WidthLineRenders[i].SetPosition(0, GridBuildingSystem.Instance.pf_grid.GetWorldPosition(i, 0));
            WidthLineRenders[i].SetPosition(1, GridBuildingSystem.Instance.pf_grid.GetWorldPosition(i, Height));
        }
        for (int j = 0; j < Height; j++)
        {
            HeightLines[j] = Instantiate(Line_phototype, this.transform);
            HeightLineRenders[j] = HeightLines[j].GetComponent<LineRenderer>();
            HeightLineRenders[j].positionCount = 2;
            HeightLineRenders[j].startWidth = 0.2f;
            HeightLineRenders[j].endWidth = 0.2f;
            HeightLineRenders[j].SetPosition(0, GridBuildingSystem.Instance.pf_grid.GetWorldPosition(0, j));
            HeightLineRenders[j].SetPosition(1, GridBuildingSystem.Instance.pf_grid.GetWorldPosition(Width, j));
        }
        SetInvisible();
    }
    private void PlayerModeChangedHandler(PlayerMode playerMode)
    {
        if (playerMode == PlayerMode.Build)
        {
            SetVisible();
        }
        else
        {
            SetInvisible();
        }
    }

    private void SetVisible()
    {
        for (int i = 0; i < Width; i++)
        {
            WidthLineRenders[i].enabled = true;
        }
        for (int j = 0; j < Height; j++)
        {
            HeightLineRenders[j].enabled = true;
        }
    }
    private void SetInvisible()
    {
        for (int i = 0; i < Width; i++)
        {
            WidthLineRenders[i].enabled = false;
        }
        for (int j = 0; j < Height; j++)
        {
            HeightLineRenders[j].enabled = false;
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if(tmp == false)
            {
                SetVisible();
                tmp = true;
            }
            else
            {
                SetInvisible();
                tmp = false;
            }
        }
    }
}
