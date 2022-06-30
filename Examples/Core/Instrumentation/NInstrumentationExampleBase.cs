using Nevron.Nov.DataStructures;
using Nevron.Nov.Editors;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Text;
using Nevron.Nov.UI;
using System.Text;

namespace Nevron.Nov.Examples.Gauge
{
    /// <summary>
    /// Base example for all instrumentation examples
    /// </summary>
    public abstract class NInstrumentationExampleBase : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        public NInstrumentationExampleBase()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        static NInstrumentationExampleBase()
        {
            NInstrumentationExampleBaseSchema = NSchema.Create(typeof(NInstrumentationExampleBase), NExampleBase.NExampleBaseSchema);
        }

        #endregion

		#region Implementation

		protected NBorder CreateBorder()
		{
			return NBorder.CreateThreeColorBorder(NColor.LightGray, NColor.White, NColor.DarkGray, 10, 10);
		}

		#endregion

		#region Static

		public static readonly NSchema NInstrumentationExampleBaseSchema;

        #endregion

		#region Default Values

		protected static NSize defaultLinearHorizontalGaugeSize = new NSize(300, 100);
		protected static NSize defaultLinearVerticalGaugeSize = new NSize(100, 300);
		protected static NSize defaultRadialGaugeSize = new NSize(300, 300);

		#endregion
	}
}
