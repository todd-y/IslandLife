using UnityEngine;
using System.Collections;

public class GeneralDefine : Singleton<GeneralDefine> {

    public int gameSpeed = RefGeneral.GetIntValue("GameSpeed", 4);

    public float peopleFoodCost = 1f;
    public float stopArmyFoodCost = 1f;
    public float activeArmyFoodCost = 2f;
    public float kingBaseFoodCost = 10f;
}
