using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BuffObj {
    public BaseData target;
    public ResultType resultType;
    public int value;

    private DateTime startTime;
    private DateTime curTime;
    private DateTime endTime;
    private State state;

    public BuffObj(BaseData _target, ResultType _type, int _value, int _durationTime) {
        target = _target;
        resultType = _type;
        value = _value;
        startTime = BattleMgr.Instance.CurTime;
        curTime = startTime;
        endTime = startTime.AddDays(_durationTime);
        state = _durationTime == 0 ? State.Forever : State.Limit;
    }

    public void Update(float dt) {
        if (state != State.Limit)
            return;
        float dayPass = dt / BattleMgr.Instance.GetCurDaySecond();
        curTime = curTime.AddDays(dayPass);
        if (curTime > endTime) {
            state = State.End;
            ClearData();
        }
    }

    public void ClearData() {
        switch(target.roleType){
            case RoleType.Country:
                Debug.LogError("country buffobj is no handle");
                break;
            default:
                target.RemoveBuff(this);
                break;
        }
    }

    public enum State {
        Limit,
        Forever,
        End,
    }
}
