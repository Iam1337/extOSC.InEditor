/* Copyright (c) 2018 ExT (V.Sigalkin) */

#if EXTOSC

using extOSC.Core;
using extOSC.Core.Console;

namespace extEditorOSC.Core
{
	public static class OSCEditorConsole
	{
		#region Public Methods

		public static void Received(OSCEditorReceiver receiver, OSCPacket packet)
		{
			var message = new OSCConsolePacket();
			message.Info = string.Format("<color=green>Editor Receiver</color>: {0}. From: {1}", receiver.LocalPort, packet.Ip != null ? packet.Ip.ToString() : "Debug");
			message.PacketType = OSCConsolePacketType.Received;
			message.Packet = packet;

			Log(message);
		}

		public static void Transmitted(OSCEditorTransmitter transmitter, OSCPacket packet)
		{
			var message = new OSCConsolePacket();
			message.Info = string.Format("<color=green>Editor Transmitter</color>: {0}:{1}", transmitter.RemoteHost, transmitter.RemotePort);
			message.PacketType = OSCConsolePacketType.Transmitted;
			message.Packet = packet;

			Log(message);
		}

		#endregion

		#region Private Methods

		private static void Log(OSCConsolePacket message)
		{
			// COPY PACKET
			var rawData = OSCConverter.Pack(message.Packet);
			message.Packet = OSCConverter.Unpack(rawData);

			OSCConsole.ConsoleBuffer.Add(message);
		}

		#endregion
	}
}

#endif