using Nevron.Nov.Graphics;
using System;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Helper class that generates sample data for stock/financial series
	/// </summary>
	internal class NStockDataGenerator
	{
		#region Constructors

		/// <summary>
		/// Initializer constructor
		/// </summary>
		/// <param name="range"></param>
		/// <param name="reversalFactor"></param>
		/// <param name="valueScale"></param>
		internal NStockDataGenerator(NRange range, double reversalFactor, double valueScale)
		{
			m_Rand = new Random();
			m_Range = range;
			m_Direction = 1;
			m_StepsInCurrentTrend = 0;
			m_Value = 0;
			m_ReversalPorbability = 0;
			m_ReversalFactor = reversalFactor;
			m_ValueScale = valueScale;
		}

		#endregion

		#region Methods

		/// <summary>
		/// Resets the enumerator
		/// </summary>
		internal void Reset()
		{
			m_Direction = 1;
			m_StepsInCurrentTrend = 0;
			m_Value = (m_Range.Begin + m_Range.End) / 2;
			m_ReversalPorbability = 0;
		}
		/// <summary>
		/// Gets the next price value
		/// </summary>
		/// <returns></returns>
		internal double GetNextValue()
		{
			int nNewValueDirection = 0;

			if (m_Value <= m_Range.Begin)
			{
				if (m_Direction == -1)
				{
					m_ReversalPorbability = 1.0;
				}
				else
				{
					m_ReversalPorbability = 0.0;
				}

				nNewValueDirection = 1;
			}
			else if (m_Value >= m_Range.End)
			{
				if (m_Direction == 1)
				{
					m_ReversalPorbability = 1.0;
				}
				else
				{
					m_ReversalPorbability = 0.0;
				}

				nNewValueDirection = -1;
			}
			else
			{
				if (m_Rand.NextDouble() < 0.80)
				{
					nNewValueDirection = m_Direction;
				}
				else
				{
					nNewValueDirection = -m_Direction;
				}

				m_ReversalPorbability += m_StepsInCurrentTrend * m_ReversalFactor;
			}

			if (m_Rand.NextDouble() < m_ReversalPorbability)
			{
				m_Direction = -m_Direction;
				m_ReversalPorbability = 0;
				m_StepsInCurrentTrend = 0;
			}
			else
			{
				m_StepsInCurrentTrend++;
			}

			m_Value += nNewValueDirection * m_Rand.NextDouble() * m_ValueScale;

			return m_Value;
		}

		#endregion

		#region Fields

		private Random m_Rand;
		private NRange m_Range;
		private int m_Direction;
		private int m_StepsInCurrentTrend;
		private double m_Value;
		private double m_ReversalPorbability;
		private double m_ReversalFactor;
		private double m_ValueScale;

		#endregion
	}
}
