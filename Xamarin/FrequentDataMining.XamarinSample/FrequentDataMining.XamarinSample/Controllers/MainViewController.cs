// MIT License.
// (c) 2015, Andrey Baboshin

using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using SamplesCommon;
using UIKit;

namespace FrequentDataMining.XamarinSample
{
	public partial class MainViewController : UIViewController
	{
		public MainViewController (IntPtr handle) : base (handle)
		{
			tableViewDelegate = new TableViewDelegate{
				GetData = () => transactions.Select(t=>string.Join(", ", t.Select(i=>i.ToString()))).ToList()
			};
		}

		IEnumerable<IEnumerable<BookAuthor>> transactions = new SampleHelper().Transactions;

		TableViewDelegate tableViewDelegate;

		public override void ViewDidLoad ()
		{
			TableView.RegisterClassForCellReuse (typeof(CellView), TableViewDelegate.CellIdentifier);
			TableView.Source = tableViewDelegate;
		}

		public override void PrepareForSegue (UIStoryboardSegue segue, NSObject sender)
		{
			base.PrepareForSegue (segue, sender);

			var ctrl = (AnalyzeViewController)segue.DestinationViewController;
			if (ctrl != null) {
				ctrl.Transactions = transactions;
			}
		}
	}
}
