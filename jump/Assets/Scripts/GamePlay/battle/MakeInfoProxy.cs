using UnityEngine;
using System.Collections;

public class MakeInfoProxy : MonoBehaviour {

    private MakeItemGrid[] arrMake;

    private MakeInfo makeInfo {
        get {
            return BattleMgr.Instance.makeInfo;
        }
    }

    void Start() {
        InitCtrl();
        InitMsg();
        RefreshWindow();
    }

    void Destroy() {
        ClearMsg();
    }

    private void InitCtrl() {
        arrMake = new MakeItemGrid[makeInfo.alchemyNum];
        for (int index = 0; index < makeInfo.alchemyNum; index++ ) {
            MakeItemGrid grid = gameObject.GetChildControl<Transform>("imgMakeBg" + (index + 1)).gameObject.AddMissingComponent<MakeItemGrid>();
            arrMake[index] = grid;
            grid.Init(index);
        }
    }

    private void InitMsg() {
        Send.RegisterMsg(SendType.MakeChange, OnMakeChange);
    }

    private void ClearMsg() {
        Send.UnregisterMsg(SendType.MakeChange, OnMakeChange);
    }

    private void RefreshWindow() {
        OnMakeChange();
    }

    private void OnMakeChange(params object[] objs) {
        int length = Mathf.Min(makeInfo.arrAlchemy.Length, makeInfo.alchemyNum);
        for (int i = 0; i < length; i++ ) {
            arrMake[i].SetData(makeInfo.arrAlchemy[i]);
        }
        for (int i = length; i < makeInfo.alchemyNum;i++ ) {
            arrMake[i].Clear();
        }
    }
}
