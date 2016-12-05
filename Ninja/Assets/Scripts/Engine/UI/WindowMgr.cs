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

        if (window.hasOpen)
            return;

        if (window.windowInfo.group != 0) {
            CloseGroupWindow(window.windowInfo.group);
        }

        openList.Add(winName);
        window.transform.SetAsLastSibling();
        window.DoOpen();
    }

    public void CloseGroupWindow(int group) {
        List<string> tempList = new List<string>(openList);
        for (int index = 0; index < tempList.Count; index++) {
            string name = tempList[index];
            if (allList[name].windowInfo.group == group) {
                allList[name].CloseWindow();
            }
        }
    }

    public void CloseWindow<T>() {
        string winName = typeof(T).Name;
        CloseWindow(winName);
    }

    public void CloseWindow(string winName) {
        BaseWindow window = GetWindow(winName);
        if (window == null) {
            Debug.LogError("open window fail window is null " + winName);
            return;
        }

        if (!window.hasOpen)
            return;

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
