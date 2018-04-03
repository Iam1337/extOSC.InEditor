/* Copyright (c) 2018 ExT (V.Sigalkin) */

using System;
using System.Collections.Generic;

namespace extEditorOSC
{
	[Serializable]
	public class OSCEditorConfigs
	{
		#region Public Vars

		public List<OSCEditorReceiverConfig> ReceiverConfigs = new List<OSCEditorReceiverConfig>();

		public List<OSCEditorTransmitterConfig> TransmitterConfigs = new List<OSCEditorTransmitterConfig>();

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
}