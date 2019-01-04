/* Copyright (c) 2019 ExT (V.Sigalkin) */

#if EXTOSC

using UnityEngine;

using UnityEditor;

using extOSC.Editor;
using extOSC.Editor.Windows;

using extEditorOSC.Panels;

namespace extEditorOSC.Windows
{
	public class OSCWindowManager : OSCWindow<OSCWindowManager, OSCPanelManager>
	{
		#region Static Public Methods

		[MenuItem("Tools/extEditorOSC/Editor Manager", false, 0)]
		public static void ShowWindow()
		{
			Instance.titleContent = new GUIContent("OSC Editor Manager", OSCEditorTextures.IronWall);
			Instance.minSize = new Vector2(250, 200);
			Instance.Show();
		}

		#endregion

		#region Protected Methods

		protected override void SaveWindowSettings()
		{
			OSCEditorManager.SaveSettigs();
		}

		#endregion
	}
}

#endif