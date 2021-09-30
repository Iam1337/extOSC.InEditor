/* Copyright (c) 2021 dr. ext (Vladimir Sigalkin) */

using UnityEngine;
using UnityEditor;

using extOSC.InEditor;
using extOSC.Editor.Panels.InEditor;

namespace extOSC.Editor.Windows.InEditor
{
	public class OSCWindowComponents : OSCWindow<OSCWindowComponents, OSCPanelComponents>
	{
		#region Static Public Methods

		[MenuItem("Tools/extOSC.InEditor/Editor Components", false, 1)]
		public static void ShowWindow()
		{
			Instance.titleContent = new GUIContent("OSC Editor Components", OSCEditorTextures.IronWall);
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