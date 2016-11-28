using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResultObj {
    private List<BaseData> targetList;
    private ResultType resultType;
    private int durationTime;
    private int value;

    public ResultObj(List<BaseData> _targetList, ResultType _resultType, int _value, int _durationTime) {
        targetList = _targetList;
        resultType = _resultType;
        value = _value;
        durationTime = _durationTime;
    }

    public string GetResultDesc() {
        string desc = "";
        for (int index = 0; index < targetList.Count; index++ ) {
            BaseData curTarget = targetList[index];
            desc = desc + RefLanguage.GetValue(curTarget.roleName) + " ";
        }
        desc = desc + RefLanguage.GetValue(resultType.ToString()) + value + " contine" + durationTime;
        return desc;
    }
}
