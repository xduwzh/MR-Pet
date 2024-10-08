using Grid;
using UnityEngine;
using UnityEngine.AI;

public class BuildingPreview : MonoBehaviour
{
    [SerializeField] private Material previewMaterial;

    private Transform visual;
    private PlaceableObjectSO placeableObjectSO;
    private Mesh previewMesh;

    private Color originalColor;

    private void Start()
    {      
        if (Controller.Instance.cur_PlayerMode == PlayerMode.Build)
        {
            RefreshVisual();
        }
        Controller.Instance.OnPlayerModeChanged += BuildingModeChangedHandler;
        Controller.Instance.OnBuildingChanged += BuildingChangedHandler;
    }

    private void LateUpdate()
    {
        if (Controller.Instance.cur_PlayerMode == PlayerMode.Build)
        {
            var targetPosition = Controller.Instance.GetMouseWorldSnappedPosition();
            if (Controller.Instance.isOnUpWord && Controller.Instance.isEditModel)
            {
                targetPosition.y = 70f;
            }
            else
            {
                targetPosition.y = 0.1f;
            }
            if (!Controller.Instance.isCanBuild)
            {
                var meshes = visual.GetComponentsInChildren<MeshRenderer>();
                foreach (var mesh in meshes)
                {
                    mesh.material.color = Color.red;
                }
            }
            else
            {
                var meshes = visual.GetComponentsInChildren<MeshRenderer>();
                foreach (var mesh in meshes)
                {
                    mesh.material.color = originalColor;
                }
            }
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 15f);
            transform.rotation = Quaternion.Lerp(transform.rotation, Controller.Instance.GetCurrentBuildingRotation(), Time.deltaTime * 15f);
        }
    }

    private void RefreshVisual()
    {
        DestroyPreview();
        var placeableObjectSO = Controller.Instance.cur_PlaceableObjectSO;
        if (placeableObjectSO != null)
        {
            visual = Instantiate(placeableObjectSO.Prefab, Vector3.zero, Quaternion.identity);
            visual.GetComponent<PlaceableObject>().enabled = false;
            visual.parent = transform;
            visual.localPosition = Vector3.zero;
            visual.localEulerAngles = Vector3.zero;

            var meshes = visual.GetComponentsInChildren<MeshRenderer>();
            foreach (var mesh in meshes)
            {
                mesh.material = previewMaterial;
                originalColor = mesh.material.color;
            }
            var navObstacles = visual.GetComponentsInChildren<NavMeshObstacle>();
            foreach (var navObstacle in navObstacles)
            {
                navObstacle.enabled = false;
            }
        }
    }

    private void DestroyPreview()
    {
        if (visual != null)
        {
            Destroy(visual.gameObject);
            visual = null;
        }
    }

    private void BuildingModeChangedHandler(PlayerMode playerMode)
    {
        if (playerMode == PlayerMode.Build)
        {
            RefreshVisual();
        }
        else
        {
            DestroyPreview();
        }
    }

    private void BuildingChangedHandler()
    {
        RefreshVisual();
    }
}
