using UnityEngine;
using System.Collections;

public class CameraCtrl : MonoBehaviour {

    private System.Action moveEndCallBack;

	// Use this for initialization
	void Start () {
        BattleMgr.Instance.curCameraCtrl = this;
	}

    public void SetPos(Vector3 newPos, System.Action _callBack) {
        moveEndCallBack = _callBack;
        iTween.MoveTo(gameObject, iTween.Hash("position", new Vector3(newPos.x, newPos.y, transform.position.z), "time", 0.5f, "islocal", true, "oncomplete", "MoveEnd"));
        //transform.position = new Vector3(newPos.x, newPos.y, transform.position.z);
    }

    private void MoveEnd() {
        if (moveEndCallBack != null) {
            moveEndCallBack();
        }
    }
}
