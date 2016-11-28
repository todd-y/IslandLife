using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// 事件列表
/// </summary>
public class ActionInfoWindow : BaseWindowWrapper<ActionInfoWindow> {
    public Text txtTitle;
    public Text txtDesc;
    public Text txtCost;
    public Text txtActor;
    public Text txtTarget;
    public Button btnSelectActor;
    public Button btnSelectTarget;
    public Button btnStart;
    public Button btnLeave;
    public Button btnFinish;
    public Slider sldProgess;
    public Text txtResult;

    public ActionInfo actionInfo;
    private List<BaseData> executorList;
    private List<BaseData> targetList;

    public void OpenWindow(ActionInfo action) {
        if (actionInfo != null && actionInfo.actionID == action.actionID) {
            CloseWindow();
            return;
        }
        actionInfo = action;
        if (hasOpen) {
            RefreshWindow();
        }
        else {
            WindowMgr.Instance.OpenWindow<ActionInfoWindow>();
        }
    }

    protected override void InitCtrl() {
    }

    protected override void OnPreOpen() {
        RefreshWindow();
    }

    private void RefreshWindow() {
        RefreshInfo();
        RefreshList();
        RefreshBtnState();
        RefreshProgress();
    }

    private void RefreshBtnState() {
        switch(actionInfo.ActionState){
            case ActionState.Prepare:
                btnStart.gameObject.SetActive(true);
                btnLeave.gameObject.SetActive(true);
                sldProgess.gameObject.SetActive(false);
                btnFinish.gameObject.SetActive(false);
                txtResult.gameObject.SetActive(false);
                break;
            case ActionState.Doing:
            case ActionState.NoExecutor:
            case ActionState.NoTarget:
                btnStart.gameObject.SetActive(false);
                btnLeave.gameObject.SetActive(true);
                sldProgess.gameObject.SetActive(true);
                btnFinish.gameObject.SetActive(false);
                txtResult.gameObject.SetActive(false);
                break;
            case ActionState.Finish:
                btnStart.gameObject.SetActive(false);
                btnLeave.gameObject.SetActive(false);
                sldProgess.gameObject.SetActive(false);
                btnFinish.gameObject.SetActive(true);
                txtResult.gameObject.SetActive(true);
                txtResult.setText(actionInfo.GetResultDesc());
                break;
            default:
                Debug.LogError("no handle type :" + actionInfo.ActionState);
                break;
        }
    }

    protected override void OnOpen() {
    }

    protected override void InitMsg() {
        btnSelectActor.onClick.AddListener(OnSelectActor);
        btnSelectTarget.onClick.AddListener(OnSelectTarget);
        btnStart.onClick.AddListener(OnStartClick);
        btnLeave.onClick.AddListener(CloseWindow);
        btnFinish.onClick.AddListener(OnFinishClick);

        Send.RegisterMsg(SendType.ActionStateChange, OnActionStateChange);
        Send.RegisterMsg(SendType.ActionUpdate, OnActionUpdate);
    }

    protected override void ClearMsg() {
        btnSelectActor.onClick.RemoveListener(OnSelectActor);
        btnSelectTarget.onClick.RemoveListener(OnSelectTarget);
        btnStart.onClick.RemoveListener(OnStartClick);
        btnLeave.onClick.RemoveListener(CloseWindow);
        btnFinish.onClick.AddListener(OnFinishClick);

        Send.UnregisterMsg(SendType.ActionStateChange, OnActionStateChange);
        Send.UnregisterMsg(SendType.ActionUpdate, OnActionUpdate);
    }

    private void OnSelectActor() {
        if (executorList == null || executorList.Count == 0 || executorList.Count == 1) {
            //to do tip;
        }
        else {
            if (executorList[0].roleType == RoleType.County) {
                MapWindow.Instance.OpenWindow(MapWindowState.Select, (County)actionInfo.target, SelectTargetHandle);
            }
            else {
                SelectActorWindow.Instance.OpenWindow(executorList, SelectExecutorHandle);
            }
        }
    }

    private void SelectExecutorHandle(BaseData executor) {
        actionInfo.executor = executor;
        txtActor.setText(actionInfo.executor.roleName);
    }

    private void OnSelectTarget() {
        if (targetList == null || targetList.Count == 0 || targetList.Count == 1) {
            //to do tip;
        }
        else {
            if (targetList[0].roleType == RoleType.County) {
                MapWindow.Instance.OpenWindow(MapWindowState.Select, (County)actionInfo.target, SelectTargetHandle);
            }
            else {
                SelectActorWindow.Instance.OpenWindow(targetList, SelectTargetHandle);
            }
        }
    }

    private void SelectTargetHandle(BaseData target) {
        actionInfo.target = target;
        txtTarget.setText(actionInfo.target.roleName);
    }

    private void OnStartClick() {
        if (BattleMgr.Instance.StartAction(actionInfo)) {
            CloseWindow();
        }
        else {
            //to do tip
        }
    }


    private void OnFinishClick() {
        BattleMgr.Instance.CheckClearAction(actionInfo);
        CloseWindow();
    }

    private void RefreshInfo() {
        txtTitle.setText(actionInfo.action.Action + LangType.Title.ToString());
        txtDesc.setText(actionInfo.action.Action + LangType.Desc.ToString());
        txtCost.setText(actionInfo.action.GetCostDesc());
    }

    private void RefreshList() {
        if (actionInfo.executor != null) {
            txtActor.setText(actionInfo.executor.roleName);
        }
        else {
            executorList = BattleMgr.Instance.GetActionActorList(actionInfo.action.ExecutorList);
            if (executorList.Count == 0) {
                txtActor.setText("NoCanChoose");
            }
            else if (executorList.Count == 1) {
                txtActor.setText(executorList[0].roleName);
            }
            else {
                txtActor.setText("ChooseExecutor");
            }

        }

        if (actionInfo.target != null) {
            txtTarget.setText(actionInfo.target.roleName);
        }
        else {
            targetList = BattleMgr.Instance.GetActionActorList(actionInfo.action.TargetList);
            if (targetList.Count == 0) {
                txtTarget.setText("NoCanChoose");
            }
            else if (targetList.Count == 1) {
                txtTarget.setText(targetList[0].roleName);
            }
            else {
                txtTarget.setText("ChooseTarget");
            }
        }
    }

    private void OnActionStateChange(object[] objs) {
        ActionInfo changeInfo = (ActionInfo)objs[0];
        if (actionInfo == null || changeInfo.actionID != actionInfo.actionID)
            return;

        RefreshBtnState();
    }

    private void OnActionUpdate(object[] objs) {
        ActionInfo changeInfo = (ActionInfo)objs[0];
        if (actionInfo == null || changeInfo.actionID != actionInfo.actionID)
            return;

        RefreshProgress();
    }

    private void RefreshProgress() {
        sldProgess.value = actionInfo.GetProgress();
    }

    public override void CloseWindow() {
        actionInfo = null;
        base.CloseWindow();
    }
}
