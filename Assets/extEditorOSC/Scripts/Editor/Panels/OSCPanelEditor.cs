/* Copyright (c) 2018 ExT (V.Sigalkin) */

using UnityEngine;

using UnityEditor;

using System.Net;

using extOSC;
using extOSC.Editor;
using extOSC.Editor.Panels;
using extOSC.Editor.Windows;

using extEditorOSC.Core;


namespace extEditorOSC.Panels
{
	public class OSCPanelEditor : OSCPanel
	{
		#region Static Private Vars

		private static readonly GUIContent _localPortContent = new GUIContent("Local Port:");

		private static readonly GUIContent _localHostContent = new GUIContent("Local Host:");

		private static readonly GUIContent _remoteHostContent = new GUIContent("Remote Host:");

		private static readonly GUIContent _remotePortContent = new GUIContent("Remote Port:");

		//private static readonly GUIContent _mapBundleContent = new GUIContent("Map Bundle:");

		private static readonly GUIContent _controlsContent = new GUIContent("Controls:");

		#endregion

		#region Private Vars

		private Color _defaultColor;

		private string _localHost;

		private Vector2 _scrollPosition;

		#endregion

		#region Public Methods

		public OSCPanelEditor(OSCWindow parentWindow, string panelId) : base(parentWindow, panelId)
		{
			_localHost = OSCUtilities.GetLocalHost();
		}

		#endregion

		#region Protected Methods

		protected override void DrawContent(Rect contentRect)
		{
			_defaultColor = GUI.color;

			var wide = contentRect.width > 520;

			_scrollPosition = GUILayout.BeginScrollView(_scrollPosition);

			GUILayout.BeginHorizontal();
			GUILayout.Space(10);

			GUILayout.BeginVertical();
			GUILayout.Space(10);

			if (wide)
				GUILayout.BeginHorizontal();
			else
				GUILayout.BeginVertical();

			DrawReceiver(OSCEditorManager.Receiver, wide ? contentRect.width / 2f : 0);

			DrawTransmitter(OSCEditorManager.Transmitter);

			if (wide)
				GUILayout.EndHorizontal();
			else
				GUILayout.EndVertical();

			GUILayout.Space(10);
			GUILayout.EndVertical();

			GUILayout.Space(10);
			GUILayout.EndHorizontal();

			GUILayout.EndScrollView();
		}

		#endregion

		#region Private Methods

		private void DrawReceiver(OSCEditorReceiver receiver, float width)
		{
			if (width > 0)
				GUILayout.BeginVertical("box", GUILayout.Width(width));
			else
				GUILayout.BeginVertical("box");

			// SETTINGS BLOCK
			GUILayout.BeginVertical("box");

			EditorGUILayout.LabelField("Receiver Settings:", EditorStyles.boldLabel);

			// SETTINGS BOX
			GUILayout.BeginVertical("box");

			GUILayout.BeginHorizontal();
			EditorGUILayout.LabelField(_localHostContent, GUILayout.Width(EditorGUIUtility.labelWidth - 4));
			EditorGUILayout.SelectableLabel(_localHost, GUILayout.Height(EditorGUIUtility.singleLineHeight));
			GUILayout.EndHorizontal();

			// LOCAL PORT
			receiver.LocalPort = EditorGUILayout.IntField(_localPortContent, receiver.LocalPort);

			// MAP BUNDLE
			//EditorGUILayout.PropertyField(_mapBundleProperty, _mapBundleContent);

			// SETTINGS BOX END
			EditorGUILayout.EndVertical();

			// PARAMETERS BLOCK
			EditorGUILayout.BeginHorizontal("box");

			GUI.color = OSCEditorManager.AutoConnectReceiver ? Color.green : Color.red;
			if (GUILayout.Button("Auto Connect"))
			{
				OSCEditorManager.AutoConnectReceiver = !OSCEditorManager.AutoConnectReceiver;
			}
			GUI.color = _defaultColor;

			// PARAMETERS BLOCK END
			EditorGUILayout.EndHorizontal();

			// SETTINGS BLOCK END
			EditorGUILayout.EndVertical();

			// CONTROLS
			EditorGUILayout.LabelField(_controlsContent, EditorStyles.boldLabel);

			DrawControlls(receiver);

			// EDITOR BUTTONS
			GUI.color = Color.white;

			GUILayout.EndVertical();
		}

		private void DrawTransmitter(OSCEditorTransmitter transmitter)
		{
			GUILayout.BeginVertical("box");

			// SETTINGS BLOCK
			GUILayout.BeginVertical("box");

			EditorGUILayout.LabelField("Transmitter Settings:", EditorStyles.boldLabel);

			// SETTINGS BOX
			GUILayout.BeginVertical("box");
			EditorGUI.BeginChangeCheck();

			IPAddress tempAddress;

			var remoteFieldColor = IPAddress.TryParse(transmitter.RemoteHost, out tempAddress) ? _defaultColor : Color.red;

			// REMOTE HOST
			GUI.color = remoteFieldColor;
			transmitter.RemoteHost = EditorGUILayout.TextField(_remoteHostContent, transmitter.RemoteHost);
			GUI.color = _defaultColor;

			// REMOTE PORT
			transmitter.RemotePort = EditorGUILayout.IntField(_remotePortContent, transmitter.RemotePort);

			// MAP BUNDLE
			//EditorGUILayout.PropertyField(_mapBundleProperty, _mapBundleContent);

			// USE BUNDLE
			GUI.color = transmitter.UseBundle ? Color.green : Color.red;
			if (GUILayout.Button("Use Bundle"))
			{
				transmitter.UseBundle = !transmitter.UseBundle;
			}
			GUI.color = _defaultColor;

			// SETTINGS BOX END
			EditorGUILayout.EndVertical();

			// PARAMETETS BLOCK
			EditorGUILayout.BeginHorizontal("box");

			GUI.color = OSCEditorManager.AutoConnectTransmitter ? Color.green : Color.red;
			if (GUILayout.Button("Auto Connect"))
			{
				OSCEditorManager.AutoConnectTransmitter = !OSCEditorManager.AutoConnectTransmitter;
			}
			GUI.color = _defaultColor;

			// PARAMETERS BLOCK END
			EditorGUILayout.EndHorizontal();

			// SETTINGS BLOCK END
			EditorGUILayout.EndVertical();

			// CONTROLS
			EditorGUILayout.LabelField(_controlsContent, EditorStyles.boldLabel);

			DrawControlls(transmitter);

			// EDITOR BUTTONS
			GUI.color = Color.white;

			GUILayout.EndVertical();
		}

		private void DrawControlls(OSCEditorBase receiver)
		{
			EditorGUILayout.BeginHorizontal("box");

			GUI.color = receiver.IsAvaible ? Color.green : Color.red;
			var connection = GUILayout.Button(receiver.IsAvaible ? "Connected" : "Disconnected");

			GUI.color = Color.yellow;
			EditorGUI.BeginDisabledGroup(!receiver.IsAvaible);
			var reconect = GUILayout.Button("Reconnect");
			EditorGUI.EndDisabledGroup();

			EditorGUILayout.EndHorizontal();

			if (connection)
			{
				if (receiver.IsAvaible) receiver.Close();
				else receiver.Connect();
			}

			if (reconect)
			{
				if (receiver.IsAvaible) receiver.Close();

				receiver.Connect();
			}
		}

		#endregion
	}
}