using UnityEngine;
using System.Collections;

public class EnemyAI {
    private Enemy enemy;
    private float shootDelay = 2f;
    private float curTime = 0;
    private bool awake = false;

    public EnemyAI(Enemy _enemy) {
        enemy = _enemy;
    }

    public void IsAwake(bool _awake) {
        awake = _awake;
    }

    public void Update() {
        if (awake == false)
            return;
        if (enemy.CanShot() == false)
            return;

        curTime += Time.deltaTime;
        if ( curTime >= shootDelay ) {
            enemy.Shot( Random.Range(0, enemy.shotList.Count) );
            curTime = 0;
        }
    }
}
