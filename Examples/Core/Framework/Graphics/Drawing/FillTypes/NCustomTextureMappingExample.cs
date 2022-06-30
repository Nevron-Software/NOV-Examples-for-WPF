using System;
using Nevron.Nov.Dom;
using Nevron.Nov.Editors;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Framework
{
	public class NCustomTextureMappingExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NCustomTextureMappingExample()
		{
		}
		/// <summary>
		/// Static constructor.
		/// </summary>
		static NCustomTextureMappingExample()
		{
			NCustomTextureMappingExampleSchema = NSchema.Create(typeof(NCustomTextureMappingExample), NExampleBase.NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			m_PathAngle = 0;
			m_PathPositionX = 200;
			m_PathPositionY = 100;

			m_Stroke = new NStroke(1, NColor.Red);

			// create an image fill using an embedded image
			m_ImageFill = new NImageFill(NResources.Image_Artistic_Plane_png);

			// create a custom texture mapping and assign it to the image fill
			m_MyTextureMapping = new MyTextureMapping();
			m_MyTextureMapping.TextureAngle = 45;
			m_MyTextureMapping.PinPoint = new NPoint(m_PathPositionX, m_PathPositionY);
			m_ImageFill.TextureMapping = m_MyTextureMapping;

			m_Canvas = new NCanvas();
			m_Canvas.PreferredSize = new NSize(800, 600);
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
			// path rotation angle editor
			m_PathAngleSpin = new NNumericUpDown();
			m_PathAngleSpin.Minimum = 0;
			m_PathAngleSpin.Maximum = 360;
			m_PathAngleSpin.Value = m_PathAngle;
			m_PathAngleSpin.Step = 1;
			m_PathAngleSpin.DecimalPlaces = 1;
			m_PathAngleSpin.ValueChanged += new Function<NValueChangeEventArgs>(OnNumericUpDownValueChanged);

			// path rotation angle editor
			m_TextureAngleSpin = new NNumericUpDown();
			m_TextureAngleSpin.Minimum = 0;
			m_TextureAngleSpin.Maximum = 360;
			m_TextureAngleSpin.Value = m_MyTextureMapping.TextureAngle;
			m_TextureAngleSpin.Step = 1;
			m_TextureAngleSpin.DecimalPlaces = 1;
			m_TextureAngleSpin.ValueChanged += new Function<NValueChangeEventArgs>(OnNumericUpDownValueChanged);

			// X position editor
			m_XPositionSpin = new NNumericUpDown();
			m_XPositionSpin.Minimum = 0;
			m_XPositionSpin.Maximum = 800;
			m_XPositionSpin.Value = m_PathPositionX;
			m_XPositionSpin.Step = 1;
			m_XPositionSpin.DecimalPlaces = 1;
			m_XPositionSpin.ValueChanged += new Function<NValueChangeEventArgs>(OnNumericUpDownValueChanged);

			// Y position editor
			m_YPositionSpin = new NNumericUpDown();
			m_YPositionSpin.Minimum = 0;
			m_YPositionSpin.Maximum = 600;
			m_YPositionSpin.Value = m_PathPositionY;
			m_YPositionSpin.Step = 1;
			m_YPositionSpin.DecimalPlaces = 1;
			m_YPositionSpin.ValueChanged += new Function<NValueChangeEventArgs>(OnNumericUpDownValueChanged);

			NStackPanel stack = new NStackPanel();
			stack.FillMode = ENStackFillMode.None;
			stack.FitMode = ENStackFitMode.None;
			stack.Add(NPairBox.Create("Path Angle (degrees):", m_PathAngleSpin));
			stack.Add(NPairBox.Create("Texture Angle (degrees):", m_TextureAngleSpin));
			stack.Add(NPairBox.Create("X Position:", m_XPositionSpin));
			stack.Add(NPairBox.Create("Y Position:", m_YPositionSpin));

			return new NUniSizeBoxGroup(stack);
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>
	In cases when the built-in texture mappings do not fit your requirements you can implement and use your own texture mapping types.
	The custom texture mapping presented in this example demonstrates how the texture can be rotated independently of the textured shape.
	Use the controls to the right to set the rotation angles of the shape and the texture.
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

			// Create a transform matrix for the graphics path
			NMatrix matrix = NMatrix.CreateRotationMatrix(m_PathAngle * NAngle.Degree2Rad, NPoint.Zero);
			matrix.Translate(m_PathPositionX, m_PathPositionY);

			// Create a graphics path containing a rectangle and transform it
			NGraphicsPath path = new NGraphicsPath();
			path.AddRectangle(0, 0, RectWidth, RectHeight);
			path.Transform(matrix);

			// Paint the graphics path
			NPaintVisitor pv = args.PaintVisitor;
			pv.SetStroke(m_Stroke);
			pv.SetFill(m_ImageFill);
			pv.PaintPath(path);

			// Paint a border around the canvas
			pv.ClearFill();
			pv.SetStroke(NColor.Black, 1);
			pv.PaintRectangle(0, 0, canvas.Width, canvas.Height);
		}
		private void OnNumericUpDownValueChanged(NValueChangeEventArgs args)
		{
			m_PathAngle = m_PathAngleSpin.Value;
			m_PathPositionX = m_XPositionSpin.Value;
			m_PathPositionY = m_YPositionSpin.Value;

			m_MyTextureMapping.PinPoint = new NPoint(m_PathPositionX, m_PathPositionY);
			m_MyTextureMapping.TextureAngle = m_TextureAngleSpin.Value;

			m_Canvas.InvalidateDisplay();
		}

		#endregion

		#region Constants

		internal const double RectWidth = 300;
		internal const double RectHeight = 240;

		#endregion

		#region Fields

		NCanvas m_Canvas;
		NStroke m_Stroke;
		NImageFill m_ImageFill;
		MyTextureMapping m_MyTextureMapping;

		NNumericUpDown m_TextureAngleSpin;
		NNumericUpDown m_PathAngleSpin;
		NNumericUpDown m_XPositionSpin;
		NNumericUpDown m_YPositionSpin;

		double m_PathAngle;
		double m_PathPositionX;
		double m_PathPositionY;

		#endregion

		#region Schema

		public static readonly NSchema NCustomTextureMappingExampleSchema;

		#endregion
	}

	public class MyTextureMapping : NCustomTextureMapping
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public MyTextureMapping()
		{
		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static MyTextureMapping()
		{
			MyTextureMappingSchema = NSchema.Create(typeof(MyTextureMapping), NCustomTextureMapping.NCustomTextureMappingSchema);

			TextureAngleProperty = MyTextureMappingSchema.AddSlot("TextureAngle", NDomType.Double, 0.0d);
			PinPointProperty = MyTextureMappingSchema.AddSlot("PinPoint", NDomType.NPoint, NPoint.Zero);
		}

		#endregion

		#region Properties

		/// <summary>
		/// The texture angle (in degrees)
		/// </summary>
		public double TextureAngle
		{
			get
			{
				return (double)GetValue(TextureAngleProperty);
			}
			set
			{
				SetValue(TextureAngleProperty, value);
			}
		}
		/// <summary>
		/// The point around which the texture is rotated
		/// </summary>
		public NPoint PinPoint
		{
			get
			{
				return (NPoint)GetValue(PinPointProperty);
			}
			set
			{
				SetValue(PinPointProperty, value);
			}
		}

		#endregion

		#region Protected Overrides from NCustomTextureMapping

        public override void GetTextureMappingInfo(out ENTileMode tileMode, out NTextureCalibrator textureCalibrator)
        {
            tileMode = ENTileMode.Tile;
            textureCalibrator = new MyTextureCalibrator(this);
        }

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with MyTextureMapping
		/// </summary>
		public static readonly NSchema MyTextureMappingSchema;
		/// <summary>
		/// Reference to the TextureAngle property
		/// </summary>
		public static readonly NProperty TextureAngleProperty;
		/// <summary>
		/// Reference to the PinPoint property
		/// </summary>
		public static readonly NProperty PinPointProperty;

		#endregion

        class MyTextureCalibrator : NTextureCalibrator
        {
            public MyTextureCalibrator(MyTextureMapping textureMapping)
            {
                m_TextureMapping = textureMapping;
            }

            public override NMatrix Calibrate(NPaintVisitor visitor, double imgWidth, double imgHeight, NRectangle targetRect)
            {
                // Initialize the image transform
                NMatrix matrix = NMatrix.Identity;

                // Scale the image so that it fits 2 times in width and 3 times in height
                matrix.Scale(
                    NCustomTextureMappingExample.RectWidth / (2.0 * imgWidth),
                    NCustomTextureMappingExample.RectHeight / (3.0 * imgHeight));

                // Rotate the image to the specified angle
                matrix.Rotate(m_TextureMapping.TextureAngle * NAngle.Degree2Rad);

                // Translate the image to the specfied pin point
                matrix.Translate(m_TextureMapping.PinPoint.X, m_TextureMapping.PinPoint.Y);

                return matrix;
            }
            public override object DeepClone()
            {
                return new MyTextureCalibrator(m_TextureMapping);
            }

            MyTextureMapping m_TextureMapping;
        }
    }
}
