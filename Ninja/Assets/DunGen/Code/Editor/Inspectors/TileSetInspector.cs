﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;

namespace DunGen.Editor
{
    [CustomEditor(typeof(TileSet))]
	public sealed class TileSetInspector : UnityEditor.Editor
	{
        private readonly List<bool> tileShowWeights = new List<bool>();
		private readonly List<List<bool>> lockPrefabShowWeights = new List<List<bool>>();
		private bool showLockPrefabs = false;


        private void OnEnable()
        {
			TileSet tileSet = target as TileSet;

            for (int i = 0; i < tileSet.TileWeights.Weights.Count; i++)
                tileShowWeights.Add(false);

			for (int i = 0; i < tileSet.LockPrefabs.Count; i++)
			{
				lockPrefabShowWeights.Add(new List<bool>());

				for (int j = 0; j < tileSet.LockPrefabs[i].LockPrefabs.Weights.Count; j++)
					lockPrefabShowWeights[i].Add(false);
			}
        }

        public override void OnInspectorGUI()
        {
            TileSet tileSet = target as TileSet;

            if (tileSet == null)
                return;

            EditorUtil.DrawGameObjectChanceTableGUI("Tile", tileSet.TileWeights, tileShowWeights);


			EditorGUILayout.BeginVertical("box");
			showLockPrefabs = EditorGUILayout.Foldout(showLockPrefabs, "Locked Door Prefabs");

			if(showLockPrefabs)
			{
				int toDeleteIndex = -1;

				for (int i = 0; i < tileSet.LockPrefabs.Count; i++)
				{
					var l = tileSet.LockPrefabs[i];

					EditorGUILayout.BeginVertical("box");

					EditorGUILayout.BeginHorizontal();

					l.SocketGroup = (DoorwaySocketType)EditorGUILayout.EnumPopup(l.SocketGroup);

					if (GUILayout.Button("x", EditorStyles.miniButton, EditorConstants.SmallButtonWidth))
						toDeleteIndex = i;

					EditorGUILayout.EndHorizontal();

					EditorUtil.DrawGameObjectChanceTableGUI("Prefab", l.LockPrefabs, lockPrefabShowWeights[i]);

					EditorGUILayout.EndVertical();
				}

				if(toDeleteIndex > -1)
				{
					tileSet.LockPrefabs.RemoveAt(toDeleteIndex);
					lockPrefabShowWeights.RemoveAt(toDeleteIndex);
				}

				EditorGUILayout.Space();
				EditorGUILayout.Space();
				EditorGUILayout.Space();

				if(GUILayout.Button("Add"))
				{
					tileSet.LockPrefabs.Add(new LockedDoorwayAssociation());
					lockPrefabShowWeights.Add(new List<bool>());
				}
			}

			EditorGUILayout.EndVertical();

            if (GUI.changed)
                EditorUtility.SetDirty(tileSet);
        }
	}
}
