﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DunGen
{
    [AddComponentMenu("DunGen/Random Props/Random Prefab")]
    public class RandomPrefab : RandomProp
    {
        public GameObjectChanceTable Props = new GameObjectChanceTable();


        public override void Process(System.Random randomStream, Tile tile)
        {
            if (Props.Weights.Count <= 0)
                return;

			var chosenEntry = Props.GetRandom(randomStream, tile.Placement.IsOnMainPath, tile.Placement.NormalizedDepth, null, true, true);
			var prefab = chosenEntry.Value;

            GameObject newProp = (GameObject)GameObject.Instantiate(prefab);
            newProp.transform.parent = transform;
            newProp.transform.localPosition = Vector3.zero;
            newProp.transform.localRotation = Quaternion.identity;

            foreach (var childProp in newProp.GetComponentsInChildren<RandomPrefab>())
                childProp.Process(randomStream, tile);
        }
    }
}