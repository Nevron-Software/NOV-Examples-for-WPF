using System;
using System.Globalization;

using Nevron.Nov.Dom;
using Nevron.Nov.Editors;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.UI
{
	public class NCustomSymbolsExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NCustomSymbolsExample()
		{
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NCustomSymbolsExample()
		{
			NCustomSymbolsExampleSchema = NSchema.Create(typeof(NCustomSymbolsExample), NExampleBase.NExampleBaseSchema);
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
			m_SymbolsTable.MaxOrdinal = 2;
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
			m_RadioGroup.SelectedIndex = 1;
			m_RadioGroup.SelectedIndexChanged += OnRadioGroupSelectedIndexChanged;
			NPairBox pairBox = NPairBox.Create("Size:", m_RadioGroup);
			pairBox.Box1.VerticalPlacement = ENVerticalPlacement.Top;
			stack.Add(pairBox);

			return new NUniSizeBoxGroup(stack);
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>
	Nevron Open Vision provides support for drawing of vector based shapes called symbols. The advantage of such vector
	based shapes over regular raster images is that they do not blur and look nice at any size. This example demonstrates
	how to create and use custom symbols. Use the radio buttons on the right to see the symbols at different sizes.
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

			// Create a triangle up symbol
			NPolygonSymbolShape shape = new NPolygonSymbolShape(new NPoint[]{
				 new NPoint(0, size.Height),
				 new NPoint(size.Width * 0.5, 0),
				 new NPoint(size.Width, size.Height)}, ENFillRule.EvenOdd);
			shape.Fill = new NColorFill(color);

			NSymbol symbol1 = new NSymbol();
			symbol1.Add(shape);
			AddSymbolBox(symbol1, "Triangle Up");

			// Create a rectangle with an ellipse
			NRectangleSymbolShape rectShape = new NRectangleSymbolShape(0, 0, size.Width, size.Height);
			rectShape.Fill = new NColorFill(color);

			NEllipseSymbolShape ellipseShape = new NEllipseSymbolShape(size.Width / 4, size.Height / 4,
				size.Width / 2, size.Height / 2);
			ellipseShape.Fill = new NColorFill(color.Invert());

			NSymbol symbol2 = new NSymbol();
			symbol2.Add(rectShape);
			symbol2.Add(ellipseShape);
			AddSymbolBox(symbol2, "Rectangle with an ellipse");
		}
		private void AddSymbolBox(NSymbol symbol, string name)
		{
			m_SymbolsTable.Add(new NSymbolBox(symbol));

			NLabel label = new NLabel(name);
			label.VerticalPlacement = ENVerticalPlacement.Center;
			m_SymbolsTable.Add(label);
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
		/// Schema associated with NCustomSymbolsExample.
		/// </summary>
		public static readonly NSchema NCustomSymbolsExampleSchema;

		#endregion

		#region Constants

		private const double InitialSize = 16;
		private static readonly NColor DefaultSymbolColor = NColor.MediumBlue;

		#endregion
	}
}