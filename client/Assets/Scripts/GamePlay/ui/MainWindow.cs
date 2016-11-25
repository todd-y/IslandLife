using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// 主界面
/// </summary>
public class MainWindow : BaseWindowWrapper<MainWindow> {
    //info
    public Text txtTime;
    public Text txtTimeState;
    public Text txtGold;
    public Text txtTaxRate;
    public Text txtRecruitmentRate;//征兵率
    public Text txtPeople;
    public Text txtArmy;
    public Text txtLoyalty;
    //action
    public Button btnCourtiers;
    public Button btnWife;
    public Button btnSercet;
    public Button btnPolicy;
    public Button btnDaily;
    //actionList
    public ScrollRect svDoing;
    public ScrollRect svCanDo;
    public ScrollRect svActor;
    public GameObject prefebDoing;
    public GameObject prefabCanDo;
    public GameObject prefabActor;

    private Button btnTimeState;

    private List<RefAction> canDoActionList = new List<RefAction>();
    private List<CanDoProxy> canDoProxyList = new List<CanDoProxy>();

    private List<DoingPorxy> doingPorxyList = new List<DoingPorxy>();

    private List<Actor> selectActorList = new List<Actor>();
    private List<ActorPorxy> actorProxyList = new List<ActorPorxy>();

    private CurSelectType selectType = CurSelectType.None;

    //data
    public Country Country {
        get {
            return BattleMgr.Instance.country;
        }
    }

    protected override void InitCtrl() {
        prefebDoing.SetActive(false);
        prefabCanDo.SetActive(false);
        btnTimeState = txtTimeState.GetComponent<Button>();
    }

    protected override void OnPreOpen() {
        selectType = CurSelectType.None;
        RefreshWindow();
    }

    protected override void OnOpen() {
    }

    protected override void InitMsg() {
        btnTimeState.onClick.AddListener(ChangeGameState);
        btnCourtiers.onClick.AddListener(OnCourtiersClick);
        btnWife.onClick.AddListener(OnWifeClick);
        btnSercet.onClick.AddListener(OnSercetClick);
        btnPolicy.onClick.AddListener(OnPolicyClick);
        btnDaily.onClick.AddListener(OnDailyClick);

        Send.RegisterMsg(SendType.DayChange, OnDayChange);
        Send.RegisterMsg(SendType.GameStateChange, OnGameStateChange);
        Send.RegisterMsg(SendType.TimeUpdate, OnTimeUpdate);
    }

    protected override void ClearMsg() {
        btnTimeState.onClick.RemoveListener(ChangeGameState);
        btnCourtiers.onClick.RemoveListener(OnCourtiersClick);
        btnWife.onClick.RemoveListener(OnWifeClick);
        btnSercet.onClick.RemoveListener(OnSercetClick);
        btnPolicy.onClick.RemoveListener(OnPolicyClick);
        btnDaily.onClick.RemoveListener(OnDailyClick);

        Send.UnregisterMsg(SendType.DayChange, OnDayChange);
        Send.UnregisterMsg(SendType.GameStateChange, OnGameStateChange);
        Send.UnregisterMsg(SendType.TimeUpdate, OnTimeUpdate);
    }

    private void RefreshWindow() {
        RefreshInfo();
    }

    private void RefreshInfo() {
        txtGold.setText(Country.RemainFood);
        txtTaxRate.setText(Country.TaxRate);
        txtRecruitmentRate.setText(Country.ArmyRate);
        txtPeople.setText(Country.PeopleNum());
        txtArmy.setText(Country.ArmyNum());
        txtLoyalty.setText(Country.PeopleLoyalty());
        txtTime.setText(BattleMgr.Instance.CurTime);
        txtTimeState.setText(BattleMgr.Instance.GameState.ToString());
    }

    private void ChangeGameState() {
        switch(BattleMgr.Instance.GameState){
            case GameState.Stop:
                BattleMgr.Instance.GameState = GameState.Playing;
                BattleMgr.Instance.GameSpeed = GameSpeed.Normal;
                break;
            case GameState.Playing:
                BattleMgr.Instance.GameState = GameState.Stop;
                BattleMgr.Instance.GameSpeed = GameSpeed.Fast;
                break;
            default:
                Debug.LogError("未处理的枚举：" + BattleMgr.Instance.GameState);
                break;
        }
    }

    private void OnDayChange(object []objs) {
        txtTime.setText(BattleMgr.Instance.CurTime);
    }

    private void OnGameStateChange(object[] objs) {
        txtTimeState.setText(BattleMgr.Instance.GameState.ToString());
    }

    private void OnTimeUpdate(object[] objs) {
        RefreshDoing();
    }

    private void OnCourtiersClick() {
        OnSelectClick(CurSelectType.Courtiers);
    }

    private void OnWifeClick() {
        OnSelectClick(CurSelectType.Wife);
    }

    private void OnPolicyClick() {
        OnSelectClick(CurSelectType.Policy);
    }

    private void OnSercetClick() {
        OnSelectClick(CurSelectType.Sercet);
    }

    private void OnDailyClick(){
        OnSelectClick(CurSelectType.Daily);
    }

    private void OnSelectClick(CurSelectType type) {
        if (selectType == CurSelectType.None || selectType != type) {
            selectType = type;
            switch(type){
                case CurSelectType.None:
                    break;
                case CurSelectType.Courtiers:
                    selectActorList = Country.ministerList;
                    canDoActionList = new List<RefAction>();
                    break;
                case CurSelectType.Wife:
                    selectActorList = Country.wifeList;
                    canDoActionList = new List<RefAction>();
                    break;
                case CurSelectType.Sercet:
                    selectActorList = new List<Actor>();
                    canDoActionList = RefAction.GetTypeList(ActionType.SecretAction);
                    break;
                case CurSelectType.Policy:
                    selectActorList = new List<Actor>();
                    canDoActionList = RefAction.GetTypeList(ActionType.PolicyAction);
                    break;
                case CurSelectType.Daily:
                    selectActorList = new List<Actor>();
                    canDoActionList = RefAction.GetTypeList(ActionType.DailyAction);
                    break;
            }
        }
        else {
            canDoActionList = new List<RefAction>();
            selectActorList = new List<Actor>();
            selectType = CurSelectType.None;
        }

        RefreshCanDo();
        RefreshActor();
    }

    private void RefreshCanDo() {
        svCanDo.gameObject.SetActive(canDoActionList.Count > 0);
        if (canDoActionList.Count == 0) 
            return;

        svCanDo.StopMovement();
        for (int index = canDoActionList.Count; index < canDoProxyList.Count; index++) {
            canDoProxyList[index].ClearData();
        }

        for (int index = 0; index < canDoActionList.Count; index++) {
            RefAction refAction = canDoActionList[index];
            CanDoProxy proxy;
            if (canDoProxyList.Count > index) {
                proxy = canDoProxyList[index];
            }
            else {
                GameObject canDoGo = GameObject.Instantiate<GameObject>(prefabCanDo);
                if (canDoGo == null) {
                    Debug.LogError("canDoGo is null");
                    return;
                }
                canDoGo.transform.SetParent(svCanDo.content, false);
                proxy = canDoGo.GetComponent<CanDoProxy>();
                canDoProxyList.Add(proxy);
            }
            proxy.SetData(refAction);
        }

        svCanDo.content.localPosition = Vector3.zero;
    }

    private void RefreshDoing() {
        svDoing.gameObject.SetActive(BattleMgr.Instance.actionList.Count > 0);
        if (BattleMgr.Instance.actionList.Count == 0)
            return;

        svDoing.StopMovement();
        for (int index = BattleMgr.Instance.actionList.Count; index < doingPorxyList.Count; index++) {
            doingPorxyList[index].ClearData();
        }

        for (int index = 0; index < BattleMgr.Instance.actionList.Count; index++) {
            ActionInfo actionInfo = BattleMgr.Instance.actionList[index];
            DoingPorxy proxy;
            if (doingPorxyList.Count > index) {
                proxy = doingPorxyList[index];
            }
            else {
                GameObject doingGo = GameObject.Instantiate<GameObject>(prefebDoing);
                if (doingGo == null) {
                    Debug.LogError("canDoGo is null");
                    return;
                }
                doingGo.transform.SetParent(svDoing.content, false);
                proxy = doingGo.GetComponent<DoingPorxy>();
                doingPorxyList.Add(proxy);
            }
            proxy.SetData(actionInfo);
        }

        svDoing.content.localPosition = Vector3.zero;
    }

    private void RefreshActor() {
        svActor.gameObject.SetActive(selectActorList.Count > 0);
        if (selectActorList.Count == 0)
            return;

        svActor.StopMovement();
        for (int index = selectActorList.Count; index < actorProxyList.Count; index++) {
            actorProxyList[index].ClearData();
        }

        for (int index = 0; index < selectActorList.Count; index++) {
            Actor actor = selectActorList[index];
            ActorPorxy proxy;
            if (actorProxyList.Count > index) {
                proxy = actorProxyList[index];
            }
            else {
                GameObject actorGo = GameObject.Instantiate<GameObject>(prefabActor);
                if (actorGo == null) {
                    Debug.LogError("actorGo is null");
                    return;
                }
                actorGo.transform.SetParent(svActor.content, false);
                proxy = actorGo.GetComponent<ActorPorxy>();
                actorProxyList.Add(proxy);
            }
            proxy.SetData(actor);
        }

        svActor.content.localPosition = Vector3.zero;
    }

    public enum CurSelectType {
        None = 0,
        Courtiers,
        Wife,
        Sercet,
        Policy,
        Daily,
    }
}
