﻿/* Copyright (c) 2018 ExT (V.Sigalkin) */

using UnityEditor;

using System;
using System.Collections.Generic;

using extOSC;

namespace extEditorOSC
{
	public static class OSCEditorExtensions
	{
		#region Static Public Methods

		public static Type[] GetTypes(Type type)
		{
			var types = new List<Type>();
			var guids = AssetDatabase.FindAssets("t:" + typeof(MonoScript).Name);

			foreach (var guid in guids)
			{
				var assetPath = AssetDatabase.GUIDToAssetPath(guid);

				var monoScript = AssetDatabase.LoadAssetAtPath<MonoScript>(assetPath);
				if (monoScript == null) continue;

				var componentType = monoScript.GetClass();
				if (componentType == null || !OSCUtilities.IsSubclassOf(componentType, type)) continue;

				types.Add(componentType);
			}

			return types.ToArray();
		}

		public static string GetTypeGUID(Type type)
		{
			var guids = AssetDatabase.FindAssets("t:" + typeof(MonoScript).Name);

			foreach (var guid in guids)
			{
				var assetPath = AssetDatabase.GUIDToAssetPath(guid);

				var monoScript = AssetDatabase.LoadAssetAtPath<MonoScript>(assetPath);
				if (monoScript == null) continue;

				var componentType = monoScript.GetClass();
				if (componentType == null || componentType != type) continue;

				return guid;
			}

			return string.Empty;
		}

		public static Type GetTypeByGUID(string guid)
		{
			var assetPath = AssetDatabase.GUIDToAssetPath(guid);
			if (string.IsNullOrEmpty(assetPath)) return null;

			var monoScript = AssetDatabase.LoadAssetAtPath<MonoScript>(assetPath);
			if (monoScript == null) return null;

			return monoScript.GetClass();
		}

		public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> callback)
		{
			if (enumerable == null || callback == null)
				return;

			foreach (var value in enumerable)
			{
				callback.Invoke(value);
			}
		}

		#endregion
	}
}