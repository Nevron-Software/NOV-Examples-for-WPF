using System;
using System.Globalization;

using Nevron.Nov.Dom;
using Nevron.Nov.Editors;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.UI
{
	public class NPredefinedSymbolsExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NPredefinedSymbolsExample()
		{
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NPredefinedSymbolsExample()
		{
			NPredefinedSymbolsExampleSchema = NSchema.Create(typeof(NPredefinedSymbolsExample), NExampleBase.NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			m_SymbolsTable = new NTableFlowPanel();
			m_SymbolsTable.BackgroundFill = new NColorFill(NColor.White);
			m_SymbolsTable.HorizontalSpacing = NDesign.HorizontalSpacing * 3;
			m_SymbolsTable.VerticalSpacing = NDesign.VerticalSpacing * 3;
			m_SymbolsTable.Direction = ENHVDirection.LeftToRight;
			m_SymbolsTable.MaxOrdinal = 4;
			m_SymbolsTable.Padding = new NMargins(NDesign.HorizontalSpacing, NDesign.VerticalSpacing,
				NDesign.HorizontalSpacing * 6, NDesign.VerticalSpacing);

			RecreateSymbols();

			NScrollContent scrollContent = new NScrollContent(m_SymbolsTable);
			scrollContent.HorizontalPlacement = ENHorizontalPlacement.Left;
			return scrollContent;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			
			// Create the color box
			m_ColorBox = new NColorBox();
			m_ColorBox.SelectedColor = DefaultSymbolColor;
			m_ColorBox.SelectedColorChanged += OnColorBoxSelectedColorChanged;
			stack.Add(NPairBox.Create("Color:", m_ColorBox));

			// Create the size radio button group
			NStackPanel radioStack = new NStackPanel();
			double size = InitialSize;
			for (int i = 0; i < 4; i++)
			{
				string sizeStr = size.ToString(CultureInfo.InvariantCulture);
				NRadioButton radioButton = new NRadioButton(sizeStr + "x" + sizeStr);
				radioStack.Add(radioButton);
				size *= 2;
			}

			m_RadioGroup = new NRadioButtonGroup(radioStack);
			m_RadioGroup.SelectedIndex = 0;
			m_RadioGroup.SelectedIndexChanged += OnRadioGroupSelectedIndexChanged;
			NPairBox pairBox = NPairBox.Create("Size:", m_RadioGroup);
			pairBox.Box1.VerticalPlacement = ENVerticalPlacement.Top;
			stack.Add(pairBox);

			return new NUniSizeBoxGroup(stack);
		}

		protected override string GetExampleDescription()
		{
			int symbolCount = NEnum.GetValues<ENSymbolShape>().Length;
			string symbolCountStr = symbolCount.ToString(CultureInfo.InvariantCulture);

			return @"
<p>
	Nevron Open Vision provides support for drawing of vector based shapes called symbols. The advantage of such vector
	based shapes over regular raster images is that they do not blur and look nice at any size. This example demonstrates
	how to create and use predefined symbols. Nevron Open Vision currently provides support for <b>" + symbolCountStr + @"
	predefined symbols</b>. Use the radio buttons on the right to see the symbols at different sizes.
</p>
";
		}

		#endregion

		#region Implementation

		private void RecreateSymbols()
		{
			NColor color = m_ColorBox != null ? m_ColorBox.SelectedColor : DefaultSymbolColor;
			double length = InitialSize * Math.Pow(2, m_RadioGroup != null ? m_RadioGroup.SelectedIndex : 0);
			NSize size = new NSize(length, length);

			m_SymbolsTable.Clear();

			ENSymbolShape[] symbolShapes = NEnum.GetValues<ENSymbolShape>();
			int count = symbolShapes.Length / 2 + symbolShapes.Length % 2;
			for (int i = 0; i < count; i++)
			{
				// Add a symbol box to the first column
				int column1Index = i;
				AddSymbolBox(symbolShapes[column1Index], size, color);

				// Add a symbol box to the second column
				int column2Index = count + i;
				if (column2Index < symbolShapes.Length)
				{
					NSymbolBox symbolBox = AddSymbolBox(symbolShapes[column2Index], size, color);
					symbolBox.Margins = new NMargins(NDesign.HorizontalSpacing * 10, 0, 0, 0);
				}
			}
		}
		private NSymbolBox AddSymbolBox(ENSymbolShape symbolShape, NSize size, NColor color)
		{
			NSymbol symbol = NSymbol.Create(symbolShape, size, color);
			NSymbolBox symbolBox = new NSymbolBox(symbol);
			m_SymbolsTable.Add(symbolBox);

			NLabel label = new NLabel(NStringHelpers.InsertSpacesBeforeUppersAndDigits(symbolShape.ToString()));
			label.VerticalPlacement = ENVerticalPlacement.Center;
			m_SymbolsTable.Add(label);

			return symbolBox;
		}

		#endregion

		#region Event Handlers

		private void OnColorBoxSelectedColorChanged(NValueChangeEventArgs arg)
		{
			RecreateSymbols();
		}
		private void OnRadioGroupSelectedIndexChanged(NValueChangeEventArgs arg)
		{
			RecreateSymbols();
		}

		#endregion

		#region Fields

		private NTableFlowPanel m_SymbolsTable;
		private NColorBox m_ColorBox;
		private NRadioButtonGroup m_RadioGroup;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NPredefinedSymbolsExample.
		/// </summary>
		public static readonly NSchema NPredefinedSymbolsExampleSchema;

		#endregion

		#region Constants

		private const double InitialSize = 16;
		private static readonly NColor DefaultSymbolColor = NColor.MediumBlue;

		#endregion
	}
}