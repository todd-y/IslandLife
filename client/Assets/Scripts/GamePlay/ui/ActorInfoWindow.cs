using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ActorInfoWindow : BaseWindowWrapper<ActorInfoWindow> {
    public SelectActorProxy actorPorxy;
    private Actor actor;

    public void OpenWindow(Actor _actor) {
        actor = _actor;
        if (hasOpen) {
            RefreshWindow();
        }
        else {
            WindowMgr.Instance.OpenWindow<ActorInfoWindow>();
        }
    }
    
    protected override void InitCtrl() {
    }

    protected override void OnPreOpen() {
        RefreshWindow();
    }

    protected override void OnOpen() {
    }

    protected override void InitMsg() {
    }

    protected override void ClearMsg() {
    }

    private void RefreshWindow() {
        actorPorxy.SetData(actor);
    }
}
