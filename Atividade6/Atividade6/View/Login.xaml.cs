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
	public partial class Login : ContentPage
	{
        public List<UserViewModel> lstUsers = new List<UserViewModel>();

		public Login ()
		{
            lstUsers = new LoginRepository().GetUsers();

            InitializeComponent ();
		}

        public async void OnLoginButtonClicked()
        {
            int totUser = lstUsers.Where(w => w.username == usernameEntry.Text && w.password == passwordEntry.Text).Count();

            if (totUser == 1)
            {
                await Navigation.PushAsync(new AtividadeView());
            }
            else
            {
                //Falha ao entrar
            }
        }

        public async void btnLogar_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new LoginFacebook());
        }

    }
}