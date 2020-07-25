using System;
using System.Collections.Generic;
using System.Linq;

namespace Grabacr07.KanColleWrapper.Models
{
	[Flags]
	public enum ImprovementBonusCalculationOptions
	{	
		Default,
		Firepower,
		AA,
		ASW,
		LoS,
	}
	public static class ExpeditionImprovementBonus
	{
		public static double GetImprovementBonus(this Ship[] ships, ImprovementBonusCalculationOptions option)
		{
			return ships.SelectMany(x => x.EquippedItems)
				.Select(x => GetImprovementBonus(x.Item, option))
				.Sum();
		}

		public static double GetImprovementBonus(this SlotItem slotItem, ImprovementBonusCalculationOptions option)
		{
			var calculator = option.GetCalculator();
			return calculator.GetImprovementBonus(slotItem);
		}

		private static ImprovementBonusCalculator GetCalculator(this ImprovementBonusCalculationOptions option)
		{
			switch (option)
			{
				case ImprovementBonusCalculationOptions.Firepower:
					return new FirepowerCalculator();

				case ImprovementBonusCalculationOptions.AA:
					return new AACalculator();

				case ImprovementBonusCalculationOptions.ASW:
					return new ASWCalculator();

				case ImprovementBonusCalculationOptions.LoS:
					return new LoSCalculator();

				default:
					return EmptyCalculator.Instance;
			}
		}

		private abstract class ImprovementBonusCalculator
		{
			public abstract double GetImprovementBonus(SlotItem slotItem);
		}

		private class FirepowerCalculator: ImprovementBonusCalculator
		{
			public override double GetImprovementBonus(SlotItem slotItem)
			{
				switch (slotItem.Info.Type)
				{
					case SlotItemType.小口径主砲:
					case SlotItemType.対空機銃:
					case SlotItemType.対艦強化弾:
						return 0.5 * Math.Sqrt(slotItem.Level);

					case SlotItemType.中口径主砲:
					case SlotItemType.大口径主砲:
					case SlotItemType.大口径主砲_II:
						return Math.Sqrt(slotItem.Level);

					case SlotItemType.副砲:
						return 0.15 * slotItem.Level;

					default:
						return .0;
				}
			}
		}

		private class AACalculator : ImprovementBonusCalculator
		{
			public override double GetImprovementBonus(SlotItem slotItem)
			{
				switch (slotItem.Info.IconType)
				{
					case SlotItemIconType.AAGun:
						return Math.Sqrt(slotItem.Level);

					case SlotItemIconType.HighAngleGun:
						return 0.3 * slotItem.Level;

					default:
						return .0;
				}

			}
		}

		private class ASWCalculator : ImprovementBonusCalculator
		{
			public override double GetImprovementBonus(SlotItem slotItem)
			{
				switch (slotItem.Info.Type)
				{
					case SlotItemType.ソナー:
					case SlotItemType.大型ソナー: // 改修未実装なのでﾃｷﾄｰ
					case SlotItemType.爆雷:
						return Math.Sqrt(slotItem.Level);

					default:
						return .0;
				}
			}
		}

		private class LoSCalculator : ImprovementBonusCalculator
		{
			public override double GetImprovementBonus(SlotItem slotItem)
			{
				switch (slotItem.Info.Type)
				{
					case SlotItemType.小型電探:
					case SlotItemType.大型電探:
					case SlotItemType.大型電探_II:
						return Math.Sqrt(slotItem.Level);

					default:
						return .0;
				}
			}
		}

		private class EmptyCalculator: ImprovementBonusCalculator
		{
			public static EmptyCalculator Instance { get; } = new EmptyCalculator();
			
			public override double GetImprovementBonus(SlotItem slotItem) => .0;

			private EmptyCalculator() { }
		}
	}
}
