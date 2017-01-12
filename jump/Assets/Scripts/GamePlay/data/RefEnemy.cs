using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RefEnemy : RefBase {
    public static Dictionary<int, RefEnemy> cacheMap = new Dictionary<int, RefEnemy>();

    public int Id;
    public string Icon;
    public int Hp;
    public int Atk;

    public override string GetFirstKeyName() {
        return "Id";
    }

    public override void LoadByLine(Dictionary<string, string> _value, int _line) {
        base.LoadByLine(_value, _line);
        Id = GetInt("Id");
        Icon = GetString("Icon");
        Hp = GetInt("Hp");
        Atk = GetInt("Atk");
    }

    public static RefEnemy GetRef(int _id) {
        RefEnemy data = null;
        if (cacheMap.TryGetValue(_id, out data)) {
            return data;
        }

        if (data == null) {
            Debug.LogError("error RefEnemy key:" + _id);
        }
        return data;
    }

    public static RefEnemy GetRandomEnemy() {
        List<RefEnemy> enemyList = new List<RefEnemy>();
        foreach (RefEnemy enemy in cacheMap.Values) {
            enemyList.Add(enemy);
        }

        return enemyList[Random.Range(0, enemyList.Count)];
    }
}
