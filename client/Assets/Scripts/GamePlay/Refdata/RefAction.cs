using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RefAction : RefBase {
    public static Dictionary<Action, RefAction> cacheMap = new Dictionary<Action, RefAction>();

    public Action Action;
    public ActionType ActionType;
    public List<RoleType> ExecutorList;
    public List<RoleType> TargetList;
    public int NeedTime;
    public int NeedFood;
    public int NeedCachet;
    public List<int> ResultIDList;

    public override string GetFirstKeyName() {
        return "Action";
    }

    public override void LoadByLine(Dictionary<string, string> _value, int _line) {
        base.LoadByLine(_value, _line);
        Action = (Action)GetEnum("Action", typeof(Action));
        ActionType = (ActionType)GetEnum("ActionType", typeof(ActionType));
        ExecutorList = GetList<RoleType>("ExecutorList", typeof(RoleType));
        TargetList = GetList<RoleType>("TargetList", typeof(RoleType));
        NeedTime = GetInt("NeedTime");
        NeedFood = GetInt("NeedFood");
        NeedCachet = GetInt("NeedCachet");
        ResultIDList = GetList<int>("ResultIDList", typeof(int));
    }

    public string GetCostDesc() {
        string desc = "";
        if (NeedFood != 0) {
            desc += RefLanguage.GetValueParam("FoodCost", NeedFood);
        }

        if (NeedCachet != 0) {
            desc += " " + RefLanguage.GetValueParam("CachetCost", NeedCachet);
        }

        if (NeedTime != 0) {
            desc += " " + RefLanguage.GetValueParam("NeedTimeCost", NeedTime);
        }
        return desc;
    }

    public static RefAction GetRef(Action _id) {
        RefAction data = null;
        if (cacheMap.TryGetValue(_id, out data)) {
            return data;
        }

        if (data == null) {
            Debug.LogError("error RefAction key:" + _id);
        }
        return data;
    }

    public static List<RefAction> GetTypeList(ActionType _type) {
        List<RefAction> actionList = new List<RefAction>();
        foreach (RefAction action in cacheMap.Values) {
            if (action.ActionType == _type) {
                actionList.Add(action);
            }
        }
        return actionList;
    }
}
