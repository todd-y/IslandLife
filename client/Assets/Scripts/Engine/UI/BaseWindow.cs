using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BaseWindow : MonoBehaviour {
    private GameObject emptyClose;
    private GameObject mask;
    private WindowInfo m_windowInfo = null;
    public bool hasOpen = false;
    public WindowInfo windowInfo {
        get { return m_windowInfo ?? (m_windowInfo = this.GetComponent<WindowInfo>()); }
    }

    protected void Awake() {
        AddClose();
        AddMask();
        InitCtrl();
    }

    protected void OnDestory() {
        DestroyUI();
    }

    protected virtual void InitCtrl() {

    }

    protected virtual void DestroyUI() {

    }

    public void DoOpen() {
        if (hasOpen)
            return;
        hasOpen = true;
        OnPreOpen();
        InitMsg();
        this.gameObject.SetActive(true);
        PlayOpenAnim();
    }

    public void DoClose(bool needPlay = true) {
        if (!hasOpen)
            return;
        hasOpen = false;
        ClearMsg();
        OnPreClose();
        if (needPlay) {
            PlayCloseAnim();
        }
        else {
            OnClose();
        }
    }

    protected virtual void OnPreOpen() {

    }

    protected virtual void OnOpen() {

    }

    protected virtual void OnPreClose() {

    }

    protected virtual void OnClose() {
        this.gameObject.SetActive(false);
    }

    protected virtual void InitMsg() {

    }

    protected virtual void ClearMsg() {

    }

    private void PlayOpenAnim() {
        iTween.Stop(gameObject);
        switch (windowInfo.animType) {
            case OpenAnimType.None:
                OnOpen();
                break;
            case OpenAnimType.Position:
                iTween.MoveTo(gameObject, iTween.Hash("position", windowInfo.openPos, "time", windowInfo.animTime, "islocal", true, "oncomplete", "OnOpen"));
                break;
            case OpenAnimType.Scale:
                Debug.LogError("未实现");
                break;
            case OpenAnimType.Alpha:
                Debug.LogError("未实现");
                break;
            case OpenAnimType.ScaleAndAlpha:
                Debug.LogError("未实现");
                break;
            case OpenAnimType.Custom:
                Debug.LogError("未实现");
                break;
        }
    }

    private void PlayCloseAnim() {
        iTween.Stop(gameObject);
        switch (windowInfo.animType) {
            case OpenAnimType.None:
                OnClose();
                break;
            case OpenAnimType.Position:
                iTween.MoveTo(gameObject, iTween.Hash("position", windowInfo.defaultPos, "time", windowInfo.animTime, "islocal", true, "oncomplete", "OnClose"));
                break;
            case OpenAnimType.Scale:
                Debug.LogError("未实现");
                break;
            case OpenAnimType.Alpha:
                Debug.LogError("未实现");
                break;
            case OpenAnimType.ScaleAndAlpha:
                Debug.LogError("未实现");
                break;
            case OpenAnimType.Custom:
                Debug.LogError("未实现");
                break;
        }
    }

    private void AddMask() {
        if (windowInfo.windowType != WindowType.Modal)
            return;

        if (mask == null) {
            mask = Instantiate(LocalAssetMgr.Instance.Load_UIPrefab("WindowMask")) as GameObject;
            mask.layer = this.gameObject.layer;
            mask.name = "Mask";
            mask.transform.SetParent(transform, false);
            mask.transform.SetAsFirstSibling();
        }
    }

    private void AddClose() {
        if (windowInfo.closeOnEmpty == false)
            return;

        if (emptyClose == null) {
            emptyClose = Instantiate(LocalAssetMgr.Instance.Load_UIPrefab("WindowClose")) as GameObject;
            emptyClose.layer = this.gameObject.layer;
            emptyClose.name = "EmptyClose";
            emptyClose.transform.SetParent(transform, false);
            emptyClose.transform.SetAsFirstSibling();

            Button button = emptyClose.GetComponent<Button>();
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(CloseWindow);
        }
    }

    public virtual void CloseWindow() {
        WindowMgr.Instance.CloseWindow(this.GetType().Name);
    }
}
