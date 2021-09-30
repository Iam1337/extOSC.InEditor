/* Copyright (c) 2021 dr. ext (Vladimir Sigalkin) */

using extOSC.Core;

namespace extOSC.InEditor
{
	public static class OSCEditorConsole
	{
		#region Public Methods

		public static void Received(OSCEditorReceiver receiver, IOSCPacket packet)
		{
			var message = new OSCConsolePacket();
			message.Info = $"<color=green>Editor Receiver</color>: {receiver.LocalPort}. From: {(packet.Ip != null ? packet.Ip.ToString() : "Debug")}";
			message.PacketType = OSCConsolePacketType.Received;
			message.Packet = packet;

			Log(message);
		}

		public static void Transmitted(OSCEditorTransmitter transmitter, IOSCPacket packet)
		{
			var message = new OSCConsolePacket();
			message.Info = $"<color=green>Editor Transmitter</color>: {transmitter.RemoteHost}:{transmitter.RemotePort}";
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