using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DunGen
{
    [AddComponentMenu("DunGen/Runtime Dungeon")]
	public class RuntimeDungeon : MonoBehaviour
	{
        public DungeonGenerator Generator = new DungeonGenerator();
        public bool GenerateOnStart = true;


        protected virtual void Start()
        {


            //just for test
            if (GenerateOnStart)
                Generator.Generate();
        }
	}
}
