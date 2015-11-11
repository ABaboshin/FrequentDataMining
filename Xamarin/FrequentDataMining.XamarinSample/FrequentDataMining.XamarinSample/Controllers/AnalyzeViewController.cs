// MIT License.
// (c) 2015, Andrey Baboshin

using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using FrequentDataMining.AgrawalFaster;
using FrequentDataMining.FPGrowth;
using SamplesCommon;
using UIKit;

namespace FrequentDataMining.XamarinSample
{
	public partial class AnalyzeViewController : UIViewController
	{
		public AnalyzeViewController (IntPtr handle) : base (handle)
		{
		}

		public List<List<BookAuthor>> Transactions {
			get;
			set;
		}

		public override void ViewDidLoad ()
		{
            FrequentDataMining.Common.TypeRegister.Register<BookAuthor>((a, b) => a.Name.CompareTo(b.Name)); ;

            var fpGrowth = new FPGrowth<BookAuthor>();
			var result = fpGrowth.ProcessTransactions((double)1/9, Transactions);

			FrequentItemsTable.RegisterClassForCellReuse (typeof(FrequentItemTableViewCell), FrequentTableViewDelegate.CellIdentifier);
			FrequentItemsTable.Source = new FrequentTableViewDelegate{ 
				GetData = () => result.OrderByDescending(i=>i.Support).ToList()
			};

			var agrawal = new AgrawalFaster<BookAuthor>();
			var ar = agrawal.Run(0.01, 0.01, result, Transactions.Count);

			FrequentRulesTable.RegisterClassForCellReuse (typeof(FrequentRulesTableViewCell), FrequentRulesTableViewDelegate.CellIdentifier);
			FrequentRulesTable.Source = new FrequentRulesTableViewDelegate{ 
				GetData = () => ar.OrderByDescending(i=>i.Confidence).ToList()
			};
		}
	}
}
