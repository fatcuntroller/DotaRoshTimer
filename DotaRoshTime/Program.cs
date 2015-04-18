using System;
using System.Windows.Forms;

namespace DotaRoshTime
{
	class MainClass
	{
		[STAThread]
		public static void Main(){
			Application.Run (new DotaTrayApp());
		}
	}
}
