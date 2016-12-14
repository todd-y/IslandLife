using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BattleWindow : BaseWindowWrapper<BattleWindow> {

    public Slider sldHP;
    public Slider sldMP;

    protected override void InitCtrl() {
    }

    protected override void OnPreOpen() {
        RefreshWindow();
    }

    protected override void OnOpen() {
    }

    protected override void InitMsg() {
        Send.RegisterMsg(SendType.PlayerHpChange, OnHpChange);
        Send.RegisterMsg(SendType.PlayerMpChange, OnMpChange);
    }

    protected override void ClearMsg() {
        Send.UnregisterMsg(SendType.PlayerHpChange, OnHpChange);
        Send.UnregisterMsg(SendType.PlayerMpChange, OnMpChange);
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
}
