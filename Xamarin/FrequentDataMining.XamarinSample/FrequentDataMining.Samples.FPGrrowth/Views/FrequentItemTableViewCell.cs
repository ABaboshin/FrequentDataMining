// MIT License.
// (c) 2015, Andrey Baboshin

using System;
using Foundation;
using UIKit;

namespace FrequentDataMining.XamarinSample
{
	public partial class FrequentItemTableViewCell : UITableViewCell
	{
		public static readonly UINib Nib = UINib.FromName ("FrequentItemTableViewCell", NSBundle.MainBundle);
		public static readonly NSString Key = new NSString ("FrequentItemTableViewCell");

		public FrequentItemTableViewCell (IntPtr handle) : base (handle)
		{
		}

		public static FrequentItemTableViewCell Create ()
		{
			return (FrequentItemTableViewCell)Nib.Instantiate (null, null) [0];
		}
	}
}

