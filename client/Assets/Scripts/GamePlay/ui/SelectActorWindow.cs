using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class SelectActorWindow : BaseWindowWrapper<SelectActorWindow> {

    public ScrollRect svSelectActor;
    public GameObject prefabSelectActor;
    private List<SelectActorProxy> proxyList = new List<SelectActorProxy>();
    private List<BaseData> dataList = null;
    private System.Action<Actor> callBack;
    private Actor curSelectActor = null;

    public void OpenWindow(List<BaseData> list, System.Action<Actor> _callBack) {
        dataList = list;
        callBack = _callBack;
        curSelectActor = null;
        WindowMgr.Instance.OpenWindow<SelectActorWindow>();
    }

    protected override void InitCtrl() {
    }

    protected override void OnPreOpen() {
        RefreshList();
    }

    protected override void OnOpen() {
    }

    protected override void InitMsg() {
    }

    protected override void ClearMsg() {
    }

    private void RefreshList() {
        svSelectActor.StopMovement();
        for (int index = dataList.Count; index < proxyList.Count; index++) {
            proxyList[index].ClearData();
        }

        for (int index = 0; index < dataList.Count; index++) {
            if (!(dataList[index] is Actor)) {
                Debug.LogError("date is not actor");
                continue;
            }
            Actor actor = (Actor)dataList[index];
            SelectActorProxy proxy;
            if (proxyList.Count > index) {
                proxy = proxyList[index];
            }
            else {
                GameObject canDoGo = GameObject.Instantiate<GameObject>(prefabSelectActor);
                if (canDoGo == null) {
                    Debug.LogError("canDoGo is null");
                    return;
                }
                canDoGo.transform.SetParent(svSelectActor.content, false);
                proxy = canDoGo.GetComponent<SelectActorProxy>();
                proxy.SetCallBack(SelectActorHandle);
                proxyList.Add(proxy);
            }
            proxy.SetData(actor);
        }

        svSelectActor.content.localPosition = Vector3.zero;
    }

    private void SelectActorHandle(Actor actor) {
        curSelectActor = actor;
    }

    public void ChooseHandle() {
        if (curSelectActor == null) {
            //to do tip
            return;
        }
        if (callBack != null) {
            callBack(curSelectActor);
            CloseWindow();
        }
        else {
            Debug.LogError("call back is null");
        }
    }
}
