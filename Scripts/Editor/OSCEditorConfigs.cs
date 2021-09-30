/* Copyright (c) 2021 dr. ext (Vladimir Sigalkin) */

using System;
using System.Collections.Generic;

namespace extOSC.InEditor
{
	[Serializable]
	public class OSCEditorConfigs
	{
		#region Public Vars

		public List<OSCEditorReceiverConfig> Receivers = new List<OSCEditorReceiverConfig>();

		public List<OSCEditorTransmitterConfig> Transmitters = new List<OSCEditorTransmitterConfig>();

		public List<OSCEditorComponentConfig> Components = new List<OSCEditorComponentConfig>();

		#endregion
	}
	
	[Serializable]
	public class OSCEditorReceiverConfig
	{
		#region Public Vars
		
		public bool AutoConnect;
		
		public OSCLocalHostMode LocalHostMode;

		public string LocalHost;

		public int LocalPort;
		

		#endregion
	}

	[Serializable]
	public class OSCEditorTransmitterConfig
	{
		#region Public Vars
		
		public bool AutoConnect;

		public string RemoteHost;

		public int RemotePort;

		public bool UseBundle;

		public OSCLocalHostMode LocalHostMode;

		public string LocalHost;
		
		public OSCLocalPortMode LocalPortMode;

		public int LocalPort;

		#endregion
	}

	[Serializable]
	public class OSCEditorComponentConfig
	{
		#region Public Vars

		public string Guid;

		public string Type;

		public int Index = -1;

		public bool Active;

		#endregion
	}
}