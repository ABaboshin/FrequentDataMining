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
	[Register ("FrequentRulesTableViewCell")]
	partial class FrequentRulesTableViewCell
	{
		[Outlet]
		public UIKit.UILabel Items { get; set; }

		[Outlet]
		public UIKit.UILabel Lift { get; set; }

		[Outlet]
		public UIKit.UILabel Support { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (Support != null) {
				Support.Dispose ();
				Support = null;
			}

			if (Lift != null) {
				Lift.Dispose ();
				Lift = null;
			}

			if (Items != null) {
				Items.Dispose ();
				Items = null;
			}
		}
	}
}
