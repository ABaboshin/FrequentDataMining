// MIT License.
// (c) 2015, Andrey Baboshin

using System;
using Foundation;
using UIKit;

namespace FrequentDataMining.XamarinSample
{
	public partial class CellView : UITableViewCell
	{
		public static readonly UINib Nib = UINib.FromName ("CellView", NSBundle.MainBundle);
		public static readonly NSString Key = new NSString ("CellView");

		public CellView (IntPtr handle) : base (handle)
		{
			AwakeFromNib ();
		}

		public static CellView Create ()
		{
			return (CellView)Nib.Instantiate (null, null) [0];
		}
	}
}

