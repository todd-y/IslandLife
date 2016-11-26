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
    public Button btnSelect;

    private Text[] txtMapList;
    private Outline[] outLineList;
    private County m_curSelectCounty;
    private System.Action<County> callBack;
    private MapWindowState state = MapWindowState.Normal;

    public County CurSelectCounty {
        get {
            return m_curSelectCounty;
        }
        set {
            ClearOutLine();
            m_curSelectCounty = value;
            ShowSelectOutLine();
        }
    }

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

    public void OpenWindow(MapWindowState _mapState, County select = null, System.Action<County> _callBack = null) {
        state = _mapState;
        CurSelectCounty = select;
        callBack = _callBack;
        WindowMgr.Instance.OpenWindow<MapWindow>();
    }

    protected override void OnPreOpen() {
        RefreshButtonState();
        RrefreshMapInfo();
        RefreshInfo();
    }

    protected override void OnOpen() {
    }

    protected override void InitMsg() {
        btnClose.onClick.AddListener(CloseWindow);
        btnSelect.onClick.AddListener(SelectCounty);

        Send.RegisterMsg(SendType.DayChange, OnDayChange);
    }

    protected override void ClearMsg() {
        btnClose.onClick.RemoveListener(CloseWindow);
        btnSelect.onClick.RemoveListener(SelectCounty);

        Send.UnregisterMsg(SendType.DayChange, OnDayChange);
    }

    private void SelectArea(int index) {
        CurSelectCounty = BattleMgr.Instance.country.countyList[index];
        RefreshInfo();
    }

    private void RefreshButtonState() {
        btnSelect.gameObject.SetActive(state == MapWindowState.Select);
    }

    private void RrefreshMapInfo() {
        for (int index = 0; index < btnMapList.Length; index++ ) {
            txtMapList[index].setText(BattleMgr.Instance.country.countyList[index].roleName);
        }
    }

    private void RefreshInfo() {
        if (CurSelectCounty == null) {
            ClearInfo();
            return;
        }

        txtName.setText(CurSelectCounty.roleName);
        txtPeople.setText("PeopleNum", (int)CurSelectCounty.PeopleNum);
        txtRemainFood.setText("RemainFood", CurSelectCounty.RemainFood);
        txtArmy.setText("ArmyNum", (int)CurSelectCounty.ArmyNum);
        txtLoyalty.setText("LoyaltyDesc", (int)CurSelectCounty.Loyalty);
        txtCorruptionRate.setText("CorruptionRate", (int)CurSelectCounty.CorruptionRate);
        txtAreaFactor.setText("AreaFactor", CurSelectCounty.AreaFactor);
        txtOther.setText("Event");
    }

    private void ClearInfo() {
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

    private void ShowSelectOutLine() {
        if (m_curSelectCounty == null)
            return;

        int index = BattleMgr.Instance.country.countyList.IndexOf(m_curSelectCounty);
        outLineList[index].enabled = true;
    }

    private void OnDayChange(object[] objs) {
        RefreshInfo();
    }

    private void SelectCounty() {
        if (CurSelectCounty == null)
            return;

        if (callBack != null) {
            callBack(CurSelectCounty);
            CloseWindow();
        }
        else {
            //to do tip
        }
    }
}

public enum MapWindowState {
    Normal,
    Select,
}