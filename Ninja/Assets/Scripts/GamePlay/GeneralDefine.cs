using UnityEngine;
using System.Collections;

public class GeneralDefine : Singleton<GeneralDefine> {
    public static readonly int WallLayer = LayerMask.NameToLayer("Wall");
    public static readonly int PlayerLayer = LayerMask.NameToLayer("Player");
    public static readonly int EnemyLayer = LayerMask.NameToLayer("Enemy");
    public static readonly int PlayerBulletLayer = LayerMask.NameToLayer("PlayerBullet");
    public static readonly int EnemyBulletLayer = LayerMask.NameToLayer("EnemyBullet");
    public static readonly int PlayerBackLayer = LayerMask.NameToLayer("PlayerBack");
}
