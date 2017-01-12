using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnemyProxy : MonoBehaviour {
    public RefEnemy data;
    private Image imgIcon;
    public int hp;
    public int atk;

    public void Init() {
        imgIcon = gameObject.GetComponent<Image>();
        data = RefEnemy.GetRandomEnemy();

        RefIcon.SetSprite(imgIcon, data.Icon);
        hp = data.Hp;
        atk = data.Atk;
    }

    public void DoAtk(){
        BattleMgr.Instance.playerInfo.Hp -= atk;
        hp -= BattleMgr.Instance.playerInfo.Atk;
        if (hp < 0) {
            BattleMgr.Instance.playerInfo.Exp++;
            //to do 掉落
            RoomCreatMgr.Instance.RemoveGameObject(gameObject);
        }
    }
}
