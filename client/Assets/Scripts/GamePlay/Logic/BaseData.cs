﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BaseData {
    //public string iconName = "";
    public RoleType roleType = RoleType.None;
    public string roleName = "";

    List<BuffObj> buffList = new List<BuffObj>();

    public virtual void AddBuff(ResultType resultType, int value) {
        
    }
}
