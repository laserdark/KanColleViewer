using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace Grabacr07.KanColleWrapper.Models
{
	public class FleetCondition : TimerNotifier
	{
		private Ship[] ships;
		private bool notificated;
		private int minCondition;

		/// <summary>
		/// 通知機能が有効かどうかを示す値を取得または設定します。
		/// </summary>
		public bool IsEnabled { get; set; }

		public string Name { get; set; }

		#region RejuvenateTime / IsRejuvenating 変更通知プロパティ

		private DateTimeOffset? _RejuvenateTime;

		/// <summary>
		/// 疲労回復の目安時間を取得します。
		/// </summary>
		public DateTimeOffset? RejuvenateTime
		{
			get { return this._RejuvenateTime; }
			private set
			{
				if (this._RejuvenateTime != value)
				{
					this._RejuvenateTime = value;
					this.notificated = false;
					this.RaisePropertyChanged();
					this.RaisePropertyChanged(nameof(this.IsRejuvenating));
				}
			}
		}

		/// <summary>
		/// 艦隊に編成されている艦娘の疲労を自然回復しているかどうかを示す値を取得します。
		/// </summary>
		public bool IsRejuvenating => this.RejuvenateTime.HasValue;

		#endregion

		#region Remaining 変更通知プロパティ

		private TimeSpan? _Remaining;

		/// <summary>
		/// 疲労の回復が完了するまでの残り時間を取得します。1 秒ごとに更新されます。
		/// </summary>
		public TimeSpan? Remaining
		{
			get { return this._Remaining; }
			private set
			{
				if (this._Remaining != value)
				{
					this._Remaining = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		public event EventHandler<ConditionRejuvenatedEventArgs> Rejuvenated;

		internal void Update(Ship[] s)
		{			
			if (s.Length == 0)
			{				
				this.RejuvenateTime = null;
				this.ships = s;
				return;
			}

			var isSubset = false;
			if (this.ships != null)
			{				
				isSubset = s.All(x => this.ships.Any(y => x.Id == y.Id));
			}

			var condTmp = this.ships?.Min(x => x?.Condition) ?? 0;
			var condition = s.Min(x => x.Condition);
			if ((condition > this.minCondition) && (isSubset || (condition == condTmp)))
			{
				var rejuvnate = GetRejuvnate(condition);

				if (this.RejuvenateTime <= rejuvnate)
				{
					if (this.RejuvenateTime <= DateTimeOffset.Now)
						this.RejuvenateTime = null;
				}
				else
					this.RejuvenateTime = rejuvnate <= DateTimeOffset.Now
						? (DateTimeOffset?)null
						: rejuvnate;
			}
			else if (!((condition == this.minCondition) && isSubset))
			{
				var rejuvnate = GetRejuvnate(condition);

				this.RejuvenateTime = rejuvnate <= DateTimeOffset.Now
					? (DateTimeOffset?)null
					: rejuvnate;
			}

			this.ships = s;
		}
		private DateTimeOffset GetRejuvnate(int cond)
		{
			this.minCondition = cond;

			var r = DateTimeOffset.Now; // 回復完了予測時刻

			while (cond < Math.Min(49, KanColleClient.Current.Settings.ReSortieCondition))
			{
				r = r.AddMinutes(3);
				cond += 3;
				if (cond > 49) cond = 49;
			}

			return r;
		}

		protected override void Tick()
		{
			base.Tick();

			if (this.RejuvenateTime.HasValue && this.IsEnabled)
			{
				var remaining = this.RejuvenateTime.Value.Subtract(DateTimeOffset.Now);
				if (remaining.Ticks < 0) remaining = TimeSpan.Zero;

				this.Remaining = remaining;

				if (!this.notificated && this.Rejuvenated != null && remaining.Ticks <= 0)
				{
					this.Rejuvenated(this, new ConditionRejuvenatedEventArgs(this.Name, 0));
					this.notificated = true;
				}
			}
			else
			{
				this.Remaining = null;
			}
		}
	}
}
