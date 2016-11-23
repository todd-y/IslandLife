using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ActionInfo {
    private const int DefaultActionID = 1000;
    public int actionID = 0;
    public RefAction action = null;
    public BaseData executor = null;
    public BaseData target = null;
    private ActionState m_actionState;
    private DateTime startTime;
    private DateTime curTime;
    private DateTime endTime;

    public ActionState ActionState {
        get {
            return m_actionState;
        }
        set {
            m_actionState = value;
            Send.SendMsg(SendType.ActionStateChange, this);
        }
    }

    public ActionInfo(RefAction _action) {
        m_actionState = ActionState.Prepare;
        action = _action;
        actionID = DefaultActionID + (int)action.Action;
        InitAction();
    }

    public void InitAction() {
        List<BaseData> executorList = BattleMgr.Instance.GetActionActorList(action.ExecutorList);
        List<BaseData> targetList = BattleMgr.Instance.GetActionActorList(action.TargetList);

        if (executorList.Count == 1) {
            executor = executorList[0];
        }

        if (targetList.Count == 1) {
            target = targetList[0];
        }
    }

    public bool StartAction(int newActionID) {
        if (executor == null || target == null) {
            Debug.LogError("actor is null " + executor + "||" + target);
            return false;
        }
        actionID = newActionID;
        startTime = BattleMgr.Instance.CurTime;
        curTime = startTime;
        endTime = startTime.AddDays(action.NeedTime);
        ActionState = ActionState.Doing;
        return true;
    }

    public void UpdateAction(float dt) {
        float dayPass = dt / BattleMgr.Instance.GetCurDaySecond();
        curTime = curTime.AddDays(dayPass);
        if (curTime > endTime) {
            FinishHandle();
        }

        Send.SendMsg(SendType.ActionUpdate, this);
    }

    private void FinishHandle() {
        ActionState = ActionState.Finish;
    }

    public float GetProgress() {
        if (ActionState == ActionState.Prepare)
            return 0f;

        return (float)((curTime - startTime).TotalHours / (endTime - startTime).TotalHours);
    }

    public string GetResultDesc() {
        return "result is dont work";
    }
}

public enum ActionState {
    Prepare,
    Doing,
    NoExecutor,
    NoTarget,
    Finish,
    
}
