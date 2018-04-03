/* Copyright (c) 2018 ExT (V.Sigalkin) */

using System;

namespace extEditorOSC
{
	[AttributeUsage(AttributeTargets.Class)]
	public class OSCEditorReceiverAttribute : Attribute
	{
		#region Public Vars

		public int ReceiverId
		{
			get { return _receiverId; }
		}

		#endregion

		#region Private Vars

		private int _receiverId;

		#endregion

		#region Public Methods

		public OSCEditorReceiverAttribute(int receiverId)
		{
			_receiverId = receiverId;
		}

		#endregion
	}
}