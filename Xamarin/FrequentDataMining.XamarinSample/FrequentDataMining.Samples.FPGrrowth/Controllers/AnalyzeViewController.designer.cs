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
	[Register ("AnalyzeViewController")]
	partial class AnalyzeViewController
	{
		[Outlet]
		UIKit.UITableView FrequentItemsTable { get; set; }

		[Outlet]
		UIKit.UITableView FrequentRulesTable { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (FrequentItemsTable != null) {
				FrequentItemsTable.Dispose ();
				FrequentItemsTable = null;
			}

			if (FrequentRulesTable != null) {
				FrequentRulesTable.Dispose ();
				FrequentRulesTable = null;
			}
		}
	}
}
