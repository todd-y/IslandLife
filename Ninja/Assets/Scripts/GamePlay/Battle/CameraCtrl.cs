using UnityEngine;
using System.Collections;

public class CameraCtrl : MonoBehaviour {

	// Use this for initialization
	void Start () {
        BattleMgr.Instance.curCameraCtrl = this;
	}

    public void SetPos(Vector3 newPos) {
        transform.position = new Vector3(newPos.x, newPos.y, transform.position.z);
    }
}
