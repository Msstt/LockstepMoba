using Combat.Actor;
using InputSystem;
using UnityEngine;

public static class InputUtils {
    
    public static Vector3F? GetMousePos(int layerMask = -1) {
        if (layerMask == -1) layerMask = LayerMask.GetMask("Plane");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, layerMask)) {
            return hitInfo.point.ToVector3F();
        }
        return null;
    }
    
    public static int? GetMouseActorUid() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, LayerMask.GetMask("Actor"))) {
            return hitInfo.collider?.transform.GetComponent<ActorRaycasterCom>()?.Uid;
        }
        return null;
    }
    
    public static Vector3F? GetMouseDir() {
        Actor actor = ActorUtils.GetActor();
        if (actor == null) {
            return null;
        }
        Vector3F? pos = GetMousePos();
        if (!pos.HasValue) {
            return null;
        }
        Vector3F dir = pos.Value - actor.Pos;
        dir.y = 0;
        dir = dir.Normalized();
        if (dir == Vector3F.zero) {
            return null;
        }
        return dir;
    }
}
