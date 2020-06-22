using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grabacr07.KanColleWrapper.Models;
using Livet;
using Livet.EventListeners;

namespace Grabacr07.KanColleViewer.ViewModels.Contents.Fleets
{
	public class RepairShipDurationViewModel:ViewModel
	{
		private readonly RepairShipRepairingDuration source;

		public string RepairingDuration => this.source.RepairingDuration.HasValue
			? ": " + $"{(int)this.source.RepairingDuration.Value.TotalHours:D2}:{this.source.RepairingDuration.Value.ToString(@"mm\:ss")}"
			: "";

		public RepairShipDurationViewModel(RepairShipRepairingDuration duration)
		{
			this.source = duration;
			this.CompositeDisposable.Add(new PropertyChangedEventListener(duration, (sender, args) => this.RaisePropertyChanged(args.PropertyName)));
		}
	}
}
