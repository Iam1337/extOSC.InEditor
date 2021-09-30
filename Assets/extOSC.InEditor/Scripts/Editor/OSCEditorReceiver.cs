﻿/* Copyright (c) 2021 dr. ext (Vladimir Sigalkin) */

using System;
using UnityEngine;
using UnityEngine.Events;

using System.Collections.Generic;

using extOSC.Core;
using extOSC.Core.Network;

namespace extOSC.InEditor
{
	public class OSCEditorReceiver : OSCEditorBase
	{
		#region Public Vars
		
		public override bool IsStarted => _receiverBackend.IsAvailable;

		public OSCLocalHostMode LocalHostMode
		{
			get => _localHostMode;
			set
			{
				if (_localHostMode == value)
					return;

				_localHostMode = value;

				if (_receiverBackend.IsRunning && IsStarted)
				{
					Close();
					Connect();
				}
			}
		}

		public string LocalHost
		{
			get => GetLocalHost();
			set
			{
				if (_localHost == value)
					return;

				_localHost = value;

				if (_receiverBackend.IsRunning && IsStarted)
				{
					Close();
					Connect();
				}
			}
		}

		public int LocalPort
		{
			get => _localPort;
			set
			{
				value = OSCUtilities.ClampPort(value);

				if (_localPort == value)
					return;

				_localPort = value;

				if (_receiverBackend.IsRunning && IsStarted)
				{
					Close();
					Connect();
				}
			}
		}

		#endregion

		#region Private Vars

		private OSCReceiverBackend _receiverBackend
		{
			get
			{
				if (__receiverBackend == null)
				{
					__receiverBackend = OSCReceiverBackend.Create();
					__receiverBackend.ReceivedCallback = PacketReceived;
				}

				return __receiverBackend;
			}
		}
		
		private OSCLocalHostMode _localHostMode = OSCLocalHostMode.Any;
		
		private string _localHost;
		
		private int _localPort = 7001;

		private readonly Queue<IOSCPacket> _packets = new Queue<IOSCPacket>();

		private readonly List<IOSCBind> _messageBindings = new List<IOSCBind>();

		private readonly Stack<IOSCBind> _messageBindStack = new Stack<IOSCBind>();

		private readonly Stack<IOSCBind> _messageUnbindStack = new Stack<IOSCBind>();

		private readonly List<IOSCBindBundle> _bundleBindings = new List<IOSCBindBundle>();

		private readonly Stack<IOSCBindBundle> _bundleBindStack = new Stack<IOSCBindBundle>();

		private readonly Stack<IOSCBindBundle> _bundleUnbindStack = new Stack<IOSCBindBundle>();

		private readonly object _lock = new object();

		private OSCReceiverBackend __receiverBackend;

		private bool _processMessage;
		
		#endregion

		#region Public Methods

		public override string ToString()
		{
			return $"<{nameof(OSCReceiver)} (LocalHost: {_localHost} LocalPort: {_localPort})>";
		}
		
		public override void Connect()
		{
			_receiverBackend.Connect(GetLocalHost(), _localPort);
		}

		public override void Close()
		{
			if (_receiverBackend.IsAvailable)
				_receiverBackend.Close();
		}

		// IOSCBind
		public void Bind(IOSCBind bind)
		{
			if (bind == null)
				throw new NullReferenceException(nameof(bind));

			if (string.IsNullOrEmpty(bind.ReceiverAddress))
				throw new Exception("[OSCReceiver] Address can not be empty!");
			
			if (_processMessage)
			{
				_messageBindStack.Push(bind);
				return;
			}

			if (!_messageBindings.Contains(bind))
				_messageBindings.Add(bind);
		}

		public OSCBind Bind(string address, UnityAction<OSCMessage> callback)
		{
			var bind = new OSCBind(address, callback);

			Bind(bind);

			return bind;
		}

		public void Unbind(IOSCBind bind)
		{
			if (bind == null) return;

			if (_processMessage)
			{
				_messageUnbindStack.Push(bind);

				return;
			}

			_messageBindings.Remove(bind);
		}
		
		// IOSCBindBundle
		public void Bind(IOSCBindBundle bind)
		{
			if (bind == null)
				throw new ArgumentNullException(nameof(bind));

			if (_processMessage)
			{
				_bundleBindStack.Push(bind);

				return;
			}

			if (_bundleBindings.Contains(bind))
				throw new Exception("[OSCEditorReceiver] Bind already binded.");

			_bundleBindings.Add(bind);
		}

		public OSCBindBundle Bind(UnityAction<OSCBundle> callback)
		{
			var bind = new OSCBindBundle(callback);

			Bind(bind);

			return bind;
		}

		public void Unbind(IOSCBindBundle bind)
		{
			if (bind == null)
				throw new ArgumentNullException(nameof(bind));

			if (_processMessage)
			{
				_bundleUnbindStack.Push(bind);

				return;
			}

			if (!_bundleBindings.Contains(bind))
				throw new Exception("[OSCEditorReceiver] Bind already unbinded.");

			_bundleBindings.Remove(bind);
		}
		
		public void ClearBinds()
		{
			_messageBindings.Clear();
			_bundleBindings.Clear();
		}

		#endregion

		#region Protected Methods

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);

			ClearBinds();
		}

		protected override void Update()
		{
			if (!IsStarted || !_receiverBackend.IsRunning) return;

			lock (_lock)
			{
				while (_packets.Count > 0)
				{
					var packet = _packets.Dequeue();

					// TODO: MapBundle?
					// if (MapBundle != null)
					// 	MapBundle.Map(packet);

					OSCEditorConsole.Received(this, packet);

					InvokePacket(packet);
				}
			}
		}

		#endregion

		#region Private Methods

		private void InvokePacket(IOSCPacket packet)
		{
			if (packet.IsBundle())
			{
				InvokeBundle(packet as OSCBundle);
			}
			else
			{
				InvokeMessage(packet as OSCMessage);
			}
		}

		private void InvokeBundle(OSCBundle bundle)
		{
			if (bundle == null) return;

			foreach (var bind in _bundleBindings)
			{
				if (bind != null && bind.Callback != null)
					bind.Callback.Invoke(bundle);
			}

			foreach (var packet in bundle.Packets)
			{
				InvokePacket(packet);
			}
		}

		private void InvokeMessage(OSCMessage message)
		{
			if (message == null) return;

			_messageBindStack.Clear();
			_messageBindStack.Clear();

			_processMessage = true;

			foreach (var bind in _messageBindings)
			{
				if (bind == null) continue;

				if (OSCUtilities.CompareAddresses(bind.ReceiverAddress, message.Address))
				{
					if (bind.Callback != null)
						bind.Callback.Invoke(message);
				}
			}

			_processMessage = false;

			while (_messageBindStack.Count > 0)
			{
				Bind(_messageBindStack.Pop());
			}

			while (_messageUnbindStack.Count > 0)
			{
				Unbind(_messageUnbindStack.Pop());
			}

			while (_bundleBindStack.Count > 0)
			{
				Bind(_bundleBindStack.Pop());
			}

			while (_bundleUnbindStack.Count > 0)
			{
				Unbind(_bundleUnbindStack.Pop());
			}
		}

		private void PacketReceived(IOSCPacket packet)
		{
			lock (_lock)
			{
				_packets.Enqueue(packet);
			}
		}
		
		private string GetLocalHost()
		{
			return _localHostMode == OSCLocalHostMode.Any ? "0.0.0.0" : _localHost;
		}

		#endregion
	}
}