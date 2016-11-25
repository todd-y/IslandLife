using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MapWindow : BaseWindowWrapper<MapWindow> {
    public Button[] btnMapList;
    public Text txtName;
    public Text txtPeople;
    public Text txtRemainFood;
    public Text txtArmy;
    public Text txtLoyalty;
    public Text txtCorruptionRate;
    public Text txtAreaFactor;
    public Text txtOther;
    public Button btnClose;

    private Text[] txtMapList;
    private Outline[] outLineList;
    private County curSelectCounty;

    protected override void InitCtrl() {
        int length = btnMapList.Length;
        txtMapList = new Text[length];
        outLineList = new Outline[length];
        for (int index = 0; index < length; index++) {
            Button btnCurMap = btnMapList[index];
            Image img = btnCurMap.GetComponent<Image>();
            outLineList[index] = btnCurMap.AddMissingComponent<Outline>();
            outLineList[index].effectDistance = new Vector2(2, 2);  //描边
            outLineList[index].enabled = false;
            outLineList[index].effectColor = Color.red;
            img.alphaHitTestMinimumThreshold = 0.5f;//精确碰撞
            txtMapList[index] = btnCurMap.GetComponentInChildren<Text>();
            int curIndex = index;
            btnCurMap.onClick.AddListener(() => SelectArea(curIndex));
        }
    }

    public void OpenWindow() {
        ClearOutLine();
        curSelectCounty = null;
        WindowMgr.Instance.OpenWindow<MapWindow>();
    }

    protected override void OnPreOpen() {
        RrefreshMapInfo();
        RefreshInfo();
    }

    protected override void OnOpen() {
    }

    protected override void InitMsg() {
        btnClose.onClick.AddListener(CloseWindow);

        Send.RegisterMsg(SendType.DayChange, OnDayChange);
    }

    protected override void ClearMsg() {
        btnClose.onClick.RemoveListener(CloseWindow);

        Send.UnregisterMsg(SendType.DayChange, OnDayChange);
    }

    private void SelectArea(int index) {
        ClearOutLine();
        outLineList[index].enabled = true;
        curSelectCounty = BattleMgr.Instance.country.countyList[index];
        RefreshInfo();
    }

    private void RrefreshMapInfo() {
        for (int index = 0; index < btnMapList.Length; index++ ) {
            txtMapList[index].setText(BattleMgr.Instance.country.countyList[index].roleName);
        }
    }

    private void RefreshInfo() {
        if (curSelectCounty == null) {
            ClearInfo();
            return;
        }

        txtName.setText(curSelectCounty.roleName);
        txtPeople.setText("PeopleNum", (int)curSelectCounty.PeopleNum);
        txtRemainFood.setText("RemainFood", curSelectCounty.RemainFood);
        txtArmy.setText("ArmyNum", (int)curSelectCounty.ArmyNum);
        txtLoyalty.setText("LoyaltyDesc", (int)curSelectCounty.Loyalty);
        txtCorruptionRate.setText("CorruptionRate", (int)curSelectCounty.CorruptionRate);
        txtAreaFactor.setText("AreaFactor", curSelectCounty.AreaFactor);
        txtOther.setText("Event");
    }

    private void ClearInfo() {
        ClearOutLine();
        txtName.setText("");
        txtPeople.setText("");
        txtRemainFood.setText("");
        txtArmy.setText("");
        txtLoyalty.setText("");
        txtCorruptionRate.setText("");
        txtAreaFactor.setText("");
        txtOther.setText("");
    }

    private void ClearOutLine() {
        for (int index = 0; index < outLineList.Length; index++ ) {
            outLineList[index].enabled = false;
        }
    }

    private void OnDayChange(object[] objs) {
        RefreshInfo();
    }
}
