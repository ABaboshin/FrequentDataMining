// MIT License.
// (c) 2015, Andrey Baboshin

using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using FrequentDataMining.AgrawalFaster;
using FrequentDataMining.Common;
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

		public IEnumerable<IEnumerable<BookAuthor>> Transactions {
			get;
			set;
		}

		public override void ViewDidLoad ()
		{
			TypeRegister.Register<BookAuthor>((a, b) => a.Name.CompareTo(b.Name));

			var itemsetReaderWriter = new BookAuthorItemsetReaderWriter();
			var transactionsReader = new BookAuthorTransactionsReader();

			var fpGrowth = new FPGrowth<BookAuthor>
			{
				MinSupport = (double) 1/9,
				ItemsetWriter = itemsetReaderWriter,
				TransactionsReader = transactionsReader
			};

			fpGrowth.ProcessTransactions();

			var ruleWriter = new BookAuthorRuleWriter();

			var agrawal = new AgrawalFaster<BookAuthor>
			{
				MinLift = 0.01,
				MinConfidence = 0.01,
				TransactionsCount = transactionsReader.GetTransactions().Count(),
				ItemsetReader = itemsetReaderWriter,
				RuleWriter = ruleWriter
			};

			agrawal.Run();

			FrequentItemsTable.RegisterClassForCellReuse (typeof(FrequentItemTableViewCell), FrequentTableViewDelegate.CellIdentifier);
			FrequentItemsTable.Source = new FrequentTableViewDelegate{ 
				GetData = () => itemsetReaderWriter.Itemsets.OrderByDescending(i=>i.Support).ToList()
			};


			FrequentRulesTable.RegisterClassForCellReuse (typeof(FrequentRulesTableViewCell), FrequentRulesTableViewDelegate.CellIdentifier);
			FrequentRulesTable.Source = new FrequentRulesTableViewDelegate{ 
				GetData = () => ruleWriter.Rules.OrderByDescending(i=>i.Confidence).ToList()
			};
		}
	}
}
