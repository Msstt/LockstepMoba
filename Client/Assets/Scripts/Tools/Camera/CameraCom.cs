// 镜头控制组件，对镜头的操作与其他模块没有交互，所以直接放在这里了
// TODO
// 1. 拉近时旋转镜头
// 2. 缩放惯性

using System;
using Combat;
using Combat.Actor;
using UnityEngine;

public class CameraCom : MonoBehaviour {
    private float screenPadding = 5;

    [Header("镜头移动速度")]
    public float moveSpeed;
    [Header("镜头缩放速度")]
    public float scaleSpeed;
    [Header("镜头高度")]
    public Vector2 cameraHeight;
    
    private Camera mainCamera;
    private float curHeight;
    private Vector3 cameraPos;
    
    public void Awake() {
        mainCamera = GetComponent<Camera>();
    }

    public void Start() {
        mainCamera.transform.position = -mainCamera.transform.forward * cameraHeight.y;
        curHeight = cameraHeight.y;
        cameraPos = Vector3.zero;
    }

    public void Update() {
        if (!Application.isFocused) {
            return;
        }
        Move();
        Scale();
        MoveToSelf();
        mainCamera.transform.position = cameraPos - mainCamera.transform.forward * curHeight;
    }

    private void Move() {
        Vector3 mousePos = Input.mousePosition;
        if (mousePos.x <= screenPadding) {
            cameraPos -= Time.deltaTime * moveSpeed * Vector3.left;
        }
        if (mousePos.x >= Screen.width - screenPadding) {
            cameraPos -= Time.deltaTime * moveSpeed * Vector3.right;
        }
        if (mousePos.y <= screenPadding) {
            cameraPos -= Time.deltaTime * moveSpeed * Vector3.back;
        }
        if (mousePos.y >= Screen.height - screenPadding) {
            cameraPos -= Time.deltaTime * moveSpeed * Vector3.forward;
        }
    }

    private void MoveToSelf() {
        if (Input.GetKey(KeyCode.Space)) {
            var self = ActorUtils.GetActor(CombatUtils.SelfUid);
            if (self != null) {
                cameraPos.x = self.Pos.ToVector3().x;
                cameraPos.z = self.Pos.ToVector3().z;
            }
        }
    }
    
    private void Scale() {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0) {
            curHeight -= Time.deltaTime * scaleSpeed * scroll;
            curHeight = Math.Clamp(curHeight, cameraHeight.x, cameraHeight.y);
        }
    }
}
