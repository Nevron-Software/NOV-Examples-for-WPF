using Nevron.Nov.Dom;
using Nevron.Nov.Editors;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Framework
{
	public class NTransformationsAndClippingExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NTransformationsAndClippingExample()
		{
		}
		/// <summary>
		/// Static constructor.
		/// </summary>
		static NTransformationsAndClippingExample()
		{
			NTransformationsAndClippingExampleSchema = NSchema.Create(typeof(NTransformationsAndClippingExample), NExampleBase.NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			m_PositionX = 100;
			m_PositionY = 220;
			m_Angle1 = -50;
			m_Angle2 = 40;
			m_Angle3 = 90;
			m_ClipRect = new NRectangle(20, 20, 500, 360);

			m_Canvas = new NCanvas();
			m_Canvas.PreferredSize = new NSize(600, 400);
			m_Canvas.BackgroundFill = new NColorFill(new NColor(220, 220, 200));
			m_Canvas.HorizontalPlacement = ENHorizontalPlacement.Center;
			m_Canvas.VerticalPlacement = ENVerticalPlacement.Center;
			m_Canvas.PrePaint += new Function<NCanvasPaintEventArgs>(OnCanvasPrePaint);

			NScrollContent scroll = new NScrollContent();
			scroll.Content = m_Canvas;
			scroll.NoScrollHAlign = ENNoScrollHAlign.Center;
			scroll.NoScrollVAlign = ENNoScrollVAlign.Center;
			return scroll;
		}
		protected override NWidget CreateExampleControls()
		{
			m_PositionXUpDown = CreateNumericUpDown(40, 400, m_PositionX);
			m_PositionYUpDown = CreateNumericUpDown(100, 300, m_PositionY);
			m_Angle1UpDown = CreateNumericUpDown(-90, -10, m_Angle1);
			m_Angle2UpDown = CreateNumericUpDown(-100, 100, m_Angle2);
			m_Angle3UpDown = CreateNumericUpDown(-100, 100, m_Angle3);

			NStackPanel roboArmControlsStack = new NStackPanel();
			roboArmControlsStack.Add(NPairBox.Create("X:", m_PositionXUpDown));
			roboArmControlsStack.Add(NPairBox.Create("Y:", m_PositionYUpDown));
			roboArmControlsStack.Add(NPairBox.Create("Angle 1:", m_Angle1UpDown));
			roboArmControlsStack.Add(NPairBox.Create("Angle 2:", m_Angle2UpDown));
			roboArmControlsStack.Add(NPairBox.Create("Angle 3:", m_Angle3UpDown));

			NGroupBox roboArmGroupBox = new NGroupBox("Robo Arm");
			roboArmGroupBox.Content = roboArmControlsStack;

			m_ClipRectXUpDown = CreateNumericUpDown(0, 600, m_ClipRect.X);
			m_ClipRectYUpDown = CreateNumericUpDown(0, 400, m_ClipRect.Y);
			m_ClipRectWUpDown = CreateNumericUpDown(0, 600, m_ClipRect.Width);
			m_ClipRectHUpDown = CreateNumericUpDown(0, 400, m_ClipRect.Height);

			NStackPanel clipRectControlsStack = new NStackPanel();
			clipRectControlsStack.Add(NPairBox.Create("X:", m_ClipRectXUpDown));
			clipRectControlsStack.Add(NPairBox.Create("Y:", m_ClipRectYUpDown));
			clipRectControlsStack.Add(NPairBox.Create("Width:", m_ClipRectWUpDown));
			clipRectControlsStack.Add(NPairBox.Create("Height:", m_ClipRectHUpDown));

			NGroupBox clipRectGroupBox = new NGroupBox("Clip Rect");
			clipRectGroupBox.Content = clipRectControlsStack;

			// create a stack and put the controls in it
			NStackPanel stack = new NStackPanel();
			stack.Add(roboArmGroupBox);
			stack.Add(clipRectGroupBox);

			return new NUniSizeBoxGroup(stack);
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>
	This example demonstrates geometric transforms and clipping with the NOV graphics.
</p>
";
		}


		#endregion

		#region Event Handlers

		private void OnCanvasPrePaint(NCanvasPaintEventArgs args)
		{
			NCanvas canvas = args.TargetNode as NCanvas;
			if (canvas == null)
				return;

			canvas.HorizontalPlacement = ENHorizontalPlacement.Center;
			canvas.VerticalPlacement = ENVerticalPlacement.Center;

			NPaintVisitor pv = args.PaintVisitor;
			pv.ClearStyles(); 
			pv.SetStroke(NColor.MidnightBlue, 1);
			pv.SetFill(NColor.LightSteelBlue);

			NMatrix m1 = NMatrix.Identity;
			m1.Rotate(NAngle.Degree2Rad * m_Angle1);

			NMatrix m2 = NMatrix.Identity;
			m2.Rotate(NAngle.Degree2Rad * m_Angle2);
			m2.Translate(100, 0);
	
			NMatrix m3 = NMatrix.Identity;
			m3.Rotate(NAngle.Degree2Rad * m_Angle3);
			m3.Translate(100, 0);

			NRegion clipRegion = NRegion.FromRectangle(m_ClipRect);

			pv.PushClip(clipRegion);

			pv.PushTransform(new NMatrix(m_PositionX, 0));
			PaintVerticalBar(pv);

			pv.PushTransform(new NMatrix(0, m_PositionY));
			PaintBase(pv);

			pv.PushTransform(m1);
			PaintLink(pv, 20);
			PaintJoint(pv, 20);

			pv.PushSnapToPixels(false);
			pv.PushTransform(m2);
			PaintLink(pv, 16);
			PaintJoint(pv, 16);

			pv.PushTransform(m3);
			PaintGripper(pv);
			PaintJoint(pv, 12);

			pv.PopTransform();// m3
			pv.PopTransform();// m2
			pv.PopTransform();// m1
			pv.PopTransform();// mTY
			pv.PopTransform();// mTX
			pv.PopSnapToPixels();
			pv.PopClip();

			// paint a border around the clip rectangle
			pv.ClearFill();
			pv.SetStroke(NColor.Red, 1);
			pv.PaintRectangle(m_ClipRect);

			// paint a border around the canvas
			pv.SetStroke(NColor.Black, 1);
			pv.PaintRectangle(0, 0, canvas.Width, canvas.Height);
		}
		private void OnNumericUpDownValueChanged(NValueChangeEventArgs args)
		{
			if (m_Canvas == null)
				return;

			m_PositionX = m_PositionXUpDown.Value;
			m_PositionY = m_PositionYUpDown.Value;
			m_Angle1 = m_Angle1UpDown.Value;
			m_Angle2 = m_Angle2UpDown.Value;
			m_Angle3 = m_Angle3UpDown.Value;

			m_ClipRect.X = m_ClipRectXUpDown.Value;			
			m_ClipRect.Y = m_ClipRectYUpDown.Value;
			m_ClipRect.Width = m_ClipRectWUpDown.Value;
			m_ClipRect.Height = m_ClipRectHUpDown.Value;

			m_Canvas.InvalidateDisplay();
		}

		#endregion

		#region Implementation

		void PaintJoint(NPaintVisitor pv, double radius)
		{
			double innerR = radius - 3;

			pv.PaintEllipse(-radius, -radius, 2 * radius, 2 * radius);
			pv.PaintEllipse(-innerR, -innerR, 2 * innerR, 2 * innerR);
		}
		void PaintLink(NPaintVisitor pv, double radius)
		{
			double r = radius - 8;
			pv.PaintRectangle(0, -r, 100, 2 * r);
		}
		void PaintGripper(NPaintVisitor pv)
		{
			if (m_ArmGripperPath == null)
			{
				m_ArmGripperPath = new NGraphicsPath();
				m_ArmGripperPath.StartFigure(0, -6);
				m_ArmGripperPath.LineTo(20, -6);
				m_ArmGripperPath.LineTo(20, -14);
				m_ArmGripperPath.LineTo(30, -14);
				m_ArmGripperPath.LineTo(30, 14);
				m_ArmGripperPath.LineTo(20, 14);
				m_ArmGripperPath.LineTo(20, 6);
				m_ArmGripperPath.LineTo(0, 6);
				m_ArmGripperPath.CloseFigure();

				m_ArmGripperPath.StartFigure(30, -14);
				m_ArmGripperPath.LineTo(40, -14);
				m_ArmGripperPath.LineTo(50, -10);
				m_ArmGripperPath.LineTo(50, -7);
				m_ArmGripperPath.LineTo(30, -7);
				m_ArmGripperPath.CloseFigure();

				m_ArmGripperPath.StartFigure(30, 14);
				m_ArmGripperPath.LineTo(40, 14);
				m_ArmGripperPath.LineTo(50, 10);
				m_ArmGripperPath.LineTo(50, 7);
				m_ArmGripperPath.LineTo(30, 7);
				m_ArmGripperPath.CloseFigure();
			}

			pv.PaintPath(m_ArmGripperPath);
		}
		void PaintBase(NPaintVisitor pv)
		{
			if (m_ArmBasePath == null)
			{
				m_ArmBasePath = new NGraphicsPath();
				m_ArmBasePath.StartFigure(0, 0);
				m_ArmBasePath.LineTo(-40, 0);
				m_ArmBasePath.LineTo(-40, 50);
				m_ArmBasePath.LineTo(25, 50);
				m_ArmBasePath.LineTo(25, 20);
				m_ArmBasePath.CloseFigure();
			}

			pv.PaintPath(m_ArmBasePath);
		}
		void PaintVerticalBar(NPaintVisitor pv)
		{
			pv.PaintRectangle(-35, 0, 8, 400);
		}

		NNumericUpDown CreateNumericUpDown(double min, double max, double value)
		{
			NNumericUpDown control = new NNumericUpDown();
			control.Minimum = min;
			control.Maximum = max;
			control.Value = value;
			control.Step = 1;
			control.DecimalPlaces = 0;
			control.ValueChanged += new Function<NValueChangeEventArgs>(OnNumericUpDownValueChanged);
			return control;
		}

		#endregion

		#region Fields

		NCanvas m_Canvas;
		NNumericUpDown m_PositionXUpDown;
		NNumericUpDown m_PositionYUpDown;
		NNumericUpDown m_Angle1UpDown;
		NNumericUpDown m_Angle2UpDown;
		NNumericUpDown m_Angle3UpDown;
		NNumericUpDown m_ClipRectXUpDown;
		NNumericUpDown m_ClipRectYUpDown;
		NNumericUpDown m_ClipRectWUpDown;
		NNumericUpDown m_ClipRectHUpDown;

		NGraphicsPath m_ArmBasePath;
		NGraphicsPath m_ArmGripperPath;

		double m_PositionX;
		double m_PositionY;
		double m_Angle1;
		double m_Angle2;
		double m_Angle3;
		NRectangle m_ClipRect;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NTransformationsAndClippingExample.
		/// </summary>
		public static readonly NSchema NTransformationsAndClippingExampleSchema;

		#endregion
	}
}