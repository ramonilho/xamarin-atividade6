using Atividade6.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Atividade6.ViewModel
{
    public class AtividadeViewModel : INotifyPropertyChanged
    {
        #region Propriedades
        static AtividadeViewModel instancia = new AtividadeViewModel();
        public static AtividadeViewModel Instancia
        {
            get { return instancia; }
            private set { instancia = value; }
        }

        public Atividade AtividadeModel { get; set; }

        private Atividade selecionado;
        public Atividade Selecionado
        {
            get { return selecionado; }
            set
            {
                selecionado = value as Atividade;
                EventPropertyChanged();
            }
        }

        private string pesquisaPorNome;
        public string PesquisaPorNome
        {
            get { return pesquisaPorNome; }
            set
            {
                if (value == pesquisaPorNome) return;

                pesquisaPorNome = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PesquisaPorNome)));
                AplicarFiltro();
            }
        }

        public List<Atividade> CopiaListaAtividades;
        public ObservableCollection<Atividade> Atividades { get; set; } = new ObservableCollection<Atividade>();

        // UI Events
        public OnAdicionarAtividadeCMD OnAdicionarAtividadeCMD { get; }
        public OnEditarAtividadeCMD OnEditarAtividadeCMD { get; }
        public OnDeleteAtividadeCMD OnDeleteAtividadeCMD { get; }
        public ICommand OnSairCMD { get; private set; }
        public ICommand OnNovoCMD { get; private set; }

        #endregion

        public AtividadeViewModel()
        {
            OnAdicionarAtividadeCMD = new OnAdicionarAtividadeCMD(this);
            OnEditarAtividadeCMD = new OnEditarAtividadeCMD(this);
            OnDeleteAtividadeCMD = new OnDeleteAtividadeCMD(this);
            OnSairCMD = new Command(OnSair);
            OnNovoCMD = new Command(OnNovo);

            CopiaListaAtividades = new List<Atividade>();
        }

        public async Task Carregar()
        {
            await AtividadeRepository.GetAtividadesSqlAzureAsync().ContinueWith(retorno =>
            {
                CopiaListaAtividades = retorno.Result.ToList();
            });
            AplicarFiltro();
        }

        public void AplicarFiltro()
        {
            if (pesquisaPorNome == null)
                pesquisaPorNome = "";

            var resultado = CopiaListaAtividades.Where(n => n.descricao.ToLowerInvariant()
                                .Contains(PesquisaPorNome.ToLowerInvariant().Trim())).ToList();

            var removerDaLista = Atividades.Except(resultado).ToList();
            foreach (var item in removerDaLista)
            {
                Atividades.Remove(item);
            }

            for (int index = 0; index < resultado.Count; index++)
            {
                var item = resultado[index];
                if (index + 1 > Atividades.Count || !Atividades[index].Equals(item))
                    Atividades.Insert(index, item);
            }
        }

        public async void Adicionar(Atividade paramAtividade)
        {
          

            if ((paramAtividade == null) || (string.IsNullOrWhiteSpace(paramAtividade.descricao)))
                await App.Current.MainPage.DisplayAlert("Atenção", "O campo nome é obrigatório", "OK");
            else if (await AtividadeRepository.PostAtividadeSqlAzureAsync(paramAtividade))
                await App.Current.MainPage.Navigation.PopAsync();
            else
                await App.Current.MainPage.DisplayAlert("Falhou", "Desculpe, ocorreu um erro inesperado =(", "OK");
        }

        public async void Editar()
        {
            await App.Current.MainPage.Navigation.PushAsync(
                new View.AtividadeView() { BindingContext = Instancia });
        }

        public async void Remover()
        {
            if (await App.Current.MainPage.DisplayAlert("Atenção?",
                string.Format("Tem certeza que deseja remover o {0}?", Selecionado.descricao), "Sim", "Não"))
            {
                if (await AtividadeRepository.DeleteAtividadeSqlAzureAsync(Selecionado.descricao.ToString()))
                {
                    CopiaListaAtividades.Remove(Selecionado);
                    await Carregar();
                }
                else
                    await App.Current.MainPage.DisplayAlert(
                            "Falhou", "Desculpe, ocorreu um erro inesperado =(", "OK");
            }
        }

        private void OnNovo()
        {
            Instancia.Selecionado = new Model.Atividade();
            App.Current.MainPage.Navigation.PushAsync(
                new View.NovaAtividadeView() { BindingContext = Instancia });
        }

        private async void OnSair()
        {
            await App.Current.MainPage.Navigation.PopAsync();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void EventPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    public class OnAdicionarAtividadeCMD : ICommand
    {
        private AtividadeViewModel atividadeVM;
        public OnAdicionarAtividadeCMD(AtividadeViewModel paramVM)
        {
            atividadeVM = paramVM;
        }
        public event EventHandler CanExecuteChanged;
        public void AdicionarCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        public bool CanExecute(object parameter) => true;
        public void Execute(object parameter)
        {
            atividadeVM.Adicionar(parameter as Atividade);
        }
    }

    public class OnEditarAtividadeCMD : ICommand
    {
        private AtividadeViewModel atividadeVM;
        public OnEditarAtividadeCMD(AtividadeViewModel paramVM)
        {
            atividadeVM = paramVM;
        }
        public event EventHandler CanExecuteChanged;
        public void EditarCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        public bool CanExecute(object parameter) => (parameter != null);
        public void Execute(object parameter)
        {
            AtividadeViewModel.Instancia.Selecionado = parameter as Atividade;
            atividadeVM.Editar();
        }
    }

    public class OnDeleteAtividadeCMD : ICommand
    {
        private AtividadeViewModel atividadeVM;
        public OnDeleteAtividadeCMD(AtividadeViewModel paramVM)
        {
            atividadeVM = paramVM;
        }
        public event EventHandler CanExecuteChanged;
        public void DeleteCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        public bool CanExecute(object parameter) => (parameter != null);
        public void Execute(object parameter)
        {
            AtividadeViewModel.Instancia.Selecionado = parameter as Atividade;
            atividadeVM.Remover();
        }
    }
}

