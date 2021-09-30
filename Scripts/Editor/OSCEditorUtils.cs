/* Copyright (c) 2021 dr. ext (Vladimir Sigalkin) */

using UnityEditor;

using System;
using System.Collections.Generic;

namespace extOSC.InEditor
{
	public static class OSCEditorUtils
	{
		#region Static Public Methods

		public static string GetName(OSCEditorBase editorBase)
		{
			if (editorBase is OSCEditorReceiver receiver)
			{
				return $"Editor Receiver: {receiver.LocalPort}";
			}

			if (editorBase is OSCEditorTransmitter transmitter)
			{
				return $"Editor Transmitter: {transmitter.RemoteHost}:{transmitter.RemotePort}";
			}

			return "- none -";
		}

		public static Dictionary<string, OSCEditorReceiver> GetReceivers()
		{
			var dictionary = new Dictionary<string, OSCEditorReceiver>();

			foreach (var receiver in OSCEditorManager.Receivers)
			{
				dictionary.Add(GetName(receiver), receiver);
			}

			return dictionary;
		}

		public static Dictionary<string, OSCEditorTransmitter> GetTransmitters()
		{
			var dictionary = new Dictionary<string, OSCEditorTransmitter>();

			foreach (var transmitter in OSCEditorManager.Transmitters)
			{
				dictionary.Add(GetName(transmitter), transmitter);
			}

			return dictionary;
		}

		public static Type[] GetTypes(Type type)
		{
			var types = new List<Type>();
			var guids = AssetDatabase.FindAssets("t:" + nameof(MonoScript));

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