// MIT License.
// (c) 2015, Andrey Baboshin

using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using UIKit;

namespace FrequentDataMining.XamarinSample
{
	public class TableViewDelegate : UITableViewSource
	{
		public static NSString CellIdentifier = new NSString("id");

		public Func<List<string>> GetData {
			get;
			set;
		}

		public override nint NumberOfSections (UITableView tableView)
		{
			return 1;
		}

		public override nint RowsInSection (UITableView tableview, nint section)
		{
			return GetData ().Count ();
		}

		public override bool CanEditRow (UITableView tableView, NSIndexPath indexPath)
		{
			return false;
		}

		public override bool CanMoveRow (UITableView tableView, NSIndexPath indexPath)
		{
			return false;
		}

		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
		}

		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			var currentCell = CellView.Create ();
			currentCell.Label.Text = GetData().Skip (indexPath.Row).First ();

			return currentCell;
		}
	}
}

