using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

/// <summary>
/// 本地资源管理器
/// 
/// 先下载资源到本地，不是边玩边下载
/// </summary>
public class LocalAssetMgr : Singleton<LocalAssetMgr> {
    private const string basic_ui = "ui/";
    private const string basic_refdata = "refdata/";
    private const string basic_music = "Sound/";

    /// <summary>
    /// refdata
    /// </summary>
    public void Load_RefData (string name, System.Action<TextAsset> callback) {
        TextAsset tableText = null;

        string path = basic_refdata + name;
        tableText = Resources.Load<TextAsset>(path);

        if (tableText == null ) {
            Debug.LogError("Failed Load_RefData from " + path);
        }

        callback(tableText);
    }

    public GameObject Load_UI(string name) {
        GameObject prefab = null;

        string path = basic_ui + "Win_" + name;
        prefab = Resources.Load(path) as GameObject;
        if (prefab == null) {
            Debug.LogError("Failed UIPrb_Local from " + path);
        }

        return prefab;
    }

    public GameObject Load_Prefab(string name) {
        GameObject prefab = null;
        string subPath = "Prefab/";
        string path = subPath + name;
        prefab = Resources.Load(path) as GameObject;
        if (prefab == null) {
            Debug.LogError("Failed Load_UIPrefab from " + path);
        }
        return prefab;
    }

    public GameObject Load_UIPrefab(string name) {
        GameObject prefab = null;
        string subPath = "Prefab/";
        string path = basic_ui + subPath + name;
        prefab = Resources.Load(path) as GameObject;
        if (prefab == null) {
            Debug.LogError("Failed Load_UIPrefab from " + path);
        }
        return prefab;
    }

    public Sprite Load_UISprite(string pack, string name) {
        Sprite sprite = null;

        string path;
        path = string.Format("Assets/Atlas/{0}/{1}.png", pack, name );
        sprite = Resources.Load(path) as Sprite;
        if (sprite == null) {
            Debug.LogError("Load_UISprite: sprite = " + path + ", failed!");
        }

        return sprite;
    }

    // 加载场景
    public void Load_Scene(string name) {
        Debug.LogWarning("Load_Scene : " + name);
        SceneManager.LoadScene(name);
    }

    public AudioClip Load_Music(string name) {
        AudioClip clip = null;
        string path = "";
        path = basic_music + name;
        clip = Resources.Load(path) as AudioClip;
        if (null == clip) {
            Debug.LogError("Failed Load_Music from " + path + "." + name);
        }

        return clip;
    }
}
