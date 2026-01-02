using UnityEngine;

public static class InputUtils {
    
    public static Vector3F? GetMousePos(int layerMask = -1) {
        if (layerMask == -1) layerMask = LayerMask.GetMask("Map");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, LayerMask.GetMask("Map"))) {
            return hitInfo.point.ToVector3F();
        }
        return null;
    }
}
