using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

/// <summary>
///  游戏主逻辑
/// </summary>
public class BattleMgr : Singleton<BattleMgr> {
    private int startYear = 9527;
    private DateTime meeting;
    private DateTime monthFoodCost;
    private DateTime chooseWife;
    private DateTime chooseMinister;
    private DateTime springHarvest;
    private DateTime autumnHarvest;
    private DateTime startTime;
    private DateTime m_curTime;

    public Country country;
    private GameState gameState = GameState.Wait;
    private GameSpeed gameSpeed = GameSpeed.Normal;
    private int curActionID = 2000;

    public List<ActionInfo> actionList = new List<ActionInfo>();

    public GameState GameState {
        get {
            return gameState;
        }
        set {
            gameState = value;
            Send.SendMsg(SendType.GameStateChange);
        }
    }

    public GameSpeed GameSpeed {
        get {
            return gameSpeed;
        }
        set {
            gameSpeed = value;
        }
    }

    public DateTime CurTime {
        get {
            return m_curTime;
        }
        set {
            m_curTime = value;
        }
    }

	public void Init(){
        InitDefaultTime();
        Send.RegisterMsg(SendType.TimeUpdate, OnTimeUpdate);
	}

    private void InitDefaultTime() {
        startTime = new DateTime(startYear, 1, 1);
        CurTime = startTime;
        meeting = new DateTime(startYear, 1, 1);
        monthFoodCost = new DateTime(startYear, 1, 10);
        chooseWife = new DateTime(startYear, 1, 15);
        chooseMinister = new DateTime(startYear, 6, 6);
        springHarvest = new DateTime(startYear, 3, 15);
        autumnHarvest = new DateTime(startYear, 9, 15);
        curActionID = 2000;
    }

    public void Clear() {
        Send.UnregisterMsg(SendType.TimeUpdate, OnTimeUpdate);
	}

    public void CreateBattle() {
        if (country != null) {
            Debug.LogError("has a battle now");
            return;
        }
        gameState = GameState.Stop;
        country = new Country();
        country.Init(1001);
        WindowMgr.Instance.OpenWindow<MainWindow>();
    }

    public void DestoryBattle() {
        gameState = GameState.End;
        country.Clear();
        country = null;
    }

    private void OnTimeUpdate(object[] _objs) {
        if (gameState != GameState.Playing) 
            return;

        int curDaySecond = GetCurDaySecond();
        float dt = Time.deltaTime;
        int day = CurTime.Day;
        CurTime = CurTime.AddDays(dt / GetCurDaySecond());
        UpdateAction(dt);
        if (day != CurTime.Day) {
            AddOneDay();
        }
        CheckTimedEvent();
    }

    private void UpdateAction(float dt) {
        for (int index = 0; index < actionList.Count; index++) {
            ActionInfo actionInfo = actionList[index];
            if (actionInfo.ActionState == ActionState.Doing) {
                actionInfo.UpdateAction(dt);
            }
        }
    }

    public int GetCurDaySecond() {
        int daySecond = GeneralDefine.Instance.gameSpeed;
        switch (GameSpeed) {
            case GameSpeed.Normal:
                break;
            case GameSpeed.Fast:
                daySecond = daySecond / 2;
                break;
            case GameSpeed.VeryFast:
                daySecond = daySecond / 4;
                break;
            default:
                break;
        }
        return daySecond;
    }

    private void AddOneDay() {
        if (country != null) {
            country.DailyUpdate();
        }
        else{
            Debug.LogError("country is null");
        }

        Send.SendMsg(SendType.DayChange);
    }

    private void CheckTimedEvent() {
        if (CurTime.Day == meeting.Day) {
            MeetingHandle();
        }

        if (CurTime.Day == monthFoodCost.Day) {
            country.MonthFoodCost();
        }

        if (CurTime.Month == chooseWife.Month && CurTime.Day == chooseWife.Day) {
            ChooseWifeHandle();
        }

        if (CurTime.Month == chooseMinister.Month && CurTime.Day == chooseMinister.Day) {
            ChooseMinisterHandle();
        }

        if (CurTime.Month == springHarvest.Month && CurTime.Day == springHarvest.Day) {
            country.HarvestHandle();
        }

        if (CurTime.Month == autumnHarvest.Month && CurTime.Day == autumnHarvest.Day) {
            country.HarvestHandle();
        }
    }

    private void MeetingHandle() {

    }

    private void ChooseWifeHandle() {

    }

    private void ChooseMinisterHandle() {

    }

    public List<BaseData> GetActionActorList(List<RoleType> roleList) {
        List<BaseData> actorList = new List<BaseData>();

        for (int index = 0; index < roleList.Count; index++ ) {
            RoleType roleType = roleList[index];
            actorList.AddRange(GetActionActorList(roleType));
        }
        return actorList;
    }

    public List<BaseData> GetActionActorList(RoleType roleType, BaseData executor = null, BaseData target = null) {
        List<BaseData> actorList = new List<BaseData>();
        switch (roleType) {
            case RoleType.King:
                actorList.Add(country.king);
                break;
            case RoleType.Wife:
                for (int k = 0; k < country.wifeList.Count; k++) {
                    actorList.Add(country.wifeList[k]);
                }
                break;
            case RoleType.Minister:
                for (int k = 0; k < country.ministerList.Count; k++) {
                    actorList.Add(country.ministerList[k]);
                }
                break;
            case RoleType.Country:
                actorList.Add(country);
                break;
            case RoleType.County:
                for (int k = 0; k < country.countyList.Count; k++) {
                    actorList.Add(country.countyList[k]);
                }
                break;
            case RoleType.Gov:
                actorList.Add(country);
                break;
            case RoleType.SecretAgent:
                for (int k = 0; k < country.secretAgentList.Count; k++) {
                    actorList.Add(country.secretAgentList[k]);
                }
                break;
            case RoleType.TriggerActor:
                if (executor != null) {
                    actorList.Add(executor);
                }
                else {
                    Debug.LogError("executor is null");
                }
                break;
            case RoleType.SelectActor:
                if (target != null) {
                    actorList.Add(target);
                }
                else {
                    Debug.LogError("target is null");
                }
                break;
            default:
                Debug.LogError("未处理的roletype:" + roleType);
                break;
        }

        return actorList;
    }

    public bool StartAction(ActionInfo action) {
        if (action.StartAction(curActionID)) {
            GameState = GameState.Playing;
            actionList.Add(action);
            curActionID++;
            return true;
        }

        return false;
    }

    public void CheckClearAction(ActionInfo action) {
        if (action == null)
            return;
        if (action.ActionState == ActionState.Finish) {
            actionList.Remove(action);
        }
    }

    public List<ResultObj> ResultListHandle(List<int> resultIDList, BaseData executor, BaseData target, int param) {
        List<ResultObj> resultList = new List<ResultObj>();
        for (int index = 0; index < resultIDList.Count; index++ ) {
            int effectID = resultIDList[index];
            RefResult refResult = RefResult.GetRef(effectID);
            if(refResult == null)
                continue;
            List<BaseData> targetList = GetActionActorList(refResult.Target, executor, target);
            if (targetList.Count == 0){
                Debug.LogError("targetList count is 0");
                continue;
            }
            int value = refResult.GetResultValue(param);
            if (value == 0) {
                Debug.LogError("value is 0");
            }

            for (int k = 0; k < targetList.Count; k++) {
                BaseData curTarget = targetList[k];
                curTarget.AddBuff(refResult.ResultType, value);
            }

            resultList.Add(new ResultObj(targetList, refResult.ResultType, value));
        }

        return resultList;
    }
}
