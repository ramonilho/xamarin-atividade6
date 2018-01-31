using Atividade6.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Atividade6.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NovaAtividadeView : ContentPage
	{
		public NovaAtividadeView ()
		{
			InitializeComponent ();
		}

        //public void teste()
        //{
        //    new OnAdicionarAtividadeCMD().Execute();
        //}
	}
}