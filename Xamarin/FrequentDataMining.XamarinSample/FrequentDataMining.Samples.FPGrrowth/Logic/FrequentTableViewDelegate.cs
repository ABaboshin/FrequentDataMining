// MIT License.
// (c) 2015, Andrey Baboshin

using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using FrequentDataMining.Common;
using SamplesCommon;
using UIKit;

namespace FrequentDataMining.XamarinSample
{
	public class FrequentTableViewDelegate: UITableViewSource
	{
		public static NSString CellIdentifier = new NSString("fid");

		public Func<List<Itemset<BookAuthor>>> GetData {
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
			var currentCell = FrequentItemTableViewCell.Create ();
			var data = GetData ().Skip (indexPath.Row).First ();
			currentCell.Support.Text = data.Support.ToString ();
			currentCell.Item.Text = string.Join (", ", data.Value.Select(i=>i.ToString()));

			return currentCell;
		}
	}
}

