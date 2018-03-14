/* Copyright (c) 2018 ExT (V.Sigalkin) */

using UnityEngine;

using System.Collections.Generic;

using extOSC;
using extOSC.Core;
using extOSC.Core.Network;

using extEditorOSC.Core;

namespace extEditorOSC
{
	public class OSCEditorTransmitter : OSCEditorBase
	{
		#region Public Vars

		public override bool IsAvaible
		{
			get
			{
				if (transmitterBackend != null)
					return transmitterBackend.IsAvaible;

				return false;
			}
		}

		public string RemoteHost
		{
			get { return remoteHost; }
			set
			{
				if (remoteHost == value)
					return;

				remoteHost = value;

				transmitterBackend.RefreshConnection(remoteHost, remotePort);
			}
		}

		public int RemotePort
		{
			get { return remotePort; }
			set
			{
				value = OSCUtilities.ClampPort(value);

				if (remotePort == value)
					return;

				remotePort = value;

				transmitterBackend.RefreshConnection(remoteHost, remotePort);
			}
		}

		public bool UseBundle
		{
			get { return useBundle; }
			set { useBundle = value; }
		}

		#endregion

		#region Protected Vars

		[SerializeField]
		protected string remoteHost = "127.0.0.1";

		[SerializeField]
		protected int remotePort = 7100;

		[SerializeField]
		protected bool useBundle;

		protected OSCTransmitterBackend transmitterBackend
		{
			get
			{
				if (_transmitterBackend == null)
					_transmitterBackend = OSCTransmitterBackend.Create();

				return _transmitterBackend;
			}
		}

		#endregion

		#region Private Vars

		private readonly List<OSCPacket> _packetPool = new List<OSCPacket>();

		private OSCTransmitterBackend _transmitterBackend;

		#endregion

		#region Public Methods

		public override void Connect()
		{
			transmitterBackend.Connect(remoteHost, remotePort);
		}

		public override void Close()
		{
			transmitterBackend.Close();
		}

		public override string ToString()
		{
			return string.Format("<{0} (Host: {1}, Port: {2})>", GetType(), remoteHost, remotePort);
		}

		public void Send(OSCPacket packet)
		{
			if (!transmitterBackend.IsAvaible) return;

			if (useBundle && packet != null && (packet is OSCMessage))
			{
				_packetPool.Add(packet);

				return;
			}

			var data = OSCConverter.Pack(packet);

			transmitterBackend.Send(data);

			OSCEditorConsole.Transmitted(this, packet);
		}

		public virtual void Send(string address, OSCValue value)
		{
			var message = new OSCMessage(address);
			message.AddValue(value);

			Send(message);
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
	}
}