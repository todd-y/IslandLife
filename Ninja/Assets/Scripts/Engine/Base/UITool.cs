using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UITool{
    public static void SetSprite(Image image, string atlasName, string spriteName, bool setNative = false) {
        if (image == null)
            return;

        Sprite sprite = LocalAssetMgr.Instance.Load_UISprite(atlasName, spriteName);
        if (sprite == null) {
            Debug.LogError("setsprite is null" + atlasName + "/" + spriteName);
        }
        image.sprite = sprite;

        if (setNative) {
            image.SetNativeSize();
        }
    }
}
