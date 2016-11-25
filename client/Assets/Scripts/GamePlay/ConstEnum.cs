/// <summary>
/// 官位
/// </summary>
public enum OfficialType {
    None = 0,
    King,
    Queen,
    Concubine,
    Hetaera,
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
    Officer,
    Manager,//总管
    Secret,// 密探
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
    CountryTaxRate,//国家税率
    CountryArmyRate,//国家征兵
    CombatCorruption,//打击腐败
    LifeStyle,//生活作风
    CountyTaxRate,//地方税率
    CountyArmyRate,//地方征兵
    TryAddFood,//巧立名目
    SelfRecommendation,//贤者自荐
    BuildCounty,//地方建设
}

public enum ActionType {
    DailyAction = 1,
    FixedAction,
    TriggerAction,
    SecretAction,
    PolicyAction,
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

public enum NameType {
    Country,
    County,
    FamilyName,
    FirstName,
}