/* Copyright (c) 2018 ExT (V.Sigalkin) */

using System;
using System.Collections.Generic;

namespace extEditorOSC
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
	public class OSCEditorBaseConfig
	{
		#region Public Vars

		public bool AutoConnect;

		#endregion
	}

	[Serializable]
	public class OSCEditorReceiverConfig : OSCEditorBaseConfig
	{
		#region Public Vars

		public int LocalPort;

		#endregion
	}

	[Serializable]
	public class OSCEditorTransmitterConfig : OSCEditorBaseConfig
	{
		#region Public Vars

		public string RemoteHost;

		public int RemotePort;

		public bool UseBundle;

		#endregion
	}

	[Serializable]
	public class OSCEditorComponentConfig
	{
		#region Public Vars

		public string Guid;

		public bool Active;

		public int Index = -1;

		#endregion
	}
}