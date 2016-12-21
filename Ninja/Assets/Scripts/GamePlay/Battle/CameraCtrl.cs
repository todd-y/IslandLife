using UnityEngine;
using System.Collections;

public class CameraCtrl : MonoBehaviour {
    // Global shake amount that gets reduced every frame
    public static float Shake;
    // The amount this camera shakes relative to the Shake value
    public float ShakeScale = 1.0f;
    // The speed at which the Shake value gets reduced
    public float ShakeDampening = 10.0f;
    // The freqncy of the camera shake
    public float ShakeSpeed = 10.0f;
    private float offsetX;
    private float offsetY;
    private Vector3 basePos;


    private System.Action moveEndCallBack;

	// Use this for initialization
	void Start () {
        BattleMgr.Instance.curCameraCtrl = this;
        Send.RegisterMsg(SendType.EnterRoom, OnEnterRoom);
        Send.RegisterMsg(SendType.PlayerPosChange, OnPlayerPosChange);

        offsetX = Random.Range(-1000.0f, 1000.0f);
        offsetY = Random.Range(-1000.0f, 1000.0f);
    }

    void Destroy() {
        Send.UnregisterMsg(SendType.EnterRoom, OnEnterRoom);
        Send.UnregisterMsg(SendType.PlayerPosChange, OnPlayerPosChange);
    }

    public void SetPos(Vector3 newPos, System.Action _callBack) {
        moveEndCallBack = _callBack;
        basePos = newPos;
        iTween.MoveTo(gameObject, iTween.Hash("position", new Vector3(newPos.x, newPos.y, transform.position.z), "time", 0.5f, "islocal", true, "oncomplete", "MoveEnd"));
        //transform.position = new Vector3(newPos.x, newPos.y, transform.position.z);
    }

    private void MoveEnd() {
        if (moveEndCallBack != null) {
            moveEndCallBack();
        }
    }

    private void OnEnterRoom(object[] objs) {
    }

    private void OnPlayerPosChange(object[] objs) {

    }

    void LateUpdate() {
        Shake = ToolMgr.Dampen(Shake, 0.0f, ShakeDampening, Time.deltaTime);

        var shakeStrength = Shake * ShakeScale;
        var shakeTime = Time.time * ShakeSpeed;
        var offset = Vector3.zero;

        offset.x = Mathf.PerlinNoise(offsetX, shakeTime) * shakeStrength;
        offset.y = Mathf.PerlinNoise(offsetY, shakeTime) * shakeStrength;
        offset.z = transform.position.z;

        transform.localPosition = basePos + offset;
    }
}
