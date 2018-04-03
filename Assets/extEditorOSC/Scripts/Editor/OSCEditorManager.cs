/* Copyright (c) 2018 ExT (V.Sigalkin) */

using UnityEngine;

using UnityEditor;

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

using extOSC;

namespace extEditorOSC
{
	[InitializeOnLoad]
	public static class OSCEditorManager
	{
		#region Public Vars

		public static OSCEditorReceiver[] Receivers
		{
			get { return _receivers.ToArray(); }
		}

		public static OSCEditorTransmitter[] Transmitters
		{
			get { return _transmitters.ToArray(); }
		}

		#endregion

		#region Private Methods

		private static List<OSCEditorReceiver> _receivers = new List<OSCEditorReceiver>();

		private static List<OSCEditorTransmitter> _transmitters = new List<OSCEditorTransmitter>();

		private static readonly string _configsPath = "./extOSC/editorconfigs.json";

		#endregion

		#region Public Methods

		static OSCEditorManager()
		{
			var directory = Path.GetDirectoryName(_configsPath);

			if (!Directory.Exists(directory))
				Directory.CreateDirectory(directory);

			if (!File.Exists(_configsPath))
			{
				var configs = new OSCEditorConfigs();
				
				var transmitterConfig =new OSCEditorTransmitterConfig();
				transmitterConfig.RemoteHost = "127.0.0.1";
				transmitterConfig.RemotePort = 7100;
				transmitterConfig.UseBundle = false;
				transmitterConfig.AutoConnect = true;

				var receiverConfig = new OSCEditorReceiverConfig();
				receiverConfig.LocalPort = 7100;
				receiverConfig.AutoConnect = true;

				configs.TransmitterConfigs.Add(transmitterConfig);
				configs.ReceiverConfigs.Add(receiverConfig);

				File.WriteAllText(_configsPath, JsonUtility.ToJson(configs));
			}

			LoadSettings();

			var componentsTypes = OSCEditorExtensions.GetTypes(typeof(OSCEditorReceiverComponent));
			foreach (var componentType in componentsTypes)
			{
				if (componentType.IsAbstract) continue;

				var component = (OSCEditorReceiverComponent)Activator.CreateInstance(componentType);
				//component.InitBinds(_receiver);
			}
		}

		public static void SaveSettigs()
		{
			var configs = new OSCEditorConfigs();

			foreach (var receiver in _receivers)
			{
				var receiverConfig = new OSCEditorReceiverConfig();
				receiverConfig.LocalPort = receiver.LocalPort;
				receiverConfig.AutoConnect = receiver.IsAvaible;

				configs.ReceiverConfigs.Add(receiverConfig);
			}

			foreach (var transmitter in _transmitters)
			{
				var transmitterConfig = new OSCEditorTransmitterConfig();
				transmitterConfig.RemoteHost = transmitter.RemoteHost;
				transmitterConfig.RemotePort = transmitter.RemotePort;
				transmitterConfig.UseBundle = transmitter.UseBundle;
				transmitterConfig.AutoConnect = transmitter.IsAvaible;

				configs.TransmitterConfigs.Add(transmitterConfig);
			}


			if (File.Exists(_configsPath))
				File.Delete(_configsPath);

			File.WriteAllText(_configsPath, JsonUtility.ToJson(configs, true));
		}

		public static void LoadSettings()
		{
			if (!File.Exists(_configsPath))
				return;

			var configs = JsonUtility.FromJson<OSCEditorConfigs>(File.ReadAllText(_configsPath));

			_receivers.ForEach(receiver => { receiver.Close(); receiver.Dispose(); });
			_receivers.Clear();

			_transmitters.ForEach(transmitter => { transmitter.Close();transmitter.Dispose();});
			_transmitters.Clear();

			foreach (var receiverConfig in configs.ReceiverConfigs)
			{
				var receiver = new OSCEditorReceiver();
				receiver.LocalPort = receiverConfig.LocalPort;

				if (receiverConfig.AutoConnect)
					receiver.Connect();

				_receivers.Add(receiver);
			}

			foreach (var transmitterConfig in configs.TransmitterConfigs)
			{
				var transmitter = new OSCEditorTransmitter();
				transmitter.RemoteHost = transmitterConfig.RemoteHost;
				transmitter.RemotePort = transmitterConfig.RemotePort;
				transmitter.UseBundle = transmitterConfig.UseBundle;

				if (transmitterConfig.AutoConnect)
					transmitter.Connect();

				_transmitters.Add(transmitter);
			}
		}

		#endregion
	}
}