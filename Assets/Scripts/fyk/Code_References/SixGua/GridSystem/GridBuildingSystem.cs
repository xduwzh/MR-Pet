using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using CodeMonkey.Utils;
using PathFinding;
using Newtonsoft.Json;

namespace Grid
{
    public class GridBuildingSystem : Singleton<GridBuildingSystem>, IDisposable
    {
        //Grid
        public int rowCount = 50;
        public int columnCount = 50;
        public float cellSize = 10;
        public Vector3 StartOrigin = Vector3.zero;
        public Vector3 UpStartOrigin;
        public bool ShowDebug = false;

        public int UpWorldHeigh;

        public Transform linePhotoyupe;
        public Transform testLine;
        public LineRenderer testLineRenderer;
        public Transform testObject;
        public Transform testObject2;
        //寻路算法网格
        [HideInInspector] public int pf_rowCount;
        [HideInInspector] public int pf_columnCount;
        [HideInInspector] public float pf_cellSize;
        [HideInInspector] public Vector3 pf_StartOrigin;
        [HideInInspector] public bool pf_ShowDebug = false;
        //PathFinding
        [HideInInspector]public GridPathFinding pathFinding;

        [HideInInspector]public GridXZ<GridObject> grid;
        [HideInInspector]public GridXZ<GridObject> Up_grid;
        public GridXZ<GridObject> pf_grid;

        [HideInInspector] public List<GridXZ<GridObject>> L_grid;
        public override void Awake()
        {
            base.Awake();
            UpStartOrigin = new Vector3(StartOrigin.x, StartOrigin.y + 70, StartOrigin.z);
            grid = new GridXZ<GridObject>(rowCount,
                columnCount,
                cellSize,
                StartOrigin,
                (GridXZ<GridObject> g, int x, int z) => new GridObject(g, x, z),
                ShowDebug);
            Up_grid = new GridXZ<GridObject>(rowCount,
                columnCount,
                cellSize,
                UpStartOrigin,
                (GridXZ<GridObject> g, int x, int z) => new GridObject(g, x, z),
                ShowDebug);
            Pf_GridInit();
            pd_walkableInit();
            pathFinding = new GridPathFinding(pf_grid);
        }
        private void pd_walkableInit()
        {
            for (int i = 0; i < rowCount * 2 + 1; i++)
            {
                for (int j = 0; j < columnCount * 2 + 1; j++)
                {
                    Vector3 origin = new Vector3(pf_grid.GetWorldPosition(i, j).x + cellSize / 4, 10, pf_grid.GetWorldPosition(i, j).z + cellSize / 4);
                    Vector3 direction = Vector3.down;
                    Ray ray = new Ray(origin, direction);
                    if (Physics.Raycast(ray, out RaycastHit info, 10000, LayerMask.GetMask("Ground", "Water")))
                    {

                        if (info.transform.gameObject.layer == LayerMask.NameToLayer("Water"))
                        {
                            pf_grid.GetGridObject(i, j).IsWalkable = false;
                            //Debug.Log("false: "+ "(" + i + "," + j + ")" + info.transform.name);
                            //Instantiate(testObject, new Vector3(pf_grid.GetWorldPosition(i, j).x + cellSize / 4, 0, pf_grid.GetWorldPosition(i, j).z + cellSize / 4), Quaternion.Euler(0, 0, 0));
                        }
                        else
                        {
                            pf_grid.GetGridObject(i, j).IsWalkable = true;
                            //Debug.Log("true: " + "(" + i + "," + j + ")" + info.transform.name);
                            //Instantiate(testObject2, new Vector3(pf_grid.GetWorldPosition(i, j).x + cellSize / 4, 0, pf_grid.GetWorldPosition(i, j).z + cellSize / 4), Quaternion.Euler(0, 0, 0));
                        }
                    }
                }
            }
        }

        private void OnDestroy()
        {
            Dispose();
        }
        public void Dispose()
        {
            for (int i = 0; i < grid.GridArray.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GridArray.GetLength(1); j++)
                {
                    grid.GridArray[i, j] = null;
                }
            }
            grid.GridArray = null;
            grid = null;
        }
        private void Pf_GridInit()
        {
            pf_rowCount = rowCount * 2 + 1;
            pf_columnCount = columnCount * 2 + 1;
            pf_cellSize = cellSize / 2;
            pf_StartOrigin = new Vector3(StartOrigin.x - cellSize / 4, StartOrigin.y, StartOrigin.z - cellSize / 4);
            pf_grid = new GridXZ<GridObject>(pf_rowCount,
                pf_columnCount,
                pf_cellSize,
                pf_StartOrigin,
                (GridXZ<GridObject> g, int x, int z) => new GridObject(g, x, z),
                pf_ShowDebug);
        }

    }
    public class GridObject
    {
        public bool CanBuild { get { return placeableObject == null; } }
        [JsonIgnore]
        public PlaceableObject PlaceableObject
        {
            get => placeableObject;
            set
            {
                placeableObject = value;
                //if (placeableObject.placeableObjectSO.isWalkable == false)
                //{
                //    IsWalkable = false;
                //}
                grid.TriggerGridObjectChanged(x, z);
            }
        }
        public int x;
        public int z;
        public string PlaceableObjectName;
        public PlaceableObjectSO.Dir Direction;

        private GridXZ<GridObject> grid;
        private PlaceableObject placeableObject;

        //Path Finding
        // Cost from the start node
        [JsonIgnore]
        public int G;
        // Heuristic cost to reach the end node(won't consider obstacle)
        [JsonIgnore]
        public int H;
        [JsonIgnore]
        //F = G + H
        public int F;
        //指向前面的节点
        [JsonIgnore]
        public GridObject CameFromNode;
        [JsonIgnore]
        public bool IsWalkable;
        public GridObject(GridXZ<GridObject> grid, int x, int z)
        {
            this.grid = grid;
            this.x = x;
            this.z = z;
            IsWalkable = true;
        }

        public override string ToString()
        {
            return $"{x}, {z}";
        }
        public void CalculateFCost()
        {
            F = G + H;
        }

        public void ClearPlaceableObject()
        {
            IsWalkable = true;
            placeableObject = null;
            PlaceableObjectName = null;
            Direction = PlaceableObjectSO.Dir.Down;
            grid.TriggerGridObjectChanged(x, z);
        }
    }
    public class GridXZ<TGridObject>
    {

        public event EventHandler<OnGridObjectChangedEventArgs> OnGridObjectChanged;
        public class OnGridObjectChangedEventArgs : EventArgs
        {
            public int x;
            public int z;
        }

        public TGridObject[,] GridArray;
        public int Width;
        public int Height;
        public float CellSize;
        [JsonIgnore]
        public TextMesh[,] DebugTextArray { get; private set; }
        private Vector3 originPosition;

        private bool DrawLine = false;

        public GridXZ() { }

        public GridXZ(int width, int height, float cellSize, Vector3 originPosition, Func<GridXZ<TGridObject>, int, int, TGridObject> createGridObject, bool showDebug)
        {
            this.Width = width;
            this.Height = height;
            this.CellSize = cellSize;
            this.originPosition = originPosition;

            GridArray = new TGridObject[width, height];

            for (int x = 0; x < GridArray.GetLength(0); x++)
            {
                for (int z = 0; z < GridArray.GetLength(1); z++)
                {
                    GridArray[x, z] = createGridObject(this, x, z);
                }
            }

            if (showDebug)
            {
                InitializeDebugTextArray(width, height, cellSize);
            }
        }

        public void InitializeDebugTextArray(int width, int height, float cellSize)
        {
            DebugTextArray = new TextMesh[width, height];
            for (int x = 0; x < GridArray.GetLength(0); x++)
            {
                for (int z = 0; z < GridArray.GetLength(1); z++)
                {
                    DebugTextArray[x, z] = UtilsClass.CreateWorldText(GridArray[x, z]?.ToString(), null, GetWorldPosition(x, z) + new Vector3(cellSize, 0, cellSize) * .5f, 15, Color.white, TextAnchor.MiddleCenter, TextAlignment.Center);
                    Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x, z + 1), Color.white, 100f);
                    Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x + 1, z), Color.white, 100f);
                }
            }
            Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
            Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);

            OnGridObjectChanged += (object sender, OnGridObjectChangedEventArgs eventArgs) =>
            {
                DebugTextArray[eventArgs.x, eventArgs.z].text = GridArray[eventArgs.x, eventArgs.z]?.ToString();
            };
        }

        public void SetDebugText(int x, int z, string text)
        {
            DebugTextArray[x, z].text = text;
        }

        public Vector3 GetWorldPosition(int x, int z)
        {
            return new Vector3(x, 0, z) * CellSize + originPosition;
        }

        public void GetXZ(Vector3 worldPosition, out int x, out int z)
        {
            x = Mathf.FloorToInt((worldPosition - originPosition).x / CellSize);
            z = Mathf.FloorToInt((worldPosition - originPosition).z / CellSize);
        }

        public void SetGridObject(int x, int z, TGridObject value)
        {
            if (x >= 0 && z >= 0 && x < Width && z < Height)
            {
                GridArray[x, z] = value;
                TriggerGridObjectChanged(x, z);
            }
        }

        public void TriggerGridObjectChanged(int x, int z)
        {
            OnGridObjectChanged?.Invoke(this, new OnGridObjectChangedEventArgs { x = x, z = z });
        }

        public void SetGridObject(Vector3 worldPosition, TGridObject value)
        {
            GetXZ(worldPosition, out int x, out int z);
            SetGridObject(x, z, value);
        }

        public TGridObject GetGridObject(int x, int z)
        {
            if (x >= 0 && z >= 0 && x < Width && z < Height)
            {
                return GridArray[x, z];
            }
            else
            {
                return default(TGridObject);
            }
        }

        public TGridObject GetGridObject(Vector3 worldPosition)
        {
            int x, z;
            GetXZ(worldPosition, out x, out z);
            return GetGridObject(x, z);
        }

        public Vector2Int ValidateGridPosition(Vector2Int gridPosition)
        {
            return new Vector2Int(
                Mathf.Clamp(gridPosition.x, 0, Width - 1),
                Mathf.Clamp(gridPosition.y, 0, Height - 1)
            );
        }

    }
}
