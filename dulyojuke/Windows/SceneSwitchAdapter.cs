using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace dulyojuke.Windows
{
	class SceneSwitchAdapter
	{
		private RoutedEventHandler PreviousButtonClickEvent { get; set; }
		private RoutedEventHandler NextButtonClickEvent { get; set; }
		private RoutedEventHandler SettingsButtonClickEvent { get; set; }
		private RoutedEventHandler HomeButtonClickEvent { get; set; }

		public SceneSwitchAdapter(
			RoutedEventHandler previousButtonClickEvent,
			RoutedEventHandler nextButtonClickEvent,
			RoutedEventHandler settingsButtonClickEvent,
			RoutedEventHandler homeButtonClickEvent )
		{
			this.PreviousButtonClickEvent = previousButtonClickEvent;
			this.NextButtonClickEvent = nextButtonClickEvent;
			this.SettingsButtonClickEvent = settingsButtonClickEvent;
			this.HomeButtonClickEvent = homeButtonClickEvent;
		}

		public void AttachEventHandlers(
			Button previousButton,
			Button nextButton,
			Button settingsButton,
			Button homeButton )
		{
			if ( previousButton != null && this.PreviousButtonClickEvent != null ) previousButton.Click += this.PreviousButtonClickEvent;
			if ( nextButton != null && this.NextButtonClickEvent != null ) nextButton.Click += this.NextButtonClickEvent;
			if ( settingsButton != null && this.SettingsButtonClickEvent != null ) settingsButton.Click += this.SettingsButtonClickEvent;
			if ( homeButton != null && this.HomeButtonClickEvent != null ) homeButton.Click += this.HomeButtonClickEvent;
		}
	}
}
