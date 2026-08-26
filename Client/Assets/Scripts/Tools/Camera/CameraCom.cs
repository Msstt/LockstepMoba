// 镜头控制组件，对镜头的操作与其他模块没有交互，所以直接放在这里了
// TODO
// 1. 拉近时旋转镜头
// 2. 缩放惯性

using System;
using Combat;
using Combat.Actor;
using EventType;
using Framework;
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

    private void OnEnable() {
        EventMgr.Instance.Register<OnGameStart>(OnGameStart);
    }
    
    private void OnDisable() {
        EventMgr.Instance.UnRegister<OnGameStart>(OnGameStart);
    }

    public void Start() {
        mainCamera.transform.position = -mainCamera.transform.forward * cameraHeight.y;
        curHeight = cameraHeight.y;
    }

    public void Update() {
        if (!Application.isFocused) {
            return;
        }
        Move();
        Scale();
        if (Input.GetKey(KeyCode.Space)) {
            MoveToSelf();
        }
        mainCamera.transform.position = cameraPos - mainCamera.transform.forward * curHeight;
    }

    private void OnGameStart() {
        MoveToSelf();
    }

    private void Move() {
#if UNITY_EDITOR
        return;
#endif
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
        var self = ActorUtils.GetActor(CombatUtils.SelfUid);
        if (self != null) {
            cameraPos.x = self.Pos.ToVector3().x;
            cameraPos.z = self.Pos.ToVector3().z;
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
