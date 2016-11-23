/// <summary>
/// 官位
/// </summary>
public enum OfficialType {
    None = 0,
    King,
    Queen,
    Prince,
    Princess,
    LPM,// 左丞相
    RPM,// 右丞相
    OfficeSS,//吏部尚书
    OfficeSL,//吏部侍郎
    HouseSS,//户部
    HouseSL,
    RiteSS,//礼部
    RiteSL,
    ArmySS,//兵部
    ArmySL,
    PenalSS,//刑部
    PenalSL,
    WorkSS,//工部
    WorkSL,
    //to do 将领
}

/// <summary>
/// 头衔
/// </summary>
public enum TitleType {
    None = 0,
}

/// <summary>
/// 特性枚举
/// </summary>
public enum CharacteristicType {
    None = 0,
}

public enum GameState {
    Wait = 1,
    Prepare,
    Playing,
    Stop,
    End,
}

public enum GameSpeed {
    Normal = 1,
    Fast,
    VeryFast,
}

public enum Action{
    Read = 1,
    Harem,
    Travel,
    ExtraChooseWife,
    ExtraChooseMinister,
    AddMinister,
    ReduceMinister,
    Gift,
    AddWife,
    ReduceWife,
    Meeting,
    FixedMeeting,
    ChooseWife,
    ChooseMinister,
    OutlanderAttack,
    KingBirthDay,
    OtherBirthDay,
    PeasantUprising,    //农民起义
    GeneralsRebel,      //将领反叛
    IntroduceMinister,
    QuitMinister,
    MinisterMurder,
    DelateMinister,     //弹劾大臣
    ResearchArea,
    ResearchMinister,
    Murder,
    Seduce,//勾引
    RobFood,//抢夺食物
    ForgeGoodOmen,//伪造祥瑞
    FindGoodOmen, //发现祥瑞
    FindBadOmen,//发现噩兆
    ForgeRumor,//伪造谣言
}

public enum ActionType {
    DailyAction = 1,
    FixedAction,
    TriggerAction,
    SecretAction,
}

public enum RoleType {
    None = 1,
    King,
    Wife,
    Minister,
    Country,
    County,
    Gov,
    SecretAgent,
    TriggerActor,
    SelectActor,
}

public enum EffectAttribute {
    None = 0,
    Cachet,
    Loyalty,
    Food,
}

public enum LangType {
    Title,
    Desc,
}