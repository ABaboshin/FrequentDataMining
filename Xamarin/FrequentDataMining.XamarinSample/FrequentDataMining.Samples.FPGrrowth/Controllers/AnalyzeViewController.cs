// MIT License.
// (c) 2015, Andrey Baboshin

using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using SamplesCommon;
using UIKit;
using FrequentDataMining.Common;
using FrequentDataMining.FPGrowth;
using FrequentDataMining.AgrawalFaster;

namespace FrequentDataMining.XamarinSample
{
	public partial class AnalyzeViewController : UIViewController
	{
		public AnalyzeViewController (IntPtr handle) : base (handle)
		{
		}

		public IEnumerable<IEnumerable<BookAuthor>> Transactions {
			get;
			set;
		}

		public override void ViewDidLoad ()
		{
			TypeRegister.Register<BookAuthor>((a, b) => a.Name.CompareTo(b.Name));

			var itemsets = new List<Itemset<BookAuthor>>();
			var transactions = new SampleHelper().Transactions.ToList();

			var fpGrowth = new FPGrowth<BookAuthor>
			{
				MinSupport = (double) 1/9,
				SaveItemset = itemset => itemsets.Add(itemset),
				GetTransactions = () => transactions
			};

			fpGrowth.ProcessTransactions();

			var rules = new List<Rule<BookAuthor>>();

			var agrawal = new AgrawalFaster<BookAuthor>
			{
				MinLift = 0.01,
				MinConfidence = 0.01,
				TransactionsCount = transactions.Count(),
				GetItemsets = () => itemsets,
				SaveRule = rule => rules.Add(rule)
			};

			agrawal.Run();

			FrequentItemsTable.RegisterClassForCellReuse (typeof(FrequentItemTableViewCell), FrequentTableViewDelegate.CellIdentifier);
			FrequentItemsTable.Source = new FrequentTableViewDelegate{ 
				GetData = () => itemsets.OrderByDescending(i=>i.Support).ToList()
			};


			FrequentRulesTable.RegisterClassForCellReuse (typeof(FrequentRulesTableViewCell), FrequentRulesTableViewDelegate.CellIdentifier);
			FrequentRulesTable.Source = new FrequentRulesTableViewDelegate{ 
				GetData = () => rules.OrderByDescending(i=>i.Confidence).ToList()
			};
		}
	}
}
