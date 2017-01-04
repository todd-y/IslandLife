using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class RefIcon : RefBase {

    public static Dictionary<string, RefIcon> cacheMap = new Dictionary<string, RefIcon>();

    public string ID;
    public string AtlasName;
    public string SpriteName;

    public override string GetFirstKeyName () {
        return "ID";
    }

    public override void LoadByLine (Dictionary<string, string> _value, int _line) {
        base.LoadByLine(_value, _line);
        ID = GetString("ID");
        AtlasName = GetString("AtlasName");
        SpriteName = GetString("SpriteName");
    }

    public static RefIcon GetRef(string _id) {

        RefIcon data = null;
        if ( cacheMap.TryGetValue(_id, out data) ) {
            return data;
        }

        if (data == null) {
            Debug.LogError("error RefIcon key:" + _id);
        }
        return data;
    }

    public static void SetSprite(Image image, string iconId) {
        if(image == null)
            return;

        RefIcon iconData = GetRef(iconId);
        if (iconData == null)
            return;

        UITool.SetSprite(image, iconData.AtlasName, iconData.SpriteName);
    }

    public static void SetItemSprite(Image img, int itemId) {
        if (img == null)
            return;
        RefItem item = RefItem.GetRef(itemId);
        if (item == null)
            return;

        SetSprite(img, item.Icon);
    }
}
