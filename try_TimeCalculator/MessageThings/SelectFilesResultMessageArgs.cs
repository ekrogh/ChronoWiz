using System;
using System.Collections.Generic;

namespace TimeDateCalculator.MessageThings
{
	public struct SelectedFileInfo
	{
		public System.IO.Stream TheStream { get; set; }
	}
	public class SelectFileResultMessageArgs : EventArgs
	{
		public bool DidPick { get; set; }
		public SelectedFileInfo TheSelectedFileInfo { get; set; }
	}

}
