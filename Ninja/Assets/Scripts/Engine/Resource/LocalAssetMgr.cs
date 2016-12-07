using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.SceneManagement;

/// <summary>
/// 本地资源管理器
/// 
/// 先下载资源到本地，不是边玩边下载
/// </summary>
public class LocalAssetMgr : Singleton<LocalAssetMgr> {
    private const string basic_ui = "ui/";
    private const string basic_refdata = "refdata/";

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
        path = string.Format("Assets/Atlas/ui/Sprite/{0}/{1}.png", pack, name );
        sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
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

//    public void UnloadScene () {
//        WWWDownloadUtilty.Instance.ClearAllByPre("scene");
//    }

//    public void UnloadMob () {
//        WWWDownloadUtilty.Instance.ClearAllByPre("mob");
//    }

//    public GameObject Load_Mob (string name, bool isAddEventRec = true) {
//        if (mobList.ContainsKey(name)) {
//            mobList[name].count++;
//        }
//        else {
//            mobList.Add(name, new MobInfo(name));
//        }

//        GameObject prefab = null;

//#if MJ_DEBUG
//        if (DebugScene.ForceRaceName && DebugScene.isTestEnter) {
//            name = DebugScene.UseRaceName;
//        }

//        if (DefineUtily.IsEditor() && DebugAsset.MobPrb_Local) {
//            string path = local_mob + name + ".prefab";
//            prefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(path);
//            if (null == prefab)
//                Debug.LogError("Failed MobPrb_Local from " + path);
//        }

//#endif
//        if (null == prefab) {
//            string assetbundleName = name;
//            string assetName = name;

//            // 从表里找出assetbundle名称
//            RefAssetbundle abInfo = RefAssetbundle.GetRef(name);
//            if (null != abInfo) {
//                Debug.Log("RefAssetbundle has key: " + name);
//                assetbundleName = abInfo.Assetbundle;
//                prefab = AssetBundleMgr.Instance.LoadAsset<GameObject>(string.Format("mob/{0}.unity3d", assetbundleName), assetName);

//                if (null == prefab) {
//                    Debug.LogWarning(string.Format("RefAssetbundle has key {0}, but load from {1} failed", assetName, assetbundleName));
//                    prefab = AssetBundleMgr.Instance.LoadAsset<GameObject>(string.Format("mob/{0}.unity3d", name), name);
//                }
//            }
//            else
//                prefab = AssetBundleMgr.Instance.LoadAsset<GameObject>(string.Format("mob/{0}.unity3d", assetbundleName), assetName);
//        }

//        if (null == prefab) {
//            Debug.LogError("Load_Mob not find name : " + name);
//            return null;
//        }

//        GameObject ob = GameObject.Instantiate(prefab) as GameObject;

//        if (null == ob)
//            return ob;

//        try {
//            Renderer matRenderer = ob.GetChildControl<Renderer>(name + "/" + name);
//            if (matRenderer != null) {
//                matRenderer.sharedMaterial.SetFloat("_OutlineWidth", GeneralDefine.Instance.OutLineSize);
//            }
//        }
//        catch (System.Exception ex) {
//            Debug.LogException(ex);
//        }

//        if (isAddEventRec) {
//            Transform child = ob.GetChildControl<Transform>(name);
//            if (child != null) {
//                Send.SendMsg(SendType.AddAnimEventRec, child.gameObject);
//            }
//            else {
//                Debug.LogError("模型" + name + "子节点名称和父节点名称呢个不一致");
//            }
//        }

//        SkinnedMeshRenderer render = ob.GetChildControl<SkinnedMeshRenderer>(name + "/" + name);
//        if (render != null) {
//            render.quality = SkinQuality.Bone2;
//#if !Close_DEBUG
//            bool closeShadows = DebugQuality.DeactivateShadowDefault;
//            render.castShadows = !closeShadows;
//            render.receiveShadows = !closeShadows;
//#endif
//        }

//        return ob;
//    }

//    public void CheckMobList () {
//        if (mobList.Count > MobMaxLength) {
//            List<MobInfo> list = new List<MobInfo>(mobList.Values);
//            list.Sort(SortMob);
//            int length = list.Count - MobSaveLength;
//            for (int index = 0; index < length; index++) {
//                MobInfo info = list[index];
//                if (info == null) {
//                    Debug.LogError("信息为空！");
//                }
//                else {
//                    mobList.Remove(info.mobName);
//                    RefAssetbundle abInfo = RefAssetbundle.GetRef(info.mobName);
//                    if (null != abInfo) {
//                        WWWDownloadUtilty.Instance.ClearByShortUrl(string.Format("mob/{0}.unity3d", abInfo.Assetbundle));
//                    }
//                    else {
//                        WWWDownloadUtilty.Instance.ClearByShortUrl(string.Format("mob/{0}.unity3d", info.mobName));
//                    }
//                }
//            }

//            foreach (MobInfo info in mobList.Values) {
//                if (info != null) {
//                    info.count = 1;
//                }
//            }
//        }
//    }

//    //排序 小的在前面
//    protected int SortMob (MobInfo a, MobInfo b) {
//        if (a == null)
//            return 1;
//        if (b == null)
//            return -1;
//        return a.count.CompareTo(b.count);
//    }

//    public IEnumerator CO_LoadMob (System.Action<GameObject> _onComplete, string name, bool isAddEventRec = true) {
//        if (mobList.ContainsKey(name)) {
//            mobList[name].count++;
//        }
//        else {
//            mobList.Add(name, new MobInfo(name));
//        }

//        GameObject prefab = null;

//#if MJ_DEBUG
//        if (DebugScene.ForceRaceName && DebugScene.isTestEnter) {
//            name = DebugScene.UseRaceName;
//        }

//        if (DefineUtily.IsEditor() && DebugAsset.MobPrb_Local) {
//            string path = local_mob + name + ".prefab";
//            prefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(path);
//            yield return null;
//            if (null == prefab)
//                Debug.LogError("Failed MobPrb_Local from " + path);
//        }

//#endif
//        if (null == prefab) {
//            string assetbundleName = name;
//            string assetName = name;

//            // 从表里找出assetbundle名称
//            RefAssetbundle abInfo = RefAssetbundle.GetRef(name);
//            if (null != abInfo) {
//                Debug.Log("RefAssetbundle has key: " + name);
//                assetbundleName = abInfo.Assetbundle;
//                yield return CoDelegator.Coroutine(AssetBundleMgr.Instance.Co_LoadAsset(delegate(GameObject go) { prefab = go; }
//                    , string.Format("mob/{0}.unity3d", assetbundleName)
//                    , assetName));

//                if (null == prefab) {
//                    Debug.LogWarning(string.Format("RefAssetbundle has key {0}, but load from {1} failed", assetName, assetbundleName));
//                    yield return CoDelegator.Coroutine(AssetBundleMgr.Instance.Co_LoadAsset(delegate(GameObject go) { prefab = go; }
//                        , string.Format("mob/{0}.unity3d", name)
//                        , name));
//                }
//            }
//            else {
//                yield return CoDelegator.Coroutine(AssetBundleMgr.Instance.Co_LoadAsset(delegate(GameObject go) { prefab = go; }
//                    , string.Format("mob/{0}.unity3d", assetbundleName)
//                    , assetName));
//            }
//        }

//        if (null == prefab) {
//            Debug.LogError("Load_Mob not find name : " + name);
//            yield break;
//        }

//        GameObject ob = GameObject.Instantiate(prefab) as GameObject;

//        if (null == ob)
//            yield break;

//        try {
//            Renderer matRenderer = ob.GetChildControl<Renderer>(name + "/" + name);
//            if (matRenderer != null) {
//                matRenderer.sharedMaterial.SetFloat("_OutlineWidth", GeneralDefine.Instance.OutLineSize);
//            }
//        }
//        catch (System.Exception ex) {
//            Debug.LogException(ex);
//        }

//        if (isAddEventRec) {
//            GameObject child = ob.GetChildControl<Transform>(name).gameObject;
//            Send.SendMsg(SendType.AddAnimEventRec, child);
//        }

//        SkinnedMeshRenderer render = ob.GetChildControl<SkinnedMeshRenderer>(name + "/" + name);
//        if (render != null) {
//            render.quality = SkinQuality.Bone2;
//#if !Close_DEBUG
//            bool closeShadows = DebugQuality.DeactivateShadowDefault;
//            render.castShadows = !closeShadows;
//            render.receiveShadows = !closeShadows;
//#endif
//        }

//        if (_onComplete != null && ob != null) {
//            _onComplete(ob);
//        }
//    }

//    public GameObject Load_DungeonStatic (string name) {

//        GameObject prefab = AssetBundleMgr.Instance.LoadAsset<GameObject>(ab_dungeonStatic, name);
//        if (null == prefab) {
//            Debug.LogError("dungeon static pos not find name : " + name);
//            return null;
//        }
//        GameObject ob = GameObject.Instantiate(prefab) as GameObject;

//        return ob;
//    }

//    public AudioClip Load_Music (string name, bool fromLocal = false) {

//        AudioClip clip = null;

//        string path = "";

//        if (fromLocal) {
//            path = basic_music + name;
//            clip = Resources.Load(path) as AudioClip;
//            if (null == clip) {
//                Debug.LogError("Failed Load_Music from " + path + "." + name);
//            }

//            return clip;
//        }

//#if MJ_DEBUG
//        path = local_music + name + ".wav";
//        clip = UnityEditor.AssetDatabase.LoadAssetAtPath<AudioClip>(path);
//#endif
//        if (clip == null) {
//            clip = AssetBundleMgr.Instance.LoadAsset<AudioClip>("music.unity3d", name);
//        }

//        if (clip == null) {
//            clip = AssetBundleMgr.Instance.LoadAsset<AudioClip>("musicEx.unity3d", name);
//        }

//        if (clip == null) {
//            clip = AssetBundleMgr.Instance.LoadAsset<AudioClip>("musicfirst.unity3d", name);
//        }

//        return clip;
//    }

//    public AudioClip Load_Music_Bg (string name, bool fromLocal = false) {
//        AudioClip clip = null;

//        string path = "";

//        if (fromLocal) {
//            path = basic_music + name;
//            clip = Resources.Load(path) as AudioClip;
//            if (null == clip) {
//                Debug.LogError("Failed Load_Music from " + path + ".wav");
//            }

//            return clip;
//        }

//#if MJ_DEBUG
//        path = local_music + name + ".wav";
//        clip = UnityEditor.AssetDatabase.LoadAssetAtPath<AudioClip>(path);
//#endif
//        if (clip == null) {
//            clip = AssetBundleMgr.Instance.LoadAsset<AudioClip>("musicBg.unity3d", name);
//            //WWWDownloadUtilty.Instance.ClearByShortUrl("musicBg.unity3d");
//        }

//        if (clip == null) {
//            clip = AssetBundleMgr.Instance.LoadAsset<AudioClip>("musicBgEx.unity3d", name);
//            //WWWDownloadUtilty.Instance.ClearByShortUrl("musicBgEx.unity3d");
//        }

//        if (clip == null) {
//            clip = AssetBundleMgr.Instance.LoadAsset<AudioClip>("musicBgfirst.unity3d", name);
//            //WWWDownloadUtilty.Instance.ClearByShortUrl("musicBgfirst.unity3d");
//        }

//        return clip;
//    }

//    public GameObject Load_Effect (string name) {

//        // 本地读取
//        GameObject prefab = null;
//        if (DefineUtily.IsEditor() && DebugAsset.EffectPrb_Local) {
//            string path = local_effect + name + ".prefab";
//            prefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(path);
//            if (null == prefab) {
//                Debug.LogError("EffectPrb_Local effect not find name : " + name);
//            }
//        }

//        // 打包读取
//        if (null == prefab)
//            prefab = AssetBundleMgr.Instance.LoadAsset<GameObject>(effectName, name);

//        if (null == prefab)
//            prefab = AssetBundleMgr.Instance.LoadAsset<GameObject>(effectExName, name);

//        if (null == prefab)
//            prefab = AssetBundleMgr.Instance.LoadAsset<GameObject>(effectFirstName, name);

//        if (null == prefab) {
//            Debug.LogError("effect not find name : " + name);
//            return null;
//        }

//        // 缓存
//        GameObject effect = GameObject.Instantiate(prefab) as GameObject;

//        return effect;
//    }

//    public GameObject GetEffect (string name) {
//        // 优先取缓存
//        if (effectList.ContainsKey(name)) {
//            List<GameObject> list = effectList[name];
//            for (int i = list.Count - 1; i >= 0; --i) {
//                GameObject go = list[i];
//                if (go == null) {
//                    list.RemoveAt(i);
//                    continue;
//                }
//                if (!go.activeSelf) {
//                    go.SetActive(true);
//                    //拖尾时间处理
//                    Send.SendMsg(SendType.EffectCtrlHandle, go);
//                    CoDelegator.Coroutine(DelayParticleSystem(go));
//                    return go;
//                }
//            }
//        }
//        else {
//            effectList.Add(name, new List<GameObject>());
//        }

//        GameObject effect = Load_Effect(name);

//        //effect.SetActive(false);
//        //CoDelegator.Coroutine(DelayActiveGo(name, effect));

//        effectList[name].Add(effect);
//        Send.SendMsg(SendType.ParticleSystemListHandle, effect, true);

//        return effect;
//    }

//    public void PreLoadEffect (string name) {
//        if (effectList.ContainsKey(name)) {
//            return;
//        }
//        effectList.Add(name, new List<GameObject>());

//        GameObject effect = Load_Effect(name);
//        effectList[name].Add(effect);
//        effect.SetActive(false);

//        Send.SendMsg(SendType.ParticleSystemListHandle, effect, true);
//    }

//    public GameObject Load_UIEffect (string name) {
//        GameObject prefab = null;
//        string path = "";

//        if (DefineUtily.IsEditor() && DebugAsset.UIPrb_Local) {
//            path = local_effect + name + ".prefab";
//            prefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(path);
//        }
//        if (prefab == null) {
//            prefab = Resources.Load<GameObject>("game/effect/" + name);
//        }
//        if (prefab == null) {
//            prefab = AssetBundleMgr.Instance.LoadAsset<GameObject>(effectName, name);
//        }

//        if (prefab == null) {
//            prefab = AssetBundleMgr.Instance.LoadAsset<GameObject>(effectExName, name);
//        }

//        if (prefab == null) {
//            prefab = AssetBundleMgr.Instance.LoadAsset<GameObject>(effectFirstName, name);
//        }

//        if (null == prefab) {
//            Debug.LogError("effect not find name : " + name);
//            return null;
//        }

//        if (prefab != null) {
//            AudioSource sound = prefab.GetComponentInChildrenFast<AudioSource>();
//            if (sound != null) {
//                Send.SendMsg(SendType.ChangeAudioSource, sound);
//            }
//        }
//        return prefab;
//    }

//    public IEnumerator CO_LoadUIEffect (System.Action<GameObject> _onComplete, string name) {
//        GameObject prefab = null;
//        string path = "";

//        if (DefineUtily.IsEditor() && DebugAsset.UIPrb_Local) {
//            path = local_effect + name + ".prefab";
//            prefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(path);
//        }

//        if (prefab == null) {
//            prefab = Resources.Load<GameObject>("game/effect/" + name);
//        }

//        if (prefab == null) {
//            yield return CoDelegator.Coroutine(AssetBundleMgr.Instance.Co_LoadAsset(delegate(GameObject go) { prefab = go; }
//                , effectName, name));
//        }

//        if (prefab == null) {
//            yield return CoDelegator.Coroutine(AssetBundleMgr.Instance.Co_LoadAsset(delegate(GameObject go) { prefab = go; }
//                , effectExName, name));
//        }

//        if (prefab == null) {
//            yield return CoDelegator.Coroutine(AssetBundleMgr.Instance.Co_LoadAsset(delegate(GameObject go) { prefab = go; }
//                , effectFirstName, name));
//        }

//        if (prefab != null) {
//            AudioSource sound = prefab.GetComponentInChildrenFast<AudioSource>();
//            if (sound != null) {
//                Send.SendMsg(SendType.ChangeAudioSource, sound);
//            }
//        }

//        if (_onComplete != null && prefab != null) {
//            _onComplete(prefab);
//        }
//    }

//    //从对象池中取特效时，带有粒子的特效要延迟一小段时间才能正常使用，否则会有残留粒子出现 unity自身的bug 除此之外尝试各种解决方案无效
//    private IEnumerator DelayParticleSystem (GameObject go) {
//        if (go == null)
//            yield break;

//        yield return null;

//        Send.SendMsg(SendType.DelayParticleSystemHandle, go);
//    }

//    //private IEnumerator DelayActiveGo (string name, GameObject go) {
//    //    yield return null;
//    //    if (go == null)
//    //        yield break;

//    //    if (effectList.ContainsKey(name)) {
//    //        effectList[name].Add(go);
//    //        Send.SendMsg(SendType.ActiveParticleGameObject, go);
//    //    }
//    //}

//    public void ClearAllEffect () {
//        effectList.Clear();
//    }

//    public class MobInfo {
//        public string mobName;
//        public int count;

//        public MobInfo (string _mobName) {
//            mobName = _mobName;
//            count = 1;
//        }
//    }
}
