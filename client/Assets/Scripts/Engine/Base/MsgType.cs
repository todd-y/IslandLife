﻿using UnityEngine;
using System.Collections;


public delegate void BroadcastCallBack (params object[] _objs);

/// <summary>
/// 消息类型枚举 不可重复
/// </summary>
public enum SendType {
    TimeUpdate = 1,
    SecondChange,
    DayChange,
    GameStateChange,
    ActionStateChange,
    ActionUpdate,
    FoodChange,
    PeopleNumChange,
    LoyaltyChange,
    ArmyChange,
}