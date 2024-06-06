using System;
using System.IO;
using Nevron.Nov.Dom;
using Nevron.Nov.Editors;
using Nevron.Nov.Globalization;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Framework
{
	public class NLocalizationExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NLocalizationExample()
		{
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NLocalizationExample()
		{
			NLocalizationExampleSchema = NSchema.Create(typeof(NLocalizationExample), NExampleBase.NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			CreateDictionaries();

			m_CalculatorHost = new NContentHolder();
			m_CalculatorHost.Content = CreateLoanCalculator();
			return m_CalculatorHost;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			
			stack.Add(new NLabel("Language:"));

			NListBox listBox = new NListBox();
			listBox.Items.Add(CreateListBoxItem(Presentation.NResources.Image_CountryFlags_us_png, EnglishLanguageName));
			listBox.Items.Add(CreateListBoxItem(Presentation.NResources.Image_CountryFlags_bg_png, BulgarianLanguageName));
			listBox.Items.Add(CreateListBoxItem(Presentation.NResources.Image_CountryFlags_de_png, GermanLanguageName));
			listBox.Selection.SingleSelect(listBox.Items[0]);
			listBox.Selection.Selected += OnListBoxItemSelected;
			stack.Add(listBox);

			return stack;
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>
	This example demonstrates how to take advantage of the NOV localization. Localization is the process
	of translating string literals used inside an application to different language. In your application's source
	code you should write localizable strings as NLoc.Get(""My string"") instead of directly as ""My string"".
	Thus on run time the string translation will be obtained from the localization dictionary instance.
</p>
<p>
	To see the localization in action, simply select a language from the list box on the right.
</p>
";
		}

		#endregion

		#region Implementation

		private void CreateDictionaries()
		{
			NLocalizationDictionary dictionary = NLocalizationDictionary.Instance;

			// Create the stream for the Bulgarian dictionary
			dictionary.SetTranslation("Loan Calculator", "Кредитен калкулатор");
			dictionary.SetTranslation("Amount:", "Количество:");
			dictionary.SetTranslation("Term in years:", "Срок в години:");
			dictionary.SetTranslation("Interest rate per year (%):", "Годишна лихва (%):");
			dictionary.SetTranslation("Repayment Summary", "Информация за погасяване");
			dictionary.SetTranslation("Monthly Payment:", "Месечна вноска:");
			dictionary.SetTranslation("Total Payments:", "Сума за връщане:");
			dictionary.SetTranslation("Total Interest:", "Общо лихви:");

			m_BulgarianStream = new MemoryStream();
			dictionary.SaveToStream(m_BulgarianStream);

			// Create the stream for the German Dictionary
			dictionary.SetTranslation("Loan Calculator", "Kreditrechner");
			dictionary.SetTranslation("Amount:", "Kreditbetrag:");
			dictionary.SetTranslation("Term in years:", "Laufzeit in Jahren:");
			dictionary.SetTranslation("Interest rate per year (%):", "Zinssatz pro Jahr (%):");
			dictionary.SetTranslation("Repayment Summary", "Rückzahlung Zusammenfassung");
			dictionary.SetTranslation("Monthly Payment:", "Monatliche Bezahlung:");
			dictionary.SetTranslation("Total Payments:", "Gesamtbetrag:");
			dictionary.SetTranslation("Total Interest:", "Gesamtzins:");

			m_GermanStream = new MemoryStream();
			dictionary.SaveToStream(m_GermanStream);

			// Create the stream for the English dictionary
			dictionary.SetTranslation("Loan Calculator", "Loan Calculator");
			dictionary.SetTranslation("Amount:", "Amount:");
			dictionary.SetTranslation("Term in years:", "Term in years:");
			dictionary.SetTranslation("Interest rate per year (%):", "Interest rate per year (%):");
			dictionary.SetTranslation("Repayment Summary", "Repayment Summary");
			dictionary.SetTranslation("Monthly Payment:", "Monthly Payment:");
			dictionary.SetTranslation("Total Payments:", "Total Payments:");
			dictionary.SetTranslation("Total Interest:", "Total Interest:");

			m_EnglishStream = new MemoryStream();
			dictionary.SaveToStream(m_EnglishStream);
		}
		private NWidget CreateLoanCalculator()
		{
			NStackPanel stack = new NStackPanel();
			stack.Font = new NFont(NFontDescriptor.DefaultSansFamilyName, 10);
			stack.HorizontalPlacement = ENHorizontalPlacement.Left;
			stack.VerticalSpacing = NDesign.VerticalSpacing * 2;

			NLabel titleLabel = new NLabel(NLoc.Get("Loan Calculator"));
			titleLabel.Font = new NFont(NFontDescriptor.DefaultSansFamilyName, 16);
			titleLabel.Margins = new NMargins(0, 0, 0, NDesign.VerticalSpacing);
			titleLabel.TextAlignment = ENContentAlignment.MiddleCenter;
			stack.Add(titleLabel);

			m_AmountUpDown = new NNumericUpDown();
			m_AmountUpDown.Value = 10000;
			m_AmountUpDown.Step = 500;
			m_AmountUpDown.ValueChanged += OnUpDownValueChanged;
			stack.Add(NPairBox.Create(NLoc.Get("Amount:"), m_AmountUpDown));

			m_TermUpDown = new NNumericUpDown();
			m_TermUpDown.Value = 8;
			m_TermUpDown.ValueChanged += OnUpDownValueChanged;
			stack.Add(NPairBox.Create(NLoc.Get("Term in years:"), m_TermUpDown));

			m_RateUpDown = new NNumericUpDown();
			m_RateUpDown.Value = 5;
			m_RateUpDown.Step = 0.1;
			m_RateUpDown.DecimalPlaces = 2;
			m_RateUpDown.ValueChanged += OnUpDownValueChanged;
			stack.Add(NPairBox.Create(NLoc.Get("Interest rate per year (%):"), m_RateUpDown));

			// Create the results labels
			NLabel repaymentLabel = new NLabel(NLoc.Get("Repayment Summary"));
			repaymentLabel.Margins = new NMargins(0, NDesign.VerticalSpacing * 5, 0, 0);
			repaymentLabel.Font = new NFont(NFontDescriptor.DefaultSansFamilyName, 12, ENFontStyle.Underline);
			repaymentLabel.TextAlignment = ENContentAlignment.MiddleCenter;
			stack.Add(repaymentLabel);

			m_MonthlyPaymentLabel = new NLabel();
			m_MonthlyPaymentLabel.TextAlignment = ENContentAlignment.MiddleRight;
			stack.Add(NPairBox.Create(NLoc.Get("Monthly Payment:"), m_MonthlyPaymentLabel));

			m_TotalPaymentsLabel = new NLabel();
			m_TotalPaymentsLabel.TextAlignment = ENContentAlignment.MiddleRight;
			stack.Add(NPairBox.Create(NLoc.Get("Total Payments:"), m_TotalPaymentsLabel));

			m_TotalInterestLabel = new NLabel();
			m_TotalInterestLabel.TextAlignment = ENContentAlignment.MiddleRight;
			stack.Add(NPairBox.Create(NLoc.Get("Total Interest:"), m_TotalInterestLabel));

			CalculateResult();

			return new NUniSizeBoxGroup(stack);
		}
		private void CalculateResult()
		{
			double amount = m_AmountUpDown.Value;
			double payments = m_TermUpDown.Value * 12;
			double monthlyRate = m_RateUpDown.Value / 100 / 12;

			// Calculate the repayment values
			double x = Math.Pow(1 + monthlyRate, payments);
			double monthly = (amount * x * monthlyRate) / (x - 1);
			double total = monthly * payments;
			double interest = total - amount;

			// Display the result
			m_MonthlyPaymentLabel.Text = monthly.ToString("C");
			m_TotalPaymentsLabel.Text = total.ToString("C");
			m_TotalInterestLabel.Text = interest.ToString("C");
		}
		private NListBoxItem CreateListBoxItem(NImage icon, string languageName)
		{
			NPairBox pairBox = new NPairBox(icon, languageName, ENPairBoxRelation.Box1BeforeBox2);
			pairBox.Spacing = NDesign.VerticalSpacing;
			
			NListBoxItem item = new NListBoxItem(pairBox);
			item.Text = languageName;
			return item;
		}

		#endregion

		#region Event Handlers

		private void OnUpDownValueChanged(NValueChangeEventArgs arg)
		{
			CalculateResult();
		}
		private void OnListBoxItemSelected(NSelectEventArgs<NListBoxItem> arg)
		{
			NListBoxItem selectedItem = arg.Item;

			switch (selectedItem.Text)
			{
				case EnglishLanguageName:
					// Load the English dictionary
					m_EnglishStream.Position = 0;
					NLocalizationDictionary.Instance.LoadFromStream(m_EnglishStream);
					break;
				case BulgarianLanguageName:
					// Load the Bulgarian dictionary
					m_BulgarianStream.Position = 0;
					NLocalizationDictionary.Instance.LoadFromStream(m_BulgarianStream);
					break;
				case GermanLanguageName:
					// Load the German dictionary
					m_GermanStream.Position = 0;
					NLocalizationDictionary.Instance.LoadFromStream(m_GermanStream);
					break;
			}

			// Recreate the Loan Calculator
			m_CalculatorHost.Content = CreateLoanCalculator();
		}

		#endregion

		#region Fields

		private NContentHolder m_CalculatorHost;

		private NNumericUpDown m_AmountUpDown;
		private NNumericUpDown m_TermUpDown;
		private NNumericUpDown m_RateUpDown;

		private NLabel m_MonthlyPaymentLabel;
		private NLabel m_TotalPaymentsLabel;
		private NLabel m_TotalInterestLabel;

		private Stream m_EnglishStream;
		private Stream m_BulgarianStream;
		private Stream m_GermanStream;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NLocalizationExample.
		/// </summary>
		public static readonly NSchema NLocalizationExampleSchema;

		#endregion

		#region Constants

		private const string EnglishLanguageName = "English (US)";
		private const string BulgarianLanguageName = "Bulgarian";
		private const string GermanLanguageName = "German";

		#endregion
	}
}