using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace Grabacr07.KanColleWrapper.Models
{
	public class RepairShipRepairingDuration: TimerNotifier
	{
		private Ship[] ships;

		private DateTimeOffset? _StartTime;

		public DateTimeOffset? StartTime
		{
			get { return this._StartTime; }
			private set
			{
				if (this._StartTime != value)
				{
					this._StartTime = value;
					this.RaisePropertyChanged();
				}
			}
		}

		private TimeSpan? _RepairingDuration;

		public TimeSpan? RepairingDuration
		{
			get { return this._RepairingDuration; }
			private set
			{
				if (this._RepairingDuration != value)
				{
					this._RepairingDuration = value;
					this.RaisePropertyChanged();
				}
			}
		}

		internal void Update(Ship[] s)
		{
			var isOnlyEquipChanged = false;
			if (this.ships != null) isOnlyEquipChanged = Enumerable.SequenceEqual(s.Select(x => x.Id), this.ships.Select(y => y.Id));
			this.ships = s;
			if (isOnlyEquipChanged)
			{
				if (this.StartTime.HasValue)
				{
					if (DateTimeOffset.Now.Subtract(this.StartTime.Value) >= TimeSpan.FromMinutes(20))
					{
						this.StartTime = DateTimeOffset.Now;
					}
				}				
				return;
			}

			var isRepairing = s.Take(Math.Min(2 + s[0].EquippedItems.Count(x => x.Item.Info.EquipType.Id == 31),
											  s.Length))
							   .Any(x =>
							   {
								   var percentage = x.HP.Maximum == 0 ? 0.0 : x.HP.Current / (double)x.HP.Maximum;
								   return (0.5 < percentage) && (percentage < 1.0);
							   });
			var isEnabled = (s[0].HP.Current / (double)s[0].HP.Maximum) > 0.5;

			if (isRepairing && isEnabled)
			{
				this.StartTime = DateTimeOffset.Now;
			}
			else
			{
				this.StartTime = null;
			}
		}

		protected override void Tick()
		{
			base.Tick();

			if (this.StartTime.HasValue)
			{
				var duration = DateTimeOffset.Now.Subtract(this.StartTime.Value);

				this.RepairingDuration = duration;
			}
			else
			{
				this.RepairingDuration = null;
			}
		}
	}
}
