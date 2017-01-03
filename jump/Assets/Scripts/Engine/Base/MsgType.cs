using UnityEngine;
using System.Collections;


public delegate void BroadcastCallBack (params object[] _objs);

/// <summary>
/// 消息类型枚举 不可重复
/// </summary>
public enum SendType {
    TimeUpdate = 1,
    PlayerHit,
    LvChange,
    ExpChange,
    HpChange,
    MpChange,
    AtkChange,
    GoldChange,
    SkillChange,
    ItemChange,
    EquipChange,
}