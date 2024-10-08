using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathFinding;
using Grid;
public enum WorkCategories
{
    Logger, Farmer, IronMiner, StoneMiner, ConstructionWorker, Idler
}
public enum WorkType
{
    Idler, StableBuildingWorker, ResourceCollecter
}
public class C_Mouse : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float stopDistance = 1f;
    public Animator animator;

    //寻路算法变量
    private int currentPathIndex;
    private List<Vector3> cur_path;
    //空闲时随机移动
    private bool idleLog = false;
    private List<Vector3> IdlePath1;
    private List<Vector3> IdlePath2;
    private List<Vector3> IdlePath3;
    private int pathLog;

    public WorkType curWorkType;
    [HideInInspector]public PlaceableObject cur_WorkingResource = null;
    [HideInInspector]public PlaceableObject cur_WorkingBuilding = null;
    
    //记录居民工作路径
    [HideInInspector]public List<Vector3> BuildingToResourcePath;
    [HideInInspector]public List<Vector3> ResourceToBuildingPath;
    private bool isWorking = false;
    private bool isSending = false;

    private float halfCellSize;
    void Start()
    {
        //halfCellSize = GridBuildingSystem.Instance.cellSize / 2;
        //animator = GetComponent<Animator>();
        //ConvertWorkType(WorkType.Idler);
    }
    private void Awake()
    {
        halfCellSize = GridBuildingSystem.Instance.cellSize / 2;
        animator = GetComponent<Animator>();
        ConvertWorkType(WorkType.Idler);
    }

    //Update is called once per frame
    void Update()
    {
        if (cur_path != null)
        {
            
            animator.SetInteger("Anim", 1);
            ResidentMovement();
        }
        else if (curWorkType == WorkType.Idler)
        {
            animator.SetInteger("Anim", 0);
            if (idleLog == false)
            {
                idleLog = true;
                StartCoroutine(Idle(UnityEngine.Random.Range(1, 3)));
            }
        }
        else if (curWorkType == WorkType.StableBuildingWorker)
        {
            this.gameObject.SetActive(false);
        }
        else if (curWorkType == WorkType.ResourceCollecter)
        {
            animator.SetInteger("Anim", 2);
            if (isWorking == false)
            {
                isWorking = true;
                switch(cur_WorkingResource.placeableObjectSO.category)
                {
                    case (PlacaebleObjectCategories.MineResource): StartCoroutine(CollectResources(Model.Instance.MineCollectTime));break;
                    case (PlacaebleObjectCategories.FoodResource): StartCoroutine(CollectResources(Model.Instance.FoodCollectTime)); break;
                }
            }
            if (isSending == false)
            {
                isSending = true;
                switch (cur_WorkingResource.placeableObjectSO.category)
                {
                    case (PlacaebleObjectCategories.MineResource): StartCoroutine(SubmitResources(Model.Instance.MineStoreTime));break;
                    case (PlacaebleObjectCategories.FoodResource): StartCoroutine(SubmitResources(Model.Instance.FoodStoreTime)); break;
                }
            }
        }
    }
    IEnumerator Idle(int time)
    {
        yield return new WaitForSeconds(time);
        if (idleLog == true) {
            switch (pathLog)
            {
                case 1:
                    pathLog = 2; changePath(IdlePath2);
                    break;
                case 2:
                    pathLog = 3; changePath(IdlePath3);
                    break;
                case 3:
                    pathLog = 1; changePath(IdlePath1);
                    break;
            }
        }
        idleLog = false;
    }
    IEnumerator CollectResources(int time)
    {
        yield return new WaitForSeconds(time);
        //switch (cur_WorkingResource.placeableObjectSO.category)
        //{
        //    case (PlacaebleObjectCategories.FoodResource): cur_WorkingResource.FoodAmount -= Model.Instance.FoodNumEachTime; break;
        //    case (PlacaebleObjectCategories.MineResource): cur_WorkingResource.ConstructionMaterialAmount -= Model.Instance.MineNumEachTime; break;
        //}

        changePath(ResourceToBuildingPath);
        isSending = false;

    }
    IEnumerator SubmitResources(int time)
    {
        yield return new WaitForSeconds(time);
        switch (cur_WorkingResource.placeableObjectSO.category)
        {
            case (PlacaebleObjectCategories.FoodResource):Model.Instance.ChangeBasicValue("Food", true, Model.Instance.FoodNumEachTime); break;
            case (PlacaebleObjectCategories.MineResource): Model.Instance.ChangeBasicValue("ConstructionMaterial", true, Model.Instance.MineNumEachTime);  break;
        }
        if(cur_WorkingResource == null)
        {
            ConvertWorkType(WorkType.StableBuildingWorker);
        }
        else 
        { 
            changePath(BuildingToResourcePath);
            isWorking = false;
         }
    }
    public void ConvertWorkType(WorkType work)
    {
        if (curWorkType != work)
        {
            curWorkType = work;
        }
        if (curWorkType == WorkType.Idler)
        {
            this.gameObject.SetActive(true);

            Vector3 pos0 = transform.position;
            Vector3 pos1 = GetRandomPosition();
            Vector3 pos2 = GetRandomPosition();

            IdlePath1 = GridPathFinding.Instance.FindPath(pos0, pos1);
            IdlePath2 = GridPathFinding.Instance.FindPath(pos1, pos2);
            IdlePath3 = GridPathFinding.Instance.FindPath(pos2, pos0);
            StopAllCoroutines();
            changePath(IdlePath1);
            pathLog = 1;
        }
        else if (curWorkType == WorkType.StableBuildingWorker)
        {
            //Vector3 buildingPos = GetBuildingDoorGrid(cur_WorkingBuilding);
            Vector3 buildingPos = new Vector3(cur_WorkingBuilding.transform.position.x + halfCellSize, cur_WorkingBuilding.transform.position.y, cur_WorkingBuilding.transform.position.z + halfCellSize);
            List<Vector3> tmpPath = GridPathFinding.Instance.FindPath(transform.position, buildingPos);
            
            StopAllCoroutines();
            changePath(tmpPath);
            idleLog = false;
        }
        else if (curWorkType == WorkType.ResourceCollecter)
        {
            this.gameObject.SetActive(true);
            StopAllCoroutines();

            CalculateWorkPath();
            List<Vector3> tmpPath = GridPathFinding.Instance.FindPath(transform.position, 
                new Vector3(cur_WorkingResource.transform.position.x + halfCellSize, cur_WorkingResource.transform.position.y, cur_WorkingResource.transform.position.z + halfCellSize));
            if(tmpPath == null)
            {
                Debug.Log("寻路失败");
            }
            changePath(tmpPath);
            idleLog = false;
            isWorking = false;
            isSending = true;
        }
    }
    public void CalculateWorkPath()
    {
        //Vector3 buildingPos = GetBuildingDoorGrid(cur_WorkingBuilding);
        //Vector3 resourcePos = GetBuildingDoorGrid(cur_WorkingResource);
        //BuildingToResourcePath = GridPathFinding.Instance.FindPath(buildingPos, resourcePos);
        //ResourceToBuildingPath = GridPathFinding.Instance.FindPath(resourcePos, buildingPos);
        //BuildingToResourcePath.Add(BuildingToResourcePath[BuildingToResourcePath.Count - 1] + new Vector3(0, 0, GridBuildingSystem.Instance.cellSize));
        //ResourceToBuildingPath.Add(ResourceToBuildingPath[ResourceToBuildingPath.Count - 1] + new Vector3(0, 0, GridBuildingSystem.Instance.cellSize));

        Vector3 buildingPos = new Vector3(cur_WorkingBuilding.transform.position.x + halfCellSize, cur_WorkingBuilding.transform.position.y, cur_WorkingBuilding.transform.position.z + halfCellSize);
        Vector3 resourcePos = new Vector3(cur_WorkingResource.transform.position.x + halfCellSize, cur_WorkingResource.transform.position.y, cur_WorkingResource.transform.position.z + halfCellSize);

        BuildingToResourcePath = GridPathFinding.Instance.FindPath(buildingPos, resourcePos);
        ResourceToBuildingPath = GridPathFinding.Instance.FindPath(resourcePos, buildingPos);
    }
    private void DoLoggerWork()
    {

    }
    private Vector3 GetBuildingDoorGrid(PlaceableObject building)
    {
        Vector3 pos = new Vector3(building.transform.position.x + (building.placeableObjectSO.Width * (GridBuildingSystem.Instance.cellSize / 2)), building.transform.position.y, 
            building.transform.position.z);

        return pos;
    }
    private Vector3 GetBuildingCenter(PlaceableObject building)
    {
        Vector3 pos = new Vector3(building.transform.position.x + building.placeableObjectSO.Width * GridBuildingSystem.Instance.cellSize / 2, 
            building.transform.position.y, building.transform.position.z + building.placeableObjectSO.Height * GridBuildingSystem.Instance.cellSize / 2);
        return pos;
    }

    private void changePath(List<Vector3> path)
    {
        currentPathIndex = 0;
        cur_path = path;
    }


    private void SetTargetPosition(Vector3 targetPosition)
    {
        currentPathIndex = 0;
        cur_path = GridPathFinding.Instance.FindPath(transform.position, targetPosition);

        if (cur_path != null && cur_path.Count > 1)
        {
            cur_path.RemoveAt(0);
            cur_path.Add(cur_path[cur_path.Count - 1] + new Vector3(0, 0, GridBuildingSystem.Instance.cellSize));
        }
    }
    private Vector3 GetRandomPosition()
    {
        Vector3 randomPosition;
        int tmp_x;
        int tmp_z;
        do
        {
            //randomPosition = new Vector3(
            //    UnityEngine.Random.Range(10, GridBuildingSystem.Instance.rowCount * GridBuildingSystem.Instance.cellSize - 10),
            //    1,
            //    UnityEngine.Random.Range(10, GridBuildingSystem.Instance.columnCount * GridBuildingSystem.Instance.cellSize - 10));
            randomPosition = new Vector3(
                UnityEngine.Random.Range(transform.position.x - 6, transform.position.x + 6),
                1,
                UnityEngine.Random.Range(transform.position.z - 6, transform.position.z + 6));
            GridBuildingSystem.Instance.pf_grid.GetXZ(randomPosition, out tmp_x, out tmp_z);
        } while (!GridBuildingSystem.Instance.pf_grid.GetGridObject(tmp_x, tmp_z).IsWalkable);
        return randomPosition;
    }

    private void StopMoving()
    {
        cur_path = null;
    }

    private void ResidentMovement()
    {
        if (cur_path != null)
        {
            var currPath = cur_path[currentPathIndex];
            var targetPosition = new Vector3(currPath.x, transform.position.y, currPath.z);

            for (int i = currentPathIndex; i < cur_path.Count - 1; ++i)
            {
                Debug.DrawLine(new Vector3(cur_path[i].x, transform.position.y, cur_path[i].z),
                    new Vector3(cur_path[i + 1].x, transform.position.y, cur_path[i + 1].z),
                    Color.green);
            }

            if (Vector3.Distance(transform.position, targetPosition) > stopDistance)
            {
                var moveDir = (targetPosition - transform.position).normalized;
                //this.transform.position = moveDir;
                this.transform.LookAt(targetPosition);
                transform.position = transform.position + moveDir * speed * Time.deltaTime;
            }
            else
            {
                currentPathIndex++;
                if (currentPathIndex >= cur_path.Count)
                {
                    StopMoving();
                }
            }
        }
    }
}
