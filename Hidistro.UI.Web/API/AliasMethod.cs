using System;
using System.Collections.Generic;

namespace Hidistro.UI.Web.API
{
	public class AliasMethod
	{
		private int[] _alias;

		private double[] _probability;

		public AliasMethod(List<double> probabilities)
		{
			this._probability = new double[probabilities.Count];
			this._alias = new int[probabilities.Count];
			double num = 1.0 / (double)probabilities.Count;
			Stack<int> stack = new Stack<int>();
			Stack<int> stack2 = new Stack<int>();
			for (int i = 0; i < probabilities.Count; i++)
			{
				if (probabilities[i] >= num)
				{
					stack2.Push(i);
				}
				else
				{
					stack.Push(i);
				}
			}
			while (stack.Count > 0 && stack2.Count > 0)
			{
				int num2 = stack.Pop();
				int num3 = stack2.Pop();
				this._probability[num2] = probabilities[num2] * (double)probabilities.Count;
				this._alias[num2] = num3;
				probabilities[num3] = probabilities[num3] + probabilities[num2] - num;
				if (probabilities[num3] >= num)
				{
					stack2.Push(num3);
				}
				else
				{
					stack.Push(num3);
				}
			}
			while (stack.Count > 0)
			{
				this._probability[stack.Pop()] = 1.0;
			}
			while (stack2.Count > 0)
			{
				this._probability[stack2.Pop()] = 1.0;
			}
		}

		public int next()
		{
			long ticks = DateTime.Now.Ticks;
			int num = (int)(ticks & 4294967295u) | (int)(ticks >> 32);
			num = num + Guid.NewGuid().GetHashCode() + new Random().Next(0, 100);
			Random random = new Random(num);
			int num2 = random.Next(this._probability.Length);
			return (random.NextDouble() < this._probability[num2]) ? num2 : this._alias[num2];
		}
	}
}
