using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RefEffect : RefBase {

    public static Dictionary<int, RefEffect> cacheMap = new Dictionary<int, RefEffect>();

    public int ID;
    public RoleType Target;
    public EffectAttribute Attribute;
    public float Num;

    public override string GetFirstKeyName() {
        return "ID";
    }

    public override void LoadByLine(Dictionary<string, string> _value, int _line) {
        base.LoadByLine(_value, _line);
        ID = GetInt("ID");
        Target = (RoleType)GetEnum("Target", typeof(RoleType));
        Attribute = (EffectAttribute)GetEnum("Attribute", typeof(EffectAttribute));
        Num = GetFloat("Num");
    }

    public static RefEffect GetRef(int _id) {
        RefEffect data = null;
        if (cacheMap.TryGetValue(_id, out data)) {
            return data;
        }

        if (data == null) {
            Debug.LogError("error RefAction key:" + _id);
        }
        return data;
    }
}
