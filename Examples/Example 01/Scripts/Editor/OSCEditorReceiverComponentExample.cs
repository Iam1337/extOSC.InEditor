/* Copyright (c) 2021 dr. ext (Vladimir Sigalkin) */

using UnityEngine;

using System.Collections.Generic;

using extOSC.Core;

namespace extOSC.InEditor.Examples
{
	[OSCEditorComponent("Examples", "Example Receiver Component")]
	public class OSCEditorReceiverComponentExample : OSCEditorReceiverComponent
	{
		#region Protected Methods

		protected override void PopulateBinds(List<IOSCBind> binds)
		{
			binds.Add(new OSCBind("/editor/example", MessageReceive));
		}

		#endregion

		#region Private Methods

		private void MessageReceive(OSCMessage message)
		{
			Debug.LogFormat("Received message: {0}", message);
		}

		#endregion
	}
}