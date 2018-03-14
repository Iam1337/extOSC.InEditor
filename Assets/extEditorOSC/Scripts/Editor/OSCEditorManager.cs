/* Copyright (c) 2018 ExT (V.Sigalkin) */

using UnityEngine;

using UnityEditor;

using System;
using System.IO;
using System.Xml;

using extOSC;

namespace extEditorOSC
{
	[InitializeOnLoad]
	public static class OSCEditorManager
	{
		#region Public Vars

		public static OSCEditorReceiver Receiver
		{
			get
			{
				if (_receiver == null)
				{
					_receiver = new OSCEditorReceiver();
				}

				return _receiver;
			}
		}

		public static bool AutoConnectReceiver
		{
			get { return _autoConnectReceiver; }
			set { _autoConnectReceiver = value; }
		}

		public static OSCEditorTransmitter Transmitter
		{
			get
			{
				if (_transmitter == null)
				{
					_transmitter = new OSCEditorTransmitter();
				}

				return _transmitter;
			}
		}

		public static bool AutoConnectTransmitter
		{
			get { return _autoConnectTransmitter; }
			set { _autoConnectTransmitter = value; }
		}

		public static string SettingsFilePath
		{
			get
			{
				if (!Directory.Exists(Path.GetDirectoryName(_settingsFilePath)))
					Directory.CreateDirectory(Path.GetDirectoryName(_settingsFilePath));

				return _settingsFilePath;
			}
		}

		#endregion

		#region Private Methods

		private static readonly string _settingsFilePath = "./extOSC/editorsettings.xml";

		private static bool _autoConnectReceiver = true;

		private static bool _autoConnectTransmitter = true;

		private static OSCEditorReceiver _receiver;

		private static OSCEditorTransmitter _transmitter;

		#endregion

		#region Public Methods

		static OSCEditorManager()
		{
			if (_receiver == null)
			{
				_receiver = new OSCEditorReceiver();
			}

			if (_transmitter == null)
			{
				_transmitter = new OSCEditorTransmitter();
			}

			if (File.Exists(SettingsFilePath))
			{
				LoadSettings();
			}

			if (_autoConnectReceiver)
			{
				_receiver.Connect();
			}

			if (_autoConnectTransmitter)
			{
				_transmitter.Connect();
			}

			var componentsTypes = OSCEditorExtensions.GetTypes(typeof(OSCEditorReceiverComponent));
			foreach (var componentType in componentsTypes)
			{
				if (componentType.IsAbstract) continue;

				var component = (OSCEditorReceiverComponent)Activator.CreateInstance(componentType);
				component.InitBinds(_receiver);
			}
		}

		public static void SaveSettigs()
		{
			var document = new XmlDocument();
			var rootElement = (XmlElement)document.AppendChild(document.CreateElement("root"));

			var autoConnectReceiverAttribute = document.CreateAttribute("autoConnectReceiver");
			autoConnectReceiverAttribute.InnerText = _autoConnectReceiver ? "true" : "false";

			var autoConnectTransmitterAttribute = document.CreateAttribute("autoConnectTransmitter");
			autoConnectTransmitterAttribute.InnerText = _autoConnectReceiver ? "true" : "false";

			rootElement.Attributes.Append(autoConnectReceiverAttribute);
			rootElement.Attributes.Append(autoConnectTransmitterAttribute);

			var receiverElement = rootElement.AppendChild(document.CreateElement("receiver"));

			var localPortAttribute = document.CreateAttribute("localPort");
			localPortAttribute.InnerText = _receiver.LocalPort.ToString();

			receiverElement.Attributes.Append(localPortAttribute);

			var transmitterElement = rootElement.AppendChild(document.CreateElement("transmitter"));

			var remoteHostAttribute = document.CreateAttribute("remoteHost");
			remoteHostAttribute.InnerText = _transmitter.RemoteHost;

			var remotePortAttribute = document.CreateAttribute("remotePort");
			remotePortAttribute.InnerText = _transmitter.RemotePort.ToString();

			var useBundleAttribute = document.CreateAttribute("useBundle");
			useBundleAttribute.InnerText = _transmitter.UseBundle ? "true" : "false";

			transmitterElement.Attributes.Append(remoteHostAttribute);
			transmitterElement.Attributes.Append(remotePortAttribute);
			transmitterElement.Attributes.Append(useBundleAttribute);

			document.Save(SettingsFilePath);
		}

		public static void LoadSettings()
		{
			var document = new XmlDocument();
			try
			{
				document.Load(SettingsFilePath);

				var rootElement = document.FirstChild;

				_autoConnectReceiver = rootElement.Attributes["autoConnectReceiver"].InnerText == "true";
				_autoConnectTransmitter = rootElement.Attributes["autoConnectTransmitter"].InnerText == "true";

				var receiverElement = rootElement["receiver"];

				_receiver.LocalPort = int.Parse(receiverElement.Attributes["localPort"].InnerText);

				var transmitterElement = rootElement["transmitter"];

				_transmitter.RemoteHost = transmitterElement.Attributes["remoteHost"].InnerText;
				_transmitter.RemotePort = int.Parse(transmitterElement.Attributes["remotePort"].InnerText);
				_transmitter.UseBundle = transmitterElement.Attributes["useBundle"].InnerText == "true";
			}
			catch (Exception e)
			{
				Debug.LogFormat("[OSCEditorManager] Error: {0}", e);
			}
		}

		#endregion
	}
}