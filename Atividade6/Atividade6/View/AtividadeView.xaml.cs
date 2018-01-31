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
	public partial class AtividadeView : ContentPage
	{
		public AtividadeView ()
		{
            BindingContext = AtividadeViewModel.Instancia;
            InitializeComponent ();
		}

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            lstAtividades.IsRefreshing = !lstAtividades.IsRefreshing;
            await AtividadeViewModel.Instancia.Carregar();
            lstAtividades.IsRefreshing = !lstAtividades.IsRefreshing;
        }
    }
}