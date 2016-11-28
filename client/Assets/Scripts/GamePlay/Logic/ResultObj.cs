using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResultObj {
    private List<BaseData> targetList;
    private ResultType resultType;
    private int value;

    public ResultObj(List<BaseData> _targetList, ResultType _resultType, int _value) {
        targetList = _targetList;
        resultType = _resultType;
        value = _value;
    }

    public string GetResultDesc() {
        string desc = "";
        for (int index = 0; index < targetList.Count; index++ ) {
            BaseData curTarget = targetList[index];
            desc = desc + RefLanguage.GetValue(curTarget.roleName) + " ";
        }
        desc = desc + RefLanguage.GetValue(resultType.ToString()) + value;
        return desc;
    }
}
