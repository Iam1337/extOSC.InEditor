/* Copyright (c) 2021 dr. ext (Vladimir Sigalkin) */

using extOSC.Core;

namespace extOSC.InEditor
{
	public abstract class OSCEditorTransmitterComponent : OSCEditorComponent
	{
		#region Public Vars

		public OSCEditorTransmitter Transmitter
		{
			get => _transmitter;
			set
			{
				if (_transmitter == value) return;

				_transmitter = value;
			}
		}

		#endregion

		#region Private Vars

		private OSCEditorTransmitter _transmitter;

		#endregion

		#region Protected Methods

		public void Send(IOSCPacket packet)
		{
			if (_transmitter != null)
				_transmitter.Send(packet);
		}

		#endregion
	}
}