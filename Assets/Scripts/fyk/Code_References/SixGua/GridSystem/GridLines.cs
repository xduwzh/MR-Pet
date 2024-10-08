using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Grid;

public class GridLines : MonoBehaviour
{
    private int Width;
    private int Height;
    private Transform[] WidthLines;
    private Transform[] HeightLines;
    //upWord
    private Transform[] UpWordWidthLines;
    private Transform[] UpWordHeightLines;

    private LineRenderer[] WidthLineRenders;
    private LineRenderer[] HeightLineRenders;
    //upWord
    private LineRenderer[] UpWordWidthLineRenders;
    private LineRenderer[] UpWordHeightLineRenders;

    public Transform Line_phototype;
    void Start()
    {
        Width = GridBuildingSystem.Instance.rowCount + 1;
        Height = GridBuildingSystem.Instance.columnCount + 1;

        WidthLines = new Transform[Width];
        HeightLines = new Transform[Height];

        WidthLineRenders = new LineRenderer[Width];
        HeightLineRenders = new LineRenderer[Height];
        //UpWord
        UpWordWidthLines = new Transform[Width];
        UpWordHeightLines = new Transform[Height];

        UpWordWidthLineRenders = new LineRenderer[Width];
        UpWordHeightLineRenders = new LineRenderer[Height];

        Controller.Instance.OnPlayerModeChanged += PlayerModeChangedHandler;
        for (int i = 0; i < Width; i++)
        {
            WidthLines[i] =  Instantiate(Line_phototype, this.transform);
            WidthLineRenders[i] = WidthLines[i].GetComponent<LineRenderer>();
            WidthLineRenders[i].positionCount = 2;
            WidthLineRenders[i].startWidth = 0.5f;
            WidthLineRenders[i].endWidth = 0.5f;
            WidthLineRenders[i].SetPosition(0, GridBuildingSystem.Instance.grid.GetWorldPosition(i, 0));
            WidthLineRenders[i].SetPosition(1, GridBuildingSystem.Instance.grid.GetWorldPosition(i, Height - 1));
        }
        for(int j = 0; j < Height; j++)
        {
            HeightLines[j] = Instantiate(Line_phototype, this.transform);
            HeightLineRenders[j] = HeightLines[j].GetComponent<LineRenderer>();
            HeightLineRenders[j].positionCount = 2;
            HeightLineRenders[j].startWidth = 0.5f;
            HeightLineRenders[j].endWidth = 0.5f;
            HeightLineRenders[j].SetPosition(0, GridBuildingSystem.Instance.grid.GetWorldPosition(0, j));
            HeightLineRenders[j].SetPosition(1, GridBuildingSystem.Instance.grid.GetWorldPosition(Width - 1, j));
        }
        //UpWord
        for (int i = 0; i < Width; i++)
        {
            UpWordWidthLines[i] = Instantiate(Line_phototype, this.transform);
            UpWordWidthLineRenders[i] = UpWordWidthLines[i].GetComponent<LineRenderer>();
            UpWordWidthLineRenders[i].positionCount = 2;
            UpWordWidthLineRenders[i].startWidth = 0.5f;
            UpWordWidthLineRenders[i].endWidth = 0.5f;
            UpWordWidthLineRenders[i].SetPosition(0, GridBuildingSystem.Instance.Up_grid.GetWorldPosition(i, 0));
            UpWordWidthLineRenders[i].SetPosition(1, GridBuildingSystem.Instance.Up_grid.GetWorldPosition(i, Height - 1));
        }
        for (int j = 0; j < Height; j++)
        {
            UpWordHeightLines[j] = Instantiate(Line_phototype, this.transform);
            UpWordHeightLineRenders[j] = UpWordHeightLines[j].GetComponent<LineRenderer>();
            UpWordHeightLineRenders[j].positionCount = 2;
            UpWordHeightLineRenders[j].startWidth = 0.5f;
            UpWordHeightLineRenders[j].endWidth = 0.5f;
            UpWordHeightLineRenders[j].SetPosition(0, GridBuildingSystem.Instance.Up_grid.GetWorldPosition(0, j));
            UpWordHeightLineRenders[j].SetPosition(1, GridBuildingSystem.Instance.Up_grid.GetWorldPosition(Width - 1, j));
        }
        SetInvisible();
        SetUpInvisible();
    }
    private void PlayerModeChangedHandler(PlayerMode playerMode)
    {
        if (playerMode == PlayerMode.Build)
        {
            if (Controller.Instance.isEditModel == true)
            {
                if (Controller.Instance.isOnUpWord == true)
                {
                    SetUpVisible();
                    SetInvisible();
                }
                else
                {
                    SetVisible();
                    SetUpInvisible();
                }
            }
            else
            {
                SetVisible();
            }
        }
        else
        {
            SetInvisible();
            if (Controller.Instance.isEditModel == true)
            {
                SetUpInvisible();
            }
        }
    }
    private void SetVisible()
    {
        for(int i = 0; i < Width; i++)
        {
            WidthLineRenders[i].enabled = true;
        }
        for(int j = 0; j < Height; j++)
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
    private void SetUpVisible()
    {
        for (int i = 0; i < Width; i++)
        {
            UpWordWidthLineRenders[i].enabled = true;
        }
        for (int j = 0; j < Height; j++)
        {
            UpWordHeightLineRenders[j].enabled = true;
        }
    }
    private void SetUpInvisible()
    {
        for (int i = 0; i < Width; i++)
        {
            UpWordWidthLineRenders[i].enabled = false;
        }
        for (int j = 0; j < Height; j++)
        {
            UpWordHeightLineRenders[j].enabled = false;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
