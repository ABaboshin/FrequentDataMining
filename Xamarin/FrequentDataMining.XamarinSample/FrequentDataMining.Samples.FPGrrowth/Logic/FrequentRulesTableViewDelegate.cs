// MIT License.
// (c) 2015, Andrey Baboshin

using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using FrequentDataMining.AgrawalFaster;
using SamplesCommon;
using UIKit;

namespace FrequentDataMining.XamarinSample
{
	public class FrequentRulesTableViewDelegate: UITableViewSource
	{
		public static NSString CellIdentifier = new NSString("frid");

		public Func<List<Rule<BookAuthor>>> GetData {
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
			var currentCell = FrequentRulesTableViewCell.Create ();
			var data = GetData ().Skip (indexPath.Row).First ();
			currentCell.Support.Text = Math.Round(data.Confidence, 2).ToString ();
			currentCell.Lift.Text = Math.Round(data.Lift, 2).ToString ();
			currentCell.Items.Text = string.Join (", ", data.Combination.Select(i=>i.ToString())) + " => " + string.Join (", ", data.Remaining.Select(i=>i.ToString()));

			return currentCell;
		}
	}
}

