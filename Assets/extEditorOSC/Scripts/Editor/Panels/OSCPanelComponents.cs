/* Copyright (c) 2018 ExT (V.Sigalkin) */

using UnityEngine;

using UnityEditor;

using System.Net;

using extOSC;
using extOSC.Editor.Panels;
using extOSC.Editor.Windows;

using extEditorOSC.Core;


namespace extEditorOSC.Panels
{
	public class OSCPanelComponents : OSCPanel
	{
		#region Public Methods

		public OSCPanelComponents(OSCWindow parentWindow, string panelId) : base(parentWindow, panelId)
		{
		}

		#endregion
	}
}