using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RefResult : RefBase {

    public static Dictionary<int, RefResult> cacheMap = new Dictionary<int, RefResult>();

    public int ID;
    public RoleType Target;
    public ResultType ResultType;
    public ValueSource ValueSource;
    public int DurationTime;
    public List<int> ParamList;
    public List<int> WeightList;

    public override string GetFirstKeyName() {
        return "ID";
    }

    public override void LoadByLine(Dictionary<string, string> _value, int _line) {
        base.LoadByLine(_value, _line);
        ID = GetInt("ID");
        Target = (RoleType)GetEnum("Target", typeof(RoleType));
        ResultType = (ResultType)GetEnum("ResultType", typeof(ResultType));
        ValueSource = (ValueSource)GetEnum("ValueSource", typeof(ValueSource));
        DurationTime = GetInt("DurationTime");
        ParamList = GetList<int>("ParamList", typeof(int));
        WeightList = GetList<int>("WeightList", typeof(int));
    }

    public int GetResultValue(int actionValue = 0) {
        int value = 0;
        switch(ValueSource){
            case ValueSource.Fixed:
                value = ParamList[0];
                break;
            case ValueSource.RandomOne:
                value = ToolMgr.Instance.RandomWithWeight(ParamList, WeightList);
                break;
            case ValueSource.RandomRange:
                value = ToolMgr.Instance.RandomRange(ParamList);
                break;
            case ValueSource.Passed:
                if (actionValue == 0) {
                    Debug.LogError("param is 0");
                }
                value = actionValue;
                break;
            default:
                Debug.LogError("noknow type:" + ValueSource);
                break;
        }
        return value;
    }

    public static RefResult GetRef(int _id) {
        RefResult data = null;
        if (cacheMap.TryGetValue(_id, out data)) {
            return data;
        }

        if (data == null) {
            Debug.LogError("error RefAction key:" + _id);
        }
        return data;
    }
}
