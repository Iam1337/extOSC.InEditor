/* Copyright (c) 2018 ExT (V.Sigalkin) */

using UnityEngine;

using UnityEditor;

using extOSC.Editor;
using extOSC.Editor.Windows;

using extEditorOSC.Panels;

namespace extEditorOSC.Windows
{
	public class OSCWindowEditor : OSCWindow<OSCWindowEditor, OSCPanelEditor>
	{
		#region Static Public Methods

		[MenuItem("Window/extEditorOSC/Editor Manager", false, 0)]
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