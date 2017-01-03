using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RefEquip : RefBase {
    public static Dictionary<int, RefEquip> cacheMap = new Dictionary<int, RefEquip>();

    public int Id;
    public EquipPosition position;
    public EquipType type;
    public Quality quality;
    public List<Attribute> attributeList;
    public List<int> valueList;

    public override string GetFirstKeyName() {
        return "Id";
    }

    public override void LoadByLine(Dictionary<string, string> _value, int _line) {
        base.LoadByLine(_value, _line);
        Id = GetInt("Id");
        position = (EquipPosition)GetEnum("position", typeof(EquipPosition));
        type = (EquipType)GetEnum("type", typeof(EquipType));
        quality = (Quality)GetEnum("quality", typeof(Quality));
        attributeList = GetList<Attribute>("attributeList", typeof(Attribute));
        valueList = GetList<int>("valueList", typeof(int));
    }

    public static RefEquip GetRef(int _id) {
        RefEquip data = null;
        if (cacheMap.TryGetValue(_id, out data)) {
            return data;
        }

        if (data == null) {
            Debug.LogError("error RefEquip key:" + _id);
        }
        return data;
    }
}
