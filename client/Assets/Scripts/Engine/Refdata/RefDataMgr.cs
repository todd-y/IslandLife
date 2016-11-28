using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class RefDataMgr : BaseRefDataMgr<RefDataMgr> {
    /// <summary>
    /// 初始加载
    /// </summary>
    public IEnumerator Init () {
        Debug.Log("RefDataMgr Init Start!!!");

        List<IEnumerator> co_list = new List<IEnumerator>() {
            Co_LoadGeneric(RefIcon.cacheMap),
            Co_LoadGeneric(RefGeneral.cacheMap),
            Co_LoadGeneric(RefResult.cacheMap),
            Co_LoadGeneric(RefAction.cacheMap),
            Co_LoadGeneric(RefLanguage.cacheMap),
            Co_LoadGeneric(RefActor.cacheMap),
            Co_LoadGeneric(RefCountry.cacheMap),
            Co_LoadGeneric(RefCounty.cacheMap),
            Co_LoadGeneric(RefName.cacheMap),
        };
        for (int index = 0, total = co_list.Count; index < total; index++) {
            yield return CoDelegator.Coroutine(co_list[index]);
            //WinMsg.SendMsg(WinMsgType.ProcessLoad_Refdata, index, total, (index + 1.0f) / total);
        }
        Debug.Log("RefDataMgr Init End!!!");

        yield break;
    }

    public void InitBasic () {
        //LoadGeneric(RefLanguage.language, true);
        //LoadGeneric(RefDownloadTip.downloadtip, true);
        //LoadGeneric(RefAssetbundle.assetbundle, true);
        //LoadGeneric(RefPlatformZone.platformZoneMap, true);
    }
}
