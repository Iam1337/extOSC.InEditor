/* Copyright (c) 2021 dr. ext (Vladimir Sigalkin) */

using UnityEngine;
using UnityEditor;

using System.Net;

using extOSC.Editor.Windows;
using extOSC.InEditor;

namespace extOSC.Editor.Panels.InEditor
{
	public class OSCPanelManager : OSCPanel
	{
		#region Static Private Vars

		private static readonly GUIContent _remoteHostContent = new GUIContent("Remote Host:");

		private static readonly GUIContent _remotePortContent = new GUIContent("Remote Port:");
		
		private static readonly GUIContent _localPortContent = new GUIContent("Local Port:");
		
		private static readonly GUIContent _localPortModeContent = new GUIContent("Local Port Mode:");

		private static readonly GUIContent _localHostContent = new GUIContent("Local Host:");

		private static readonly GUIContent _localHostModeContent = new GUIContent("Local Host Mode:");

		private static readonly GUIContent _advancedContent = new GUIContent("Advanced Settings:");

		//private static readonly GUIContent _mapBundleContent = new GUIContent("Map Bundle:");

		private static readonly GUIContent _inGameContent = new GUIContent("In Game Controls:");

		private static readonly GUIContent _inEditorContent = new GUIContent("In Editor Controls:");

		private static readonly GUIContent _receiverSettingsContent = new GUIContent("Receiver Settings:");

		private static readonly GUIContent _autoConnectContent = new GUIContent("Auto Connect");

		private static readonly GUIContent _closeOnPauseContent = new GUIContent("Close On Pause");

		//private static readonly GUIContent _mapBundleContent = new GUIContent("Map Bundle:");

		private static readonly GUIContent _controlsContent = new GUIContent("Controls:");

		#endregion

		#region Private Vars

		private Color _defaultColor;

		private string _localHost;

		private Vector2 _scrollPosition;

		#endregion

		#region Public Methods

		public OSCPanelManager(OSCWindow parentWindow) : base(parentWindow)
		{
			_localHost = OSCUtilities.GetLocalHost();
		}

		#endregion

		#region Protected Methods

		protected override void DrawContent(ref Rect contentRect)
		{
			_defaultColor = GUI.color;

			using (new GUILayout.HorizontalScope(EditorStyles.toolbar))
			{
				var connect = GUILayout.Button("Connect All", EditorStyles.toolbarButton);
				GUILayout.Space(5);
				var disconnect = GUILayout.Button("Disconnect All", EditorStyles.toolbarButton);
				GUILayout.FlexibleSpace();
				GUILayout.Label("OSC Editor Manager");
				
				if (connect || disconnect)
				{
					foreach (var receiver in OSCEditorManager.Receivers)
					{
						if (!receiver.IsStarted && connect) receiver.Connect();
						if (receiver.IsStarted && disconnect) receiver.Close();
					}

					foreach (var transmitter in OSCEditorManager.Transmitters)
					{
						if (!transmitter.IsStarted && connect) transmitter.Connect();
						if (transmitter.IsStarted && disconnect) transmitter.Close();
					}
				}
			}

			using (var scroll = new GUILayout.ScrollViewScope(_scrollPosition))
			{
				using (new GUILayout.HorizontalScope())
				{
					GUILayout.Space(5);

					using (new GUILayout.VerticalScope())
					{
						GUILayout.Space(5);

						using (new GUILayout.VerticalScope())
						{
							DrawReceivers(OSCEditorManager.Receivers);
							GUILayout.Space(25f);
							DrawTransmitters(OSCEditorManager.Transmitters);
						}
					}
				}

				_scrollPosition = scroll.scrollPosition;
			}
		}

		#endregion

		#region Private Methods

		private void DrawReceivers(OSCEditorReceiver[] receivers)
		{
			using (new GUILayout.VerticalScope(OSCEditorStyles.Box))
			{

				GUI.color = Color.red;
				using (new GUILayout.VerticalScope(OSCEditorStyles.Box))
				{
					GUILayout.Label("Receivers:", OSCEditorStyles.CenterBoldLabel);
				}
				GUI.color = _defaultColor;

				GUILayout.Space(5f);

				if (receivers.Length > 0)
				{
					OSCEditorReceiver removingReceiver = null;

					foreach (var receiver in receivers)
					{
						bool remove;

						DrawBase(receiver, out remove);
						GUILayout.Space(5f);

						if (remove) removingReceiver = receiver;
					}

					if (removingReceiver != null)
						OSCEditorManager.RemoveEditorReceiver(removingReceiver);
				}
				else
				{
					using (new GUILayout.VerticalScope(OSCEditorStyles.Box))
					{
						GUILayout.Label("- none -", OSCEditorStyles.CenterLabel);
					}
				}

				GUILayout.Space(5f);

				using (new GUILayout.VerticalScope(OSCEditorStyles.Box))
				{
					GUI.color = Color.green;
					var addButton = GUILayout.Button("Add Receiver");
					GUI.color = _defaultColor;
					
					if (addButton)
					{
						OSCEditorManager.CreateEditorReceiver();
					}
				}
			}
		}

		private void DrawTransmitters(OSCEditorTransmitter[] transmitters)
		{
			using (new GUILayout.VerticalScope(OSCEditorStyles.Box))
			{
				GUI.color = Color.red;
				using (new GUILayout.VerticalScope(OSCEditorStyles.Box))
				{
					GUILayout.Label("Transmitters:", OSCEditorStyles.CenterBoldLabel);
				}

				GUI.color = _defaultColor;

				GUILayout.Space(5f);

				if (transmitters.Length > 0)
				{
					OSCEditorTransmitter removingTransmitter = null;

					foreach (var transmitter in transmitters)
					{
						bool remove;

						DrawBase(transmitter, out remove);
						GUILayout.Space(5f);

						if (remove) removingTransmitter = transmitter;
					}

					if (removingTransmitter != null)
					{
						OSCEditorManager.RemoveEditorTransmitter(removingTransmitter);
					}
				}
				else
				{
					using (new GUILayout.VerticalScope(OSCEditorStyles.Box))
					{
						GUILayout.Label("- none -", OSCEditorStyles.CenterLabel);
					}
				}

				GUILayout.Space(5f);

				using (new GUILayout.VerticalScope(OSCEditorStyles.Box))
				{
					GUI.color = Color.green;
					var addButton = GUILayout.Button("Add Receiver");
					GUI.color = _defaultColor;

					if (addButton)
					{
						OSCEditorManager.CreateEditorTransmitter();
					}
				}
			}
		}

		private void DrawBase(OSCEditorBase editorBase, out bool remove)
		{
			var stateColor = (editorBase.IsStarted ? Color.green : Color.yellow) + new Color(0.75f, 0.75f, 0.75f);
			
			remove = false;

			GUI.color = stateColor;
			using (new GUILayout.VerticalScope(OSCEditorStyles.Box))
			{
				// OSC Name
				GUI.color = editorBase.IsStarted ? Color.green : Color.yellow;
				using (new GUILayout.VerticalScope(OSCEditorStyles.Box))
				{
					GUI.color = Color.white;
					GUILayout.Label(extOSC.InEditor.OSCEditorUtils.GetName(editorBase), OSCEditorStyles.CenterBoldLabel);
					GUI.color = stateColor;
				}
				GUI.color = stateColor;

				// OSC Settings
				GUI.color = Color.white;
				if (editorBase is OSCEditorReceiver receiver)
				{
					DrawReceiver(receiver);
				}
				else if (editorBase is OSCEditorTransmitter transmitter)
				{
					DrawTransmitter(transmitter);
				}
				GUI.color = stateColor;

				// OSC Control
				GUI.color = Color.white;
				using (new GUILayout.VerticalScope(OSCEditorStyles.Box))
				{
					EditorGUILayout.LabelField(_controlsContent, EditorStyles.boldLabel);
					DrawControls(editorBase, out remove);
				}
				GUI.color = stateColor;
			}

			GUI.color = _defaultColor;
		}

		private void DrawReceiver(OSCEditorReceiver receiver)
		{
			// SETTINGS BLOCK
			using (new GUILayout.VerticalScope(OSCEditorStyles.Box))
			{
				// RECEIVER SETTINGS
				EditorGUILayout.LabelField("Receiver Settings:", EditorStyles.boldLabel);
				using (new GUILayout.VerticalScope(OSCEditorStyles.Box))
				{
					if (receiver.LocalHostMode == OSCLocalHostMode.Any)
					{
						using (new GUILayout.HorizontalScope())
						{
							EditorGUILayout.LabelField(_localHostContent, GUILayout.Width(EditorGUIUtility.labelWidth - 4));
							EditorGUILayout.SelectableLabel(_localHost, GUILayout.Height(EditorGUIUtility.singleLineHeight));
						}
					}
					else
					{
						receiver.LocalHost = EditorGUILayout.TextField(_localHostContent, receiver.LocalHost);
					}
					
					receiver.LocalPort = EditorGUILayout.IntField(_localPortContent, receiver.LocalPort);
					// TODO: MapBundle field?
				}
				
				// ADVANCED BLOCK
				EditorGUILayout.LabelField(_advancedContent, EditorStyles.boldLabel);
				using (new GUILayout.VerticalScope(OSCEditorStyles.Box))
				{
					receiver.LocalHostMode = (OSCLocalHostMode) EditorGUILayout.EnumPopup(_localHostModeContent, receiver.LocalHostMode);
				}
			}
		}

		private void DrawTransmitter(OSCEditorTransmitter transmitter)
		{
			using (new GUILayout.VerticalScope(OSCEditorStyles.Box))
			{

				EditorGUILayout.LabelField("Transmitter Settings:", EditorStyles.boldLabel);
				using (new GUILayout.VerticalScope(OSCEditorStyles.Box))
				{
					transmitter.RemoteHost = EditorGUILayout.TextField(_remoteHostContent, transmitter.RemoteHost);
					transmitter.RemotePort = EditorGUILayout.IntField(_remoteHostContent, transmitter.RemotePort);
					// TODO: MapBundle field?
				}
				
				// ADVANCED BLOCK
				EditorGUILayout.LabelField(_advancedContent, EditorStyles.boldLabel);
				using (new GUILayout.VerticalScope(OSCEditorStyles.Box))
				{
					// Host
					transmitter.LocalHostMode = (OSCLocalHostMode)EditorGUILayout.EnumPopup(_localHostModeContent, transmitter.LocalHostMode);
					if (transmitter.LocalHostMode == OSCLocalHostMode.Any)
					{
						using (new GUILayout.HorizontalScope())
						{
							EditorGUILayout.LabelField(_localHostContent, GUILayout.Width(EditorGUIUtility.labelWidth - 4));
							EditorGUILayout.SelectableLabel(_localHost, GUILayout.Height(EditorGUIUtility.singleLineHeight));
						}
					}
					else
					{
						transmitter.LocalHost = EditorGUILayout.TextField(_localHostContent, transmitter.LocalHost);
					}

					// Port
					transmitter.LocalPortMode = (OSCLocalPortMode)EditorGUILayout.EnumPopup(_localHostModeContent, transmitter.LocalPortMode);
					if (transmitter.LocalPortMode == OSCLocalPortMode.FromRemotePort)
					{
						// LOCAL FROM REMOTE PORT
						using (new GUILayout.HorizontalScope())
						{
							EditorGUILayout.LabelField(_localPortContent, GUILayout.Width(EditorGUIUtility.labelWidth - 4));
							EditorGUILayout.SelectableLabel(transmitter.RemotePort.ToString(), GUILayout.Height(EditorGUIUtility.singleLineHeight));
						}
					}
					else if (transmitter.LocalPortMode == OSCLocalPortMode.FromReceiver)
					{
						GUI.color = Color.red;
						EditorGUILayout.HelpBox("Not available in InEditor mode.", MessageType.Warning);
						GUI.color = _defaultColor;
					}
					else if (transmitter.LocalPortMode == OSCLocalPortMode.Custom)
					{
						transmitter.LocalPort = EditorGUILayout.IntField(_localPortContent, transmitter.LocalPort);
					}
				}
			}
		}

		private void DrawControls(OSCEditorBase editorBase, out bool remove)
		{
			var connect = false;
			var reconnect = false;
			
			using (new GUILayout.VerticalScope(OSCEditorStyles.Box))
			{
				GUI.color = editorBase.IsStarted ? Color.green : Color.red;
				connect = GUILayout.Button(editorBase.IsStarted ? "Connected" : "Disconnected");
				
				using (new GUILayout.HorizontalScope(OSCEditorStyles.Box))
				{
					GUI.color = Color.yellow;
					EditorGUI.BeginDisabledGroup(!editorBase.IsStarted);
					reconnect = GUILayout.Button("Reconnect");
					EditorGUI.EndDisabledGroup();

					GUI.color = Color.red;
					remove = GUILayout.Button("Remove");
				}
			}

			// Actions
			if (connect)
			{
				if (editorBase.IsStarted) editorBase.Close();
				else editorBase.Connect();
			}
	
			if (reconnect)
			{
				if (editorBase.IsStarted) editorBase.Close();

				editorBase.Connect();
			}
		}

		#endregion
	}
}