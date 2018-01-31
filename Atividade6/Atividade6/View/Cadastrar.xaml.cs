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
	public partial class Cadastrar : ContentPage
	{
		public Cadastrar ()
		{
			InitializeComponent ();
		}

        public void btnCadastrar_Clicked()
        {
            var cad = new CadastroViewModel
            {
                dtCadastro = Convert.ToDateTime(dtCadastro.Text),
                dtEntrega = Convert.ToDateTime(dtEntrega.Text),
                tipoAvaliacao = tipoAvaliacao.Text,
                descricao = descricao.Text,
                valor = Convert.ToInt32(valor.Text)
            };

            var result = new CadastroRepository().PostCadastroSqlAzureAsync(cad);



        }


    }
}