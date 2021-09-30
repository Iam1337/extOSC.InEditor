/* Copyright (c) 2021 dr. ext (Vladimir Sigalkin) */

using UnityEngine;

using UnityEditor;

using System;
using System.IO;
using System.Collections.Generic;

namespace extOSC.InEditor
{
	[InitializeOnLoad]
	public static class OSCEditorManager
	{
		#region Public Vars

		public static OSCEditorReceiver[] Receivers => _receivers.ToArray();

		public static OSCEditorTransmitter[] Transmitters => _transmitters.ToArray();

		public static OSCEditorComponent[] Components => _components.ToArray();

		#endregion

		#region Private Methods

		private static readonly List<OSCEditorReceiver> _receivers = new List<OSCEditorReceiver>();

		private static readonly List<OSCEditorTransmitter> _transmitters = new List<OSCEditorTransmitter>();

		private static readonly List<OSCEditorComponent> _components = new List<OSCEditorComponent>();

		private static readonly string _configsPath = "./extOSC/ineditor.json";

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
				
				configs.Transmitters.Add(new OSCEditorTransmitterConfig
				{
					RemoteHost = "127.0.0.1",
					RemotePort = 7100,
					UseBundle = false,
					AutoConnect = true,
					LocalHostMode = OSCLocalHostMode.Any,
					LocalHost = "0.0.0.0",
					LocalPortMode = OSCLocalPortMode.FromRemotePort,
					LocalPort = 7100
				});
				
				configs.Receivers.Add(new OSCEditorReceiverConfig
				{
					LocalHostMode = OSCLocalHostMode.Any,
					LocalHost = "0.0.0.0",
					LocalPort = 7100,
					AutoConnect = true
				});

				File.WriteAllText(_configsPath, JsonUtility.ToJson(configs));
			}

			var componentsTypes = OSCEditorUtils.GetTypes(typeof(OSCEditorComponent));
			foreach (var componentType in componentsTypes)
			{
				if (componentType.IsAbstract) continue;

				_components.Add((OSCEditorComponent) Activator.CreateInstance(componentType));
			}

			LoadSettings();
		}

		public static void SaveSettings()
		{
			var configs = new OSCEditorConfigs();

			foreach (var receiver in _receivers)
			{
				var receiverConfig = new OSCEditorReceiverConfig();
				receiverConfig.LocalPort = receiver.LocalPort;
				receiverConfig.AutoConnect = receiver.IsStarted;

				configs.Receivers.Add(receiverConfig);
			}

			foreach (var transmitter in _transmitters)
			{
				var transmitterConfig = new OSCEditorTransmitterConfig();
				transmitterConfig.RemoteHost = transmitter.RemoteHost;
				transmitterConfig.RemotePort = transmitter.RemotePort;
				transmitterConfig.UseBundle = transmitter.UseBundle;
				transmitterConfig.AutoConnect = transmitter.IsStarted;
				transmitterConfig.LocalPortMode = transmitter.LocalPortMode;
				transmitterConfig.LocalPort = transmitter.LocalPort;

				configs.Transmitters.Add(transmitterConfig);
			}

			foreach (var component in _components)
			{
				var componentType = component.GetType();

				var componentConfig = new OSCEditorComponentConfig();
				componentConfig.Type = componentType.AssemblyQualifiedName;
				componentConfig.Active = component.Active;
				componentConfig.Guid = OSCEditorUtils.GetTypeGUID(componentType);

				var receiverComponent = component as OSCEditorReceiverComponent;
				if (receiverComponent != null && receiverComponent.Receiver != null)
				{
					componentConfig.Index = _receivers.IndexOf(receiverComponent.Receiver);
				}
				else
				{
					var transmitterComponent = component as OSCEditorTransmitterComponent;
					if (transmitterComponent != null && transmitterComponent.Transmitter != null)
					{
						componentConfig.Index = _transmitters.IndexOf(transmitterComponent.Transmitter);
					}
				}

				configs.Components.Add(componentConfig);
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

			_receivers.ForEach(receiver =>
			{
				receiver.Close();
				receiver.Dispose();
			});
			_receivers.Clear();

			_transmitters.ForEach(transmitter =>
			{
				transmitter.Close();
				transmitter.Dispose();
			});
			_transmitters.Clear();

			foreach (var receiverConfig in configs.Receivers)
			{
				var receiver = new OSCEditorReceiver();
				receiver.LocalHostMode = receiverConfig.LocalHostMode;
				receiver.LocalHost = receiverConfig.LocalHost;
				receiver.LocalPort = receiverConfig.LocalPort;

				if (receiverConfig.AutoConnect)
					receiver.Connect();

				_receivers.Add(receiver);
			}

			foreach (var transmitterConfig in configs.Transmitters)
			{
				var transmitter = new OSCEditorTransmitter();
				transmitter.RemoteHost = transmitterConfig.RemoteHost;
				transmitter.RemotePort = transmitterConfig.RemotePort;
				transmitter.UseBundle = transmitterConfig.UseBundle;
				transmitter.LocalHostMode = transmitterConfig.LocalHostMode;
				transmitter.LocalHost = transmitterConfig.LocalHost;
				transmitter.LocalPortMode = transmitterConfig.LocalPortMode;
				transmitter.LocalPort = transmitterConfig.LocalPort;

				if (transmitterConfig.AutoConnect)
					transmitter.Connect();

				_transmitters.Add(transmitter);
			}

			foreach (var componentConfig in configs.Components)
			{
				var componentType = OSCEditorUtils.GetTypeByGUID(componentConfig.Guid);
				if (componentType == null || !componentType.IsSubclassOf(typeof(OSCEditorComponent)))
				{
					componentType = typeof(OSCEditorManager).Assembly.GetType(componentConfig.Type);
					if (componentType == null) continue;
				}

				var component = GetComponent(componentType);
				if (component == null) continue;

				if (component is OSCEditorReceiverComponent receiverComponent)
				{
					receiverComponent.Receiver = GetEditorReceiver(componentConfig.Index);
				}
				else
				{
					if (component is OSCEditorTransmitterComponent transmitterComponent)
					{
						transmitterComponent.Transmitter = GetEditorTransmitter(componentConfig.Index);
					}
				}

				component.Active = componentConfig.Active;
			}
		}

		public static T GetComponent<T>() where T : OSCEditorComponent
		{
			return (T) GetComponent(typeof(T));
		}

		public static OSCEditorComponent GetComponent(Type type)
		{
			if (_components == null || _components.Count == 0)
				return null;

			foreach (var component in _components)
			{
				if (component.GetType() == type)
					return component;
			}

			return null;
		}

		public static OSCEditorTransmitter CreateEditorTransmitter()
		{
			var transmitter = new OSCEditorTransmitter();
			transmitter.RemotePort = 7100 + _transmitters.Count;
			transmitter.LocalHostMode = OSCLocalHostMode.Any;
			transmitter.LocalHost =  OSCUtilities.GetLocalHost();
			transmitter.LocalPortMode = OSCLocalPortMode.FromRemotePort;
			transmitter.LocalPort = transmitter.RemotePort;
			
			_transmitters.Add(transmitter);

			return transmitter;
		}

		public static void RemoveEditorTransmitter(OSCEditorTransmitter transmitter)
		{
			if (!_transmitters.Contains(transmitter))
				return;

			transmitter.Close();
			transmitter.Dispose();

			_transmitters.Remove(transmitter);
		}

		public static OSCEditorTransmitter GetEditorTransmitter(int index)
		{
			if (index >= _transmitters.Count ||
			    index < 0) return null;

			return _transmitters[index];
		}

		public static OSCEditorReceiver CreateEditorReceiver()
		{
			var receiver = new OSCEditorReceiver();
			receiver.LocalHostMode = OSCLocalHostMode.Any;
			receiver.LocalHost = OSCUtilities.GetLocalHost();
			receiver.LocalPort = 7100 + _receivers.Count;

			_receivers.Add(receiver);

			return receiver;
		}

		public static void RemoveEditorReceiver(OSCEditorReceiver receiver)
		{
			if (!_receivers.Contains(receiver))
				return;

			receiver.Close();
			receiver.Dispose();

			_receivers.Remove(receiver);
		}

		public static OSCEditorReceiver GetEditorReceiver(int index)
		{
			if (index >= _receivers.Count ||
			    index < 0) return null;

			return _receivers[index];
		}

		#endregion
	}
}