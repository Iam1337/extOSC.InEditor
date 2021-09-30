/* Copyright (c) 2021 dr. ext (Vladimir Sigalkin) */

using System;
using System.Collections.Generic;

using extOSC.Core;
using extOSC.Core.Network;

namespace extOSC.InEditor
{
	public class OSCEditorTransmitter : OSCEditorBase
	{
		#region Public Vars

		public override bool IsStarted => _transmitterBackend.IsAvailable;
		
		public OSCLocalHostMode LocalHostMode
		{
			get => _localHostMode;
			set
			{
				if (_localHostMode == value)
					return;

				_localHostMode = value;

				LocalRefresh();
			}
		}

		public OSCLocalPortMode LocalPortMode
		{
			get => _localPortMode;
			set
			{
				if (_localPortMode == value)
					return;

				_localPortMode = value;

				LocalRefresh();
			}
		}

		// public OSCEditorReceiver SourceReceiver
		// {
		// 	get => _localReceiver;
		// 	set
		// 	{
		// 		if (_localReceiver == value)
		// 			return;
		//
		// 		_localReceiver = value;
		//
		// 		LocalRefresh();
		// 	}
		// }

		public string LocalHost
		{
			get => GetLocalHost();
			set
			{
				if (_localHost == value)
					return;

				_localHost = value;

				LocalRefresh();
			}
		}

		public int LocalPort
		{
			get => GetLocalPort();
			set
			{
				if (_localPort == value)
					return;

				_localPort = value;

				LocalRefresh();
			}
		}

		public string RemoteHost
		{
			get => _remoteHost;
			set
			{
				if (_remoteHost == value)
					return;

				_remoteHost = value;

				RemoteRefresh();
			}
		}

		public int RemotePort
		{
			get => _remotePort;
			set
			{
				value = OSCUtilities.ClampPort(value);

				if (_remotePort == value)
					return;

				_remotePort = value;

				RemoteRefresh();
			}
		}

		public bool UseBundle
		{
			get => _useBundle;
			set => _useBundle = value;
		}

		#endregion

		#region Private Vars
		
		private OSCLocalHostMode _localHostMode = OSCLocalHostMode.Any;
		
		private OSCLocalPortMode _localPortMode = OSCLocalPortMode.Random;
		
		// private OSCEditorReceiver _localReceiver;
		
		private string _localHost;
		
		private int _localPort = 7000;
		
		private string _remoteHost = "127.0.0.1";
		
		private int _remotePort = 7000;
		
		private bool _useBundle;

		private OSCTransmitterBackend _transmitterBackend => __transmitterBackend ?? (__transmitterBackend = OSCTransmitterBackend.Create());

		private OSCTransmitterBackend __transmitterBackend;

		private readonly List<IOSCPacket> _bundleBuffer = new List<IOSCPacket>();
		
		private readonly List<IOSCPacket> _packetPool = new List<IOSCPacket>();

		#endregion

		#region Public Methods

		public override void Connect()
		{
			_transmitterBackend.Connect(GetLocalHost(), GetLocalPort());
			_transmitterBackend.RefreshRemote(_remoteHost, _remotePort);
		}

		public override void Close()
		{
			if (_transmitterBackend.IsAvailable)
				_transmitterBackend.Close();
		}

		public override string ToString()
		{
			return $"<{nameof(OSCTransmitter)} (LocalHost: {_localHost} LocalPort: {_localPort} | RemoteHost: {_remoteHost}, RemotePort: {_remotePort})>";
		}
		
		public void Send(IOSCPacket packet, OSCSendOptions options = OSCSendOptions.None)
		{
			if ((options & OSCSendOptions.IgnoreBundle) == 0)
			{
				if (_useBundle && packet is OSCMessage)
				{
					_bundleBuffer.Add(packet);

					return;
				}
			}

			if (!_transmitterBackend.IsAvailable)
				return;

			if ((options & OSCSendOptions.IgnoreMap) == 0)
			{
				// TODO: MapBundle?
				//if (MapBundle != null)
				//	MapBundle.Map(packet);
			}

			var length = OSCConverter.Pack(packet, out var buffer);
			
			_transmitterBackend.Send(buffer, length);

			OSCEditorConsole.Transmitted(this, packet);
		}

		#endregion

		#region Protected Methods

		protected override void Update()
		{
			if (_packetPool.Count > 0)
			{
				var bundle = new OSCBundle();

				foreach (var packet in _packetPool)
				{
					bundle.AddPacket(packet);
				}

				Send(bundle);

				_packetPool.Clear();
			}
		}

		#endregion

		#region Private Methods

		private void LocalRefresh()
		{
			if (IsStarted)
			{
				Close();
				Connect();
			}
		}
		
		private void RemoteRefresh()
		{
			_transmitterBackend.RefreshRemote(_remoteHost, _remotePort);
		}
		
		private string GetLocalHost()
		{
			//if (_localReceiver != null)
			//	return _localReceiver.LocalHost;
			
			if (_localHostMode == OSCLocalHostMode.Any)
				return "0.0.0.0";

			return _localHost;
		}

		private int GetLocalPort()
		{
			//if (_localReceiver != null)
			//	return _localReceiver.LocalPort;

			if (_localPortMode == OSCLocalPortMode.Random)
				return 0;

			if (_localPortMode == OSCLocalPortMode.FromReceiver)
				throw new Exception("[OSCEditorTransmitter] Local Port Mode does not support \"FromReceiver\" option.");

			if (_localPortMode == OSCLocalPortMode.Custom)
				return _localPort;

			return _remotePort;
		}

		#endregion
	}
}