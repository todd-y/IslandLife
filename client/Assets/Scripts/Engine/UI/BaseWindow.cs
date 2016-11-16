using UnityEngine;
using System.Collections;

public class BaseWindow : MonoBehaviour {
    private WindowInfo m_windowInfo = null;
    public WindowInfo windowInfo {
        get { return m_windowInfo ?? (m_windowInfo = this.GetComponent<WindowInfo>()); }
    }

    protected void Awake() {
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
        OnPreOpen();
        InitMsg();
        this.gameObject.SetActive(true);
        PlayOpenAnim();
    }

    public void DoClose(bool needPlay = true) {
        ClearMsg();
        OnPreClose();
        if (needPlay) {
            PlayCloseAnim();
        }
        else {
            OnClose();
        }
    }

    public virtual void OnPreOpen() {

    }

    public virtual void OnOpen() {

    }

    public virtual void OnPreClose() {

    }

    public virtual void OnClose() {

    }

    public virtual void InitMsg() {

    }

    public virtual void ClearMsg() {

    }

    public void PlayOpenAnim() {
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

    public void PlayCloseAnim() {
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
}
