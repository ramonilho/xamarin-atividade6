using Atividade6.Model;
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
	public partial class home : ContentPage
	{
        //List<CadastroViewModel> lstCadastros = new List<CadastroViewModel>();

        public home ()
		{
            lstCadastros.ItemsSource = new CadastroRepository().GetCadastroSqlAzure();
            InitializeComponent ();
		}

        public void btnAdd()
        {

        }

    }
}