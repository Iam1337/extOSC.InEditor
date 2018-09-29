/* Copyright (c) 2018 ExT (V.Sigalkin) */

#if EXTOSC

using extOSC.Core;

namespace extEditorOSC.Components
{
	public abstract class OSCEditorTransmitterComponent : OSCEditorComponent
	{
		#region Public Vars

		public OSCEditorTransmitter Transmitter
		{
			get { return _transmitter; }
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

		public void Send(OSCPacket packet)
		{
			if (_transmitter != null)
				_transmitter.Send(packet);
		}

		#endregion
	}
}

#endif