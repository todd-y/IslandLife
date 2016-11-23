using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WindowMgr : Singleton<WindowMgr> {

    public Dictionary<string, BaseWindow> allList = new Dictionary<string, BaseWindow>();
    public List<string> openList = new List<string>();

	public void Init(){
		
	}
	
	public void Clear(){
		
	}

    public void OpenWindow<T>() where T : BaseWindow {
        string winName = typeof(T).Name;
        BaseWindow window = GetWindow(winName);
        if (window == null) {
            Debug.LogError("open window fail window is null " + winName);
            return;
        }

        openList.Add(winName);
        window.DoOpen();
    }

    public void CloseWindow<T>() {
        string winName = typeof(T).Name;
        BaseWindow window = GetWindow(winName);
        if (window == null) {
            Debug.LogError("open window fail window is null " + winName);
            return;
        }

        openList.Remove(winName);
        window.DoClose();
    }

    public void CloseWindow(string winName) {
        BaseWindow window = GetWindow(winName);
        if (window == null) {
            Debug.LogError("open window fail window is null " + winName);
            return;
        }

        openList.Remove(winName);
        window.DoClose();
    }

    public BaseWindow GetWindow(string name) {
        BaseWindow window = null;
        if (allList.ContainsKey(name)) {
            window = allList[name];
        }
        else {
            window = InstantiateWin(name);
            allList.Add(name, window);
        }

        return window;
    }

    private BaseWindow InstantiateWin(string winName) {
        BaseWindow window = null;
        GameObject winPrefab = LocalAssetMgr.Instance.Load_UI(winName);
        if (winPrefab == null) {
            Debug.LogError(string.Format("无法获得窗口{0}资源！", winName));
            return window;
        }
        GameObject winGo = GameObject.Instantiate(winPrefab) as GameObject;
        winGo.name = "Win_" + winName;
        winGo.SetActive(false);
        window = winGo.GetComponent<BaseWindow>();
        WinSetParent(window);
        return window;
    }

    private void WinSetParent(BaseWindow window) {
        UIRootTwoD.Instance.SortWindow(window.transform, window.windowInfo.windowType);
    }

}
