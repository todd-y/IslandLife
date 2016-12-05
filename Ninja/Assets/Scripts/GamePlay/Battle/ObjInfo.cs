using UnityEngine;
using System.Collections;

public class ObjInfo : MonoBehaviour {

    public ObjType objType = ObjType.None;
    private ObjType defaultType;

    void Awake() {
        defaultType = objType;
    }

    public void ChangeType(ObjType _type) {
        objType = _type;
    }

    public void SetDefaultType() {
        objType = defaultType;
    }
}
