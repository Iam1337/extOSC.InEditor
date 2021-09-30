/* Copyright (c) 2021 dr. ext (Vladimir Sigalkin) */

using UnityEngine;
using UnityEditor;

using extOSC.InEditor;
using extOSC.Editor.Panels.InEditor;

namespace extOSC.Editor.Windows.InEditor
{
	public class OSCWindowManager : OSCWindow<OSCWindowManager, OSCPanelManager>
	{
		#region Static Public Methods

		[MenuItem("Tools/extOSC.InEditor/Editor Manager", false, 0)]
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
			OSCEditorManager.SaveSettings();
		}

		#endregion
	}
}