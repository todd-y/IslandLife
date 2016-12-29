using UnityEngine;
using System.Collections;

public class WindowInfo : MonoBehaviour {

    public WindowType windowType = WindowType.Normal;
    public OpenAnimType animType = OpenAnimType.None;
    public float animTime = 0.3f;
    public bool closeOnEmpty = false;
    public bool mask = false;
    public Vector3 defaultPos = Vector3.zero;
    public Vector3 openPos = Vector3.zero;
    public int group = 0;
}
