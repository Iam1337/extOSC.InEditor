/* Copyright (c) 2021 dr. ext (Vladimir Sigalkin) */

using UnityEngine;

using UnityEditor;

using System;
using System.Collections.Generic;
using System.Linq;

using extOSC.InEditor;
using extOSC.Editor.Windows;

namespace extOSC.Editor.Panels.InEditor
{
	public class OSCPanelComponents : OSCPanel
	{
		#region Extensions

		private class Group
		{
			#region Public Vars

			public string Name;

			public readonly List<OSCEditorComponent> ReceiverComponents = new List<OSCEditorComponent>();

			public readonly List<OSCEditorComponent> TransmitterComponents = new List<OSCEditorComponent>();

			#endregion
		}

		#endregion

		#region Private Vars

		private List<Group> _groups = new List<Group>();

		private Color _defaultColor;

		//private string _localHost;

		private Vector2 _scrollPosition;

		#endregion

		#region Public Methods

		public OSCPanelComponents(OSCWindow parentWindow) : base(parentWindow)
		{
			foreach (var component in OSCEditorManager.Components)
			{
				var group = _groups.FirstOrDefault(g => g.Name == component.Group);
				if (group == null)
				{
					group = new Group();
					group.Name = component.Group;

					_groups.Add(group);
				}

				var receiverComponent = component as OSCEditorReceiverComponent;
				if (receiverComponent != null)
				{
					group.ReceiverComponents.Add(receiverComponent);
				}
				else
				{
					var transmitterComponent = component as OSCEditorTransmitterComponent;
					if (transmitterComponent != null)
					{
						group.TransmitterComponents.Add(transmitterComponent);
					}
				}
			}

			_groups.Sort((a, b) => string.Compare(a.Name, b.Name, StringComparison.Ordinal));
			_groups.ForEach(g =>
			{
				g.ReceiverComponents.Sort((a, b) => string.Compare(a.Name, b.Name, StringComparison.Ordinal));
				g.TransmitterComponents.Sort((a, b) => string.Compare(a.Name, b.Name, StringComparison.Ordinal));
			});
		}

		#endregion

		#region Protected Methods

		protected override void DrawContent(ref Rect contentRect)
		{
			_defaultColor = GUI.color;

			EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
			var connect = GUILayout.Button("Enable All", EditorStyles.toolbarButton);
			GUILayout.Space(5);
			var disconnect = GUILayout.Button("Disable All", EditorStyles.toolbarButton);
			GUILayout.FlexibleSpace();
			GUILayout.Label("OSC Editor Components");
			EditorGUILayout.EndHorizontal();

			if (connect || disconnect)
			{
				foreach (var component in OSCEditorManager.Components)
				{
					component.Active = connect;
				}
			}

			_scrollPosition = GUILayout.BeginScrollView(_scrollPosition);

			GUILayout.BeginHorizontal();
			GUILayout.Space(5);

			GUILayout.BeginVertical();
			GUILayout.Space(5);

			GUILayout.BeginVertical();

			foreach (var group in _groups)
			{
				DrawGroup(group);
			}

			GUILayout.EndVertical();

			GUILayout.Space(5);
			GUILayout.EndVertical();

			GUILayout.Space(5);
			GUILayout.EndHorizontal();

			GUILayout.EndScrollView();
		}

		#endregion

		#region Private Methods

		private void DrawGroup(Group group)
		{
			GUILayout.BeginVertical("box");

			GUI.color = Color.red;
			GUILayout.BeginVertical("box");
			GUILayout.Label(group.Name, OSCEditorStyles.CenterBoldLabel);
			GUILayout.EndVertical();
			GUI.color = _defaultColor;

			GUILayout.Space(5f);

			DrawComponents("Receiver Components:", group.ReceiverComponents);
			GUILayout.Space(5f);
			DrawComponents("Transmitter Components:", group.TransmitterComponents);

			//TODO: Transmitters Components;

			GUILayout.EndVertical();
		}

		private void DrawComponents(string title, List<OSCEditorComponent> components)
		{
			GUILayout.BeginVertical("box");

			GUI.color = Color.yellow;
			GUILayout.BeginVertical("box");
			GUILayout.Label(title, OSCEditorStyles.CenterBoldLabel);
			GUILayout.EndVertical();
			GUI.color = _defaultColor;

			GUILayout.Space(5f);

			foreach (var transmitterComponent in components)
			{
				DrawReceiverComponent(transmitterComponent);
			}

			GUILayout.EndVertical();
		}

		private void DrawReceiverComponent(OSCEditorComponent component)
		{
			var receiverComponent = component as OSCEditorReceiverComponent;
			var transmitterComponent = component as OSCEditorTransmitterComponent;

			var disabled = false;

			if (receiverComponent != null)
			{
				disabled = receiverComponent.Receiver == null;
			}

			if (transmitterComponent != null)
			{
				disabled = transmitterComponent.Transmitter == null;
			}


			if (disabled) GUI.color = Color.red;
			else GUI.color = component.Active ? Color.green : Color.yellow;

			var subColor = GUI.color;

			GUILayout.BeginVertical(EditorStyles.helpBox);

			GUILayout.BeginHorizontal(EditorStyles.helpBox);

			GUILayout.Label(component.Name);
			GUILayout.FlexibleSpace();

			GUI.color = component.Active ? Color.green : Color.red;
			var enable = GUILayout.Button(component.Active ? "Enabled" : "Disabled", GUILayout.Width(80),
			                              GUILayout.Height(16));
			GUI.color = subColor;

			GUILayout.EndHorizontal();

			GUI.color = _defaultColor;

			if (receiverComponent != null)
			{
				receiverComponent.Receiver = EditorReceiversPopup(receiverComponent.Receiver, null);
			}

			if (transmitterComponent != null)
			{
				transmitterComponent.Transmitter = EditorTransmittersPopup(transmitterComponent.Transmitter, null);
			}

			GUILayout.EndVertical();

			GUI.color = _defaultColor;


			if (enable)
			{
				component.Active = !component.Active;
			}
		}

		public OSCEditorReceiver EditorReceiversPopup(OSCEditorReceiver receiver, GUIContent content)
		{
			return EditorOSCPopup(extOSC.InEditor.OSCEditorUtils.GetReceivers(), receiver, content);
		}

		public OSCEditorTransmitter EditorTransmittersPopup(OSCEditorTransmitter transmitter, GUIContent content)
		{
			return EditorOSCPopup(extOSC.InEditor.OSCEditorUtils.GetTransmitters(), transmitter, content);
		}

		private T EditorOSCPopup<T>(Dictionary<string, T> dictionary, T osc, GUIContent content) where T : OSCEditorBase
		{
			T[] objects = null;
			string[] names = null;

			FillOSCArrays(dictionary, out names, out objects);

			var currentIndex = 0;
			var currentReceiver = osc;

			for (var index = 0; index < objects.Length; index++)
			{
				if (objects[index] == currentReceiver)
				{
					currentIndex = index;
					break;
				}
			}

			if (content != null)
			{
				var contentNames = new GUIContent[names.Length];

				for (var index = 0; index < names.Length; index++)
				{
					contentNames[index] = new GUIContent(names[index]);
				}

				currentIndex = EditorGUILayout.Popup(content, currentIndex, contentNames);
			}
			else
			{
				currentIndex = EditorGUILayout.Popup(currentIndex, names);
			}

			return objects[currentIndex];
		}

		private void FillOSCArrays<T>(Dictionary<string, T> dictionary, out string[] names, out T[] objects)
			where T : OSCEditorBase
		{
			var namesList = new List<string>();
			namesList.Add("- None -");
			namesList.AddRange(dictionary.Keys);

			var objectsList = new List<T>();
			objectsList.Add(null);
			objectsList.AddRange(dictionary.Values);

			names = namesList.ToArray();
			objects = objectsList.ToArray();
		}

		#endregion
	}
}