using System;
using System.Windows.Forms;
using Microsoft.Win32;
using System.IO;
using GlobalHotKey;
using System.Windows.Input;

namespace DotaRoshTime
{
	public class DotaTrayApp : ApplicationContext
	{
		//private NotifyIcon TrayIcon;
		//private ContextMenuStrip TrayMenu;
		private Timer RoshTimer;
		private string configPath;
		private RegistryKey SteamKey;
		private HotKeyManager gHotKey;
		private int timeCount;

		public DotaTrayApp ()
		{
			timeCount = 0;
			RoshTimer = new Timer ();
			RoshTimer.Interval = 1000;
			RoshTimer.Stop ();
			RoshTimer.Tick += RoshTimer_Tick;
			gHotKey = new HotKeyManager ();
			gHotKey.KeyPressed += GHotKey_KeyPressed;
			var hotKey = gHotKey.Register (Key.F7, ModifierKeys.None);
			SteamKey = Registry.CurrentUser.OpenSubKey (@"Software\Valve\Steam");
			configPath = SteamKey.GetValue (@"SteamPath").ToString().Replace("/","\\");
			configPath += @"\steamapps\common\dota 2 beta\dota\cfg\roshtimer.cfg";
			CreateTimer (661);
		}

		void RoshTimer_Tick (object sender, EventArgs e)
		{
			CreateTimer (timeCount);
			timeCount++;
			if (timeCount >= 660) {
				RoshTimer.Stop ();
			}
		}

		void GHotKey_KeyPressed (object sender, KeyPressedEventArgs e)
		{
			if (e.HotKey.Key == Key.F7) {
				RoshTimer.Start ();
				timeCount = 0;
			}
		}

		private void CreateTimer(int time){
			FileStream timerCFGStream;
			timerCFGStream = File.Create (configPath);
			string value;
			if (time >= 660) {
				value = @"""Roshan is up!""";
			} else {
				string mins = (time / 60).ToString();
				string secs = (time % 60).ToString("D2");
				value = string.Format ("\"Roshan died {0}:{1} ago!\"", mins, secs); 
			}
			value = @"say_team " + value;  
			using (StreamWriter sw = new StreamWriter (timerCFGStream)) {
				sw.Write (value);
				sw.Close ();
			}
		}

	}
}

