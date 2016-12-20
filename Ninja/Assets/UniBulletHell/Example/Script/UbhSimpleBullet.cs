using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class UbhSimpleBullet : UbhMonoBehaviour
{
    public float _Speed = 10;
    [HideInInspector]
    public SpriteRenderer spriteRenderer;

    void OnEnable ()
    {
        rigidbody2D.velocity = transform.up.normalized * _Speed;
    }

    void OnTriggerEnter2D(Collider2D c) {
        HitCheck(c.transform);
    }

    void Update() {
        if (spriteRenderer.color == Color.green) {
            BattleMgr.Instance.curRoom.Draw(transform.localPosition, "branch31", false);
        }
        else {
            BattleMgr.Instance.curRoom.Draw(transform.localPosition, "branch41", false);
        }
    }

    private void HitCheck(Transform colTrans) {
        int colLayer = colTrans.gameObject.layer;
        if (colLayer == GeneralDefine.WallLayer) {
            if (spriteRenderer.color == Color.green) {
                BattleMgr.Instance.curRoom.Draw(transform.localPosition, "branch3");
            }
            else {
                BattleMgr.Instance.curRoom.Draw(colTrans.localPosition, "branch4");
            }

            UbhObjectPool.Instance.ReleaseGameObject(gameObject);
        }
    }

    public void SetColor(Color color) {
        if (spriteRenderer == null) {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>(true);
        }
        if (spriteRenderer == null) {
            Debug.Log("spriterenderer is null");
            return;
        }
        spriteRenderer.color = color;
    }

}