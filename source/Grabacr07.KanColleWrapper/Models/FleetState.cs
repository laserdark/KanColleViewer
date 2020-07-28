using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StatefulModel.EventListeners.WeakEvents;

namespace Grabacr07.KanColleWrapper.Models
{
	/// <summary>
	/// 艦隊の状態を表します。
	/// </summary>
	public class FleetState : DisposableNotifier
	{
		private readonly Homeport homeport;
		private readonly Fleet[] source;

		/// <summary>
		/// 艦隊に編成されている艦娘のコンディションを取得します。
		/// </summary>
		public FleetCondition Condition { get; }

		public RepairShipRepairingDuration RepairingDuration { get; }

		#region AverageLevel 変更通知プロパティ

		private double _AverageLevel;

		/// <summary>
		/// 艦隊の平均レベルを取得します。
		/// </summary>
		public double AverageLevel
		{
			get { return this._AverageLevel; }
			private set
			{
				if (!this._AverageLevel.Equals(value))
				{
					this._AverageLevel = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion
		
		#region TotalLevel 変更通知プロパティ

		private int _TotalLevel;

		public int TotalLevel
		{
			get { return this._TotalLevel; }
			private set
			{
				if (this._TotalLevel != value)
				{
					this._TotalLevel = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		private int _TotalFirepower;

		public int TotalFirepower
		{
			get { return this._TotalFirepower; }
			private set
			{
				if (this._TotalFirepower != value)
				{
					this._TotalFirepower = value;
					RaisePropertyChanged();
				}
			}
		}

		private int _ImprovementFirepower;

		public int ImprovementFirepower
		{
			get { return this._ImprovementFirepower; }
			private set
			{
				if (this._ImprovementFirepower != value)
				{
					this._ImprovementFirepower = value;
					RaisePropertyChanged();
				}
			}
		}

		private int _TotalAA;

		public int TotalAA
		{
			get { return this._TotalAA; }
			private set
			{
				if (this._TotalAA != value)
				{
					this._TotalAA = value;
					RaisePropertyChanged();
				}
			}
		}

		private int _ImprovementAA;

		public int ImprovementAA
		{
			get { return this._ImprovementAA; }
			private set
			{
				if (this._ImprovementAA != value)
				{
					this._ImprovementAA = value;
					RaisePropertyChanged();
				}
			}
		}

		private int _TotalASW;

		public int TotalASW
		{
			get { return this._TotalASW; }
			private set
			{
				if (this._TotalASW != value)
				{
					this._TotalASW = value;
					RaisePropertyChanged();
				}
			}
		}

		private int _ImprovementASW;

		public int ImprovementASW
		{
			get { return this._ImprovementASW; }
			private set
			{
				if (this._ImprovementASW != value)
				{
					this._ImprovementASW = value;
					RaisePropertyChanged();
				}
			}
		}

		private int _TotalLoS;

		public int TotalLoS
		{
			get { return this._TotalLoS; }
			private set
			{
				if (this._TotalLoS != value)
				{
					this._TotalLoS = value;
					RaisePropertyChanged();
				}
			}
		}

		private int _ImprovementLoS;

		public int ImprovementLoS
		{
			get { return this._ImprovementLoS; }
			private set
			{
				if (this._ImprovementLoS != value)
				{
					this._ImprovementLoS = value;
					RaisePropertyChanged();
				}
			}
		}

		#region MinAirSuperiorityPotential 変更通知プロパティ

		private double _MinAirSuperiorityPotential;

		/// <summary>
		/// 艦隊の制空能力を取得します。
		/// </summary>
		public double MinAirSuperiorityPotential
		{
			get { return this._MinAirSuperiorityPotential; }
			private set
			{
				if (this._MinAirSuperiorityPotential != value)
				{
					this._MinAirSuperiorityPotential = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region MaxAirSuperiorityPotential 変更通知プロパティ

		private double _MaxAirSuperiorityPotential;

		/// <summary>
		/// 艦隊の制空能力を取得します。
		/// </summary>
		public double MaxAirSuperiorityPotential
		{
			get { return this._MaxAirSuperiorityPotential; }
			private set
			{
				if (this._MaxAirSuperiorityPotential != value)
				{
					this._MaxAirSuperiorityPotential = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		private double _SecondFleetMinAirSuperiorityPotential;

		public double SecondFleetMinAirSuperiorityPotential
		{
			get { return this._SecondFleetMinAirSuperiorityPotential; }
			private set
			{
				if (this._SecondFleetMinAirSuperiorityPotential != value)
				{
					this._SecondFleetMinAirSuperiorityPotential = value;
					this.RaisePropertyChanged();
				}
			}
		}

		private double _SecondFleetMaxAirSuperiorityPotential;

		public double SecondFleetMaxAirSuperiorityPotential
		{
			get { return this._SecondFleetMaxAirSuperiorityPotential; }
			private set
			{
				if (this._SecondFleetMaxAirSuperiorityPotential != value)
				{
					this._SecondFleetMaxAirSuperiorityPotential = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#region ViewRange 変更通知プロパティ

		private double _ViewRange;

		/// <summary>
		/// 艦隊の索敵値を取得します。索敵の計算は <see cref="IKanColleClientSettings.ViewRangeCalcType"/> で指定された方法を使用します。
		/// </summary>
		public double ViewRange
		{
			get { return this._ViewRange; }
			private set
			{
				if (this._ViewRange != value)
				{
					this._ViewRange = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region ViewRangeCalcType 変更通知プロパティ

		private string _ViewRangeCalcType;

		public string ViewRangeCalcType
		{
			get { return this._ViewRangeCalcType; }
			set
			{
				if (this._ViewRangeCalcType != value)
				{
					this._ViewRangeCalcType = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region Speed 変更通知プロパティ

		private FleetSpeed _Speed;

		public FleetSpeed Speed
		{
			get { return this._Speed; }
			set
			{
				if (this._Speed != value)
				{
					this._Speed = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region Situation 変更通知プロパティ

		private FleetSituation _Situation;

		public FleetSituation Situation
		{
			get { return this._Situation; }
			private set
			{
				if (this._Situation != value)
				{
					this._Situation = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		#region IsReady 変更通知プロパティ

		private bool _IsReady;

		/// <summary>
		/// 艦隊の出撃準備ができているかどうかを示す値を取得します。
		/// </summary>
		public bool IsReady
		{
			get { return this._IsReady; }
			private set
			{
				if (this._IsReady != value)
				{
					this._IsReady = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		public event EventHandler Updated;
		public event EventHandler Calculated;


		public FleetState(Homeport homeport, params Fleet[] fleets)
		{
			this.homeport = homeport;
			this.source = fleets ?? new Fleet[0];

			this.Condition = new FleetCondition();
			this.CompositeDisposable.Add(this.Condition);
			this.RepairingDuration = new RepairShipRepairingDuration();
			this.CompositeDisposable.Add(this.RepairingDuration);
			this.CompositeDisposable.Add(new PropertyChangedWeakEventListener(KanColleClient.Current.Settings)
			{
				{ nameof(IKanColleClientSettings.ViewRangeCalcType), (_, __) => this.Calculate() },
				{ nameof(IKanColleClientSettings.ViewRangeCalcNodeFactor), (_,__) => this.Calculate() },
				{ nameof(IKanColleClientSettings.IsViewRangeCalcIncludeFirstFleet), (_, __) => this.Calculate() },
				{ nameof(IKanColleClientSettings.IsViewRangeCalcIncludeSecondFleet), (_, __) => this.Calculate() },
			});
		}


		/// <summary>
		/// 艦隊の平均レベルや制空戦力などの各種数値を再計算します。
		/// </summary>
		public void Calculate()
		{
			var ships = this.source.SelectMany(x => x.Ships).WithoutEvacuated().ToArray();
			var firstFleetShips = this.source.FirstOrDefault()?.Ships.WithoutEvacuated().ToArray() ?? new Ship[0];
			var secondFleetShips = (this.source.Count() > 1) ? this.source.Last()?.Ships.WithoutEvacuated().ToArray() : new Ship[0];

			this.TotalLevel = ships.HasItems() ? ships.Sum(x => x.Level) : 0;
			this.AverageLevel = ships.HasItems() ? (double)this.TotalLevel / ships.Length : 0.0;
			this.ImprovementFirepower = ships.HasItems() ? (int)ships.GetImprovementBonus(ImprovementBonusCalculationOptions.Firepower) : 0;
			this.TotalFirepower = ships.HasItems() ? ships.Sum(x => x.Karyoku) + this.ImprovementFirepower : 0;
			this.ImprovementAA = ships.HasItems() ? (int)ships.GetImprovementBonus(ImprovementBonusCalculationOptions.AA) : 0;
			this.TotalAA = ships.HasItems() ? ships.Sum(x => x.Taiku) + this.ImprovementAA : 0;
			this.ImprovementASW = ships.HasItems() ? (int)ships.GetImprovementBonus(ImprovementBonusCalculationOptions.ASW) : 0;
			this.TotalASW = ships.HasItems() ? ships.Sum(x => x.ASW) + this.ImprovementASW : 0;
			this.ImprovementLoS = ships.HasItems() ? (int)ships.GetImprovementBonus(ImprovementBonusCalculationOptions.LoS) : 0;
			this.TotalLoS = (int)ViewRangeCalcLogic.Get("KanColleViewer.ViewRangeType1").Calc(this.source) + this.ImprovementLoS;

			this.MinAirSuperiorityPotential = firstFleetShips.Sum(x => x.GetAirSuperiorityPotential(AirSuperiorityCalculationOptions.Minimum));
			this.MaxAirSuperiorityPotential = firstFleetShips.Sum(x => x.GetAirSuperiorityPotential(AirSuperiorityCalculationOptions.Maximum));
			this.SecondFleetMinAirSuperiorityPotential = secondFleetShips.Sum(x => x.GetAirSuperiorityPotential(AirSuperiorityCalculationOptions.Minimum));
			this.SecondFleetMaxAirSuperiorityPotential = secondFleetShips.Sum(x => x.GetAirSuperiorityPotential(AirSuperiorityCalculationOptions.Maximum));
			this.Speed = new FleetSpeed(Array.ConvertAll(ships, x => x.Speed));

			var logic = ViewRangeCalcLogic.Get(KanColleClient.Current.Settings.ViewRangeCalcType);
			this.ViewRange = logic.Calc(this.source);
			this.ViewRangeCalcType = logic.Name;
			
			this.Calculated?.Invoke(this, new EventArgs());
		}

		public void Update()
		{
			var state = FleetSituation.Empty;
			var ready = true;

			var ships = this.source.SelectMany(x => x.Ships).ToArray();
			if (ships.Length == 0)
			{
				ready = false;
			}
			else
			{
				var first = this.source[0];

				if (this.source.Length == 1)
				{
					if (first.IsInSortie)
					{
						state |= FleetSituation.Sortie;
						ready = false;
					}
					else if (first.Expedition.IsInExecution)
					{
						state |= FleetSituation.Expedition;
						ready = false;
					}
					else
					{
						state |= FleetSituation.Homeport;
					}
				}
				else
				{
					state |= FleetSituation.Combined;

					if (first.IsInSortie)
					{
						state |= FleetSituation.Sortie;
						ready = false;
					}
					else
					{
						state |= FleetSituation.Homeport;
					}
				}
			}

			this.Condition.Update(ships);
			this.Condition.IsEnabled = state.HasFlag(FleetSituation.Homeport); // 疲労回復通知は母港待機中の艦隊でのみ行う

			if (state.HasFlag(FleetSituation.Homeport))
			{
				var repairing = ships.Any(x => this.homeport.Repairyard.CheckRepairing(x.Id));
				if (repairing)
				{
					state |= FleetSituation.Repairing;
					ready = false;
				}

				var inShortSupply = ships.Any(s => s.Fuel.Current < s.Fuel.Maximum || s.Bull.Current < s.Bull.Maximum);
				if (inShortSupply)
				{
					state |= FleetSituation.InShortSupply;
					ready = false;
				}

				if (this.Condition.IsRejuvenating)
				{
					ready = false;
				}
			}

			var heavilyDamaged = ships
				.Where(s => !this.homeport.Repairyard.CheckRepairing(s.Id))
				.Where(s => !s.Situation.HasFlag(ShipSituation.Evacuation) && !s.Situation.HasFlag(ShipSituation.Tow))
				.Where(s => !(state.HasFlag(FleetSituation.Sortie) && s.Situation.HasFlag(ShipSituation.DamageControlled)))
				.Any(s => s.HP.IsHeavilyDamage());
			if (heavilyDamaged)
			{
				state |= FleetSituation.HeavilyDamaged;
				ready = false;
			}

			var flagshipIsRepairShip = this.source
				.Where(x => x.Ships.Length >= 1)
				.Select(x => x.Ships[0])
				.Any(x => x.Info.ShipType.Id == 19);
			if (flagshipIsRepairShip)
			{
				state |= FleetSituation.FlagshipIsRepairShip;
				this.RepairingDuration.Update(ships);
				if (KanColleClient.Current.Settings.CheckFlagshipIsRepairShip) ready = false;
			}

			this.Situation = state;
			this.IsReady = ready;

			this.Updated?.Invoke(this, new EventArgs());
		}
	}
}
