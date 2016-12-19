using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DunGen;

public class BattleWindow : BaseWindowWrapper<BattleWindow> {

    public Slider sldHP;
    public Slider sldMP;
    public MiniMapProxy miniMapProxy;

    protected override void InitCtrl() {
    }

    protected override void OnPreOpen() {
        RefreshWindow();
    }

    protected override void OnOpen() {
    }

    protected override void OnPreClose() {
        miniMapProxy.Clear();
    }

    protected override void InitMsg() {
        Send.RegisterMsg(SendType.PlayerHpChange, OnHpChange);
        Send.RegisterMsg(SendType.PlayerMpChange, OnMpChange);
        Send.RegisterMsg(SendType.EnterRoom, OnEnterRoom);
    }

    protected override void ClearMsg() {
        Send.UnregisterMsg(SendType.PlayerHpChange, OnHpChange);
        Send.UnregisterMsg(SendType.PlayerMpChange, OnMpChange);
        Send.UnregisterMsg(SendType.EnterRoom, OnEnterRoom);
    }

    private void RefreshWindow() {
        sldHP.value = 1;
        sldMP.value = 1;
    }

    private void OnHpChange(object []objs) {
        float cur = (float)objs[0];
        float max = (float)objs[1];

        sldHP.value = cur / max;
    }

    private void OnMpChange(object[] objs) {
        float cur = (float)objs[0];
        float max = (float)objs[1];

        sldMP.value = cur / max;
    }

    public void RefreshMap() {
        miniMapProxy.RefreshMap();
    }

    public void OnEnterRoom(object[] objs) {
        RoomInfo _roomInfo = (RoomInfo)objs[0];
        miniMapProxy.EnterRoom(_roomInfo);
    }
}
