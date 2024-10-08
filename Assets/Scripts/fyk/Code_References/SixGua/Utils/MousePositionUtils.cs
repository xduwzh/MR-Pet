using UnityEngine;
using UnityEngine.InputSystem;
public static class MousePositionUtils
{
   public static Vector2 mousePos
   {
        get => GetMousePos();
   }
    public static Vector2 GetMousePos()
    {
        return (Mouse.current == null) ? Vector2.zero : Mouse.current.position.ReadValue();
    }
    public static Vector3 MouseToTerrainPosition()
    {
        Vector3 position = Vector3.zero;
        var ray = Camera.main.ScreenPointToRay(mousePos);
        if (Physics.Raycast(ray, out RaycastHit info, 10000, LayerMask.GetMask("Ground", "Water")))
        {
            if(info.transform.gameObject.layer == LayerMask.NameToLayer("Water"))
            {
                Controller.Instance.isCanBuild = false;
            }
            else
            {
                Controller.Instance.isCanBuild = true;
            }
            position = info.point;
        }
        return position;    
    }


    public static RaycastHit CameraRay()
    {
        var ray = Camera.main.ScreenPointToRay(mousePos);
        if (Physics.Raycast(Camera.main.ScreenPointToRay(mousePos), out RaycastHit info))
        {
            return info;
        }
        return new RaycastHit();
    }
}
