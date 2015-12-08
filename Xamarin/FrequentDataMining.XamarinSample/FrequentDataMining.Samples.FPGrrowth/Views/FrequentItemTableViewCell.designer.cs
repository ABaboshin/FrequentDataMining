// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace FrequentDataMining.XamarinSample
{
	[Register ("FrequentItemTableViewCell")]
	partial class FrequentItemTableViewCell
	{
		[Outlet]
		public UIKit.UILabel Item { get; set; }

		[Outlet]
		public UIKit.UILabel Support { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (Support != null) {
				Support.Dispose ();
				Support = null;
			}

			if (Item != null) {
				Item.Dispose ();
				Item = null;
			}
		}
	}
}
