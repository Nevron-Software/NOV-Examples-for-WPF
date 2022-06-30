using System;
using Nevron.Nov.DataStructures;
using Nevron.Nov.Dom;
using Nevron.Nov.Editors;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Framework
{
	public class NAreaOperationsExample : NExampleBase
	{
		#region Constructors

		public NAreaOperationsExample()
		{
		}
		static NAreaOperationsExample()
		{
			NAreaOperationsExampleSchema = NSchema.Create(typeof(NAreaOperationsExample), NExampleBase.NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			m_InputPaths = new NList<NGraphicsPath>();
			m_OutputPath = new NGraphicsPath();

			m_Canvas = new NCanvas();
			m_Canvas.HorizontalPlacement = ENHorizontalPlacement.Fit;
			m_Canvas.VerticalPlacement = ENVerticalPlacement.Fit;
			m_Canvas.PrePaint += new Function<NCanvasPaintEventArgs>(OnCanvasPrePaint);
			m_Canvas.BackgroundFill = new NColorFill(NColor.White);

			NScrollContent scrollContent = new NScrollContent();
			scrollContent.Content = m_Canvas;

			// Add 3 circles
			NGraphicsPath path = new NGraphicsPath();
			path.AddCircle(100, 100, 100);
			m_InputPaths.Add(path);

			path = new NGraphicsPath();
			path.AddCircle(250, 100, 100);
			m_InputPaths.Add(path);

			return scrollContent;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			stack.FillMode = ENStackFillMode.None;
			stack.FitMode = ENStackFitMode.None;

			m_OperatorCombo = new NComboBox();
			m_OperatorCombo.Items.Add(new NComboBoxItem("Union"));
			m_OperatorCombo.Items.Add(new NComboBoxItem("Intersect"));
			m_OperatorCombo.Items.Add(new NComboBoxItem("Subtract"));
			m_OperatorCombo.Items.Add(new NComboBoxItem("Exclusive OR"));
			m_OperatorCombo.SelectedIndex = 0;
			m_OperatorCombo.SelectedIndexChanged += new Function<NValueChangeEventArgs>(OnAreaOperatorComboSelectedIndexChanged);
			stack.Add(m_OperatorCombo);

			// random path creation
			NButton randomRectButton = new NButton("Random Rectangle");
			randomRectButton.Click += new Function<NEventArgs>(OnRandomRectButtonClick);
			stack.Add(randomRectButton);

			NButton randomEllipseButton = new NButton("Random Ellipse");
			randomEllipseButton.Click += new Function<NEventArgs>(OnRandomEllipseButtonClick);
			stack.Add(randomEllipseButton);

			NButton randomTriangleButton = new NButton("Random Triangle");
			randomTriangleButton.Click += new Function<NEventArgs>(OnRandomTriangleButtonClick);
			stack.Add(randomTriangleButton);

			NButton clearButton = new NButton("Clear");
			clearButton.Click += new Function<NEventArgs>(OnClearButtonClick);
			stack.Add(clearButton);

			m_ShowInputPathInteriors = new NCheckBox("Show Input Path Interiors");
			m_ShowInputPathInteriors.Checked = true;
			m_ShowInputPathInteriors.CheckedChanged += new Function<NValueChangeEventArgs>(OnShowInputPathInteriorsCheckedChanged);
			stack.Add(m_ShowInputPathInteriors);

			m_ShowInputPathOutlines = new NCheckBox("Show Input Path Outlines");
			m_ShowInputPathOutlines.Checked = true;
			m_ShowInputPathOutlines.CheckedChanged += new Function<NValueChangeEventArgs>(OnShowInputPathOutlinesCheckedChanged);
			stack.Add(m_ShowInputPathOutlines);

			m_ShowOutputPathOutline = new NCheckBox("Show Output Path Outline");
			m_ShowOutputPathOutline.Checked = true;
			m_ShowOutputPathOutline.CheckedChanged += new Function<NValueChangeEventArgs>(OnShowInputPathInteriorsCheckedChanged);
			stack.Add(m_ShowOutputPathOutline);

			m_ShowOutputPathInterior = new NCheckBox("Show Output Path Interior");
			m_ShowOutputPathInterior.Checked = true;
			m_ShowOutputPathInterior.CheckedChanged += new Function<NValueChangeEventArgs>(OnShowInputPathInteriorsCheckedChanged);
			stack.Add(m_ShowOutputPathInterior);

			UpdateOuputPath();

			return stack;
		}
        protected override string GetExampleDescription()
        {
            return @"
<p>
Demonstrates the Area Set Operations implemented by the NRegion class. Area Set Operations are performed on the closed areas represented by regions. 
Via these operations you can construct very complex solid geometries by combining primitive ones.
</p>
";
        }

		#endregion

		#region Implementation

		private void UpdateOuputPath()
		{
			NRectangle bounds = new NRectangle();
			int count = m_InputPaths.Count;

			// compute the ouput path and the bounds
			if (count != 0)
			{
				NRegion result = NRegion.FromPath(m_InputPaths[0], ENFillRule.EvenOdd);
                bounds = result.Bounds;

				for (int i = 1; i < count; i++)
				{
					NRegion operand = NRegion.FromPath(m_InputPaths[i], ENFillRule.EvenOdd);
					bounds = NRectangle.Union(bounds, operand.Bounds);

					switch (m_OperatorCombo.SelectedIndex)
					{
						case 0: // union
							result = result.Union(operand);
							break;
						case 1: // intersection
							result = result.Intersect(operand);
							break;
						case 2:
							result = result.Subtract(operand);
							break;
						case 3:
							result = result.ExclusiveOr(operand);
							break;
					}
				}

				m_OutputPath = new NGraphicsPath(result.GetPath());
			}
			else
			{
				m_OutputPath = new NGraphicsPath();
			}

			// normalize the coordinates
			for (int i = 0; i < count; i++)
			{
				NGraphicsPath path = m_InputPaths[i];
				//NRectangle pathBounds = path.GetBounds();
				path.Translate(-bounds.X, -bounds.Y);
			}
			m_OutputPath.Translate(-bounds.X, -bounds.Y);

			m_Canvas.PreferredSize = new NSize(bounds.Width + 20, bounds.Height + 20);
			m_Canvas.InvalidateDisplay();
		}

		#endregion

		#region Event Handlers

		private void OnCanvasPrePaint(NCanvasPaintEventArgs args)
		{
			args.PaintVisitor.PushTransform(NMatrix.CreateTranslationMatrix(10, 10)); 

			// input path interiors
			if (m_ShowInputPathInteriors.Checked)
			{
                args.PaintVisitor.ClearStyles();
				args.PaintVisitor.SetFill(NColor.LightBlue);

				for (int i = 0; i < m_InputPaths.Count; i++)
				{
					args.PaintVisitor.PaintPath(m_InputPaths[i]);
				}
			}

			// input path outlines
			if (m_ShowInputPathOutlines.Checked)
			{
                args.PaintVisitor.ClearStyles();
				args.PaintVisitor.SetStroke(NColor.Black, 1);

				for (int i = 0; i < m_InputPaths.Count; i++)
				{
					args.PaintVisitor.PaintPath(m_InputPaths[i]);
				}
			}

			// output path interior
			if (m_ShowOutputPathInterior.Checked)
			{
                args.PaintVisitor.ClearStyles();
				args.PaintVisitor.SetFill(new NColor(NColor.LightCoral, 128));
				args.PaintVisitor.PaintPath(m_OutputPath);
			}

			// output path outline
			if (m_ShowOutputPathOutline.Checked)
			{
                args.PaintVisitor.ClearStyles();
				args.PaintVisitor.SetStroke(NColor.Black, 2);
				args.PaintVisitor.PaintPath(m_OutputPath);
			}

			args.PaintVisitor.PopTransform();
		}
		private void OnClearButtonClick(NEventArgs args)
		{
			m_InputPaths.Clear();
			UpdateOuputPath();
		}
		private void OnRandomEllipseButtonClick(NEventArgs args)
		{
			NRectangle rect = new NRectangle(m_Random.Next(500), m_Random.Next(500), m_Random.Next(500), m_Random.Next(500));
			rect.Normalize();

			NGraphicsPath path = new NGraphicsPath();
			path.AddEllipse(rect);

			m_InputPaths.Add(path);

			UpdateOuputPath();
		}
		private void OnRandomRectButtonClick(NEventArgs args)
		{
			NRectangle rect = new NRectangle(m_Random.Next(500), m_Random.Next(500), m_Random.Next(500), m_Random.Next(500));
			rect.Normalize();

			NGraphicsPath path = new NGraphicsPath();
			path.AddRectangle(rect);

			m_InputPaths.Add(path);
			UpdateOuputPath();
		}
		private void OnRandomTriangleButtonClick(NEventArgs args)
		{
			NPoint p1 = new NPoint(m_Random.Next(500), m_Random.Next(500));
			NPoint p2 = new NPoint(m_Random.Next(500), m_Random.Next(500));
			NPoint p3 = new NPoint(m_Random.Next(500), m_Random.Next(500));

			NGraphicsPath path = new NGraphicsPath();
			path.AddTriangle(new NTriangle(p1, p2, p3));
			m_InputPaths.Add(path);
			UpdateOuputPath();
		}

		private void OnShowInputPathInteriorsCheckedChanged(NValueChangeEventArgs args)
		{
			m_Canvas.InvalidateDisplay();
		}
		private void OnShowInputPathOutlinesCheckedChanged(NValueChangeEventArgs args)
		{
			m_Canvas.InvalidateDisplay();
		}
		private void OnAreaOperatorComboSelectedIndexChanged(NValueChangeEventArgs args)
		{
			UpdateOuputPath();
		}

		#endregion

		#region Fields

		NCanvas m_Canvas;
		NGraphicsPath m_OutputPath;

		NList<NGraphicsPath> m_InputPaths;
		Random m_Random = new Random(300);

		NComboBox m_OperatorCombo;
		NCheckBox m_ShowInputPathInteriors;
		NCheckBox m_ShowInputPathOutlines;
		NCheckBox m_ShowOutputPathOutline;
		NCheckBox m_ShowOutputPathInterior;

		#endregion

		#region Schema

		public static readonly NSchema NAreaOperationsExampleSchema;

		#endregion
	}
}