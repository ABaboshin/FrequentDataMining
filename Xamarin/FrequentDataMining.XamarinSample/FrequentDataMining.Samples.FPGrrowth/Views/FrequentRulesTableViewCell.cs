// MIT License.
// (c) 2015, Andrey Baboshin

using System;
using Foundation;
using UIKit;

namespace FrequentDataMining.XamarinSample
{
	public partial class FrequentRulesTableViewCell : UITableViewCell
	{
		public static readonly UINib Nib = UINib.FromName ("FrequentRulesTableViewCell", NSBundle.MainBundle);
		public static readonly NSString Key = new NSString ("FrequentRulesTableViewCell");

		public FrequentRulesTableViewCell (IntPtr handle) : base (handle)
		{
		}

		public static FrequentRulesTableViewCell Create ()
		{
			return (FrequentRulesTableViewCell)Nib.Instantiate (null, null) [0];
		}
	}
}

