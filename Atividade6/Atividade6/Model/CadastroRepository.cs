using Atividade6.ViewModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace Atividade6.Model
{
    public class Cadastro
    {
        public int id { get; set; }
        public DateTime dtCadastro { get; set; }
        public DateTime dtEntrega { get; set; }
        public enum tipoAvaliacao { parcial = 1, substitutiva = 2 }
        public string descricao { get; set; }
        public int valor { get; set; }
    }


    public class CadastroRepository
    {
        public IEnumerable<CadastroViewModel> cadastrosSqlAzure;

        public List<CadastroViewModel> GetCadastroSqlAzure()
        {
            var json = new WebClient().DownloadString("http://fiapcadastros.azurewebsites.net/tables/cadas?ZUMO-API-VERSION=2.0.0");
            var result = JsonConvert.DeserializeObject<List<CadastroViewModel>>(json);

            //var httpRequest = new HttpClient();
            //var stream = await httpRequest.GetStreamAsync(
            //    "http://fiapcadastros.azurewebsites.net/tables/cadas?ZUMO-API-VERSION=2.0.0");

            //var cadastroSerializer = new DataContractJsonSerializer(typeof(List<CadastroViewModel>));
            //cadastrosSqlAzure = (List<CadastroViewModel>)cadastroSerializer.ReadObject(stream);

            return result;
        }

        public async Task<bool> PostCadastroSqlAzureAsync(CadastroViewModel cadAdd)
        {
            if (cadAdd == null) return false;

            var httpRequest = new HttpClient();
            httpRequest.BaseAddress = new Uri("http://fiapcadastros.azurewebsites.net/");
            httpRequest.DefaultRequestHeaders.Accept.Clear();
            httpRequest.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            string cadJson = Newtonsoft.Json.JsonConvert.SerializeObject(cadAdd);
            var response = await httpRequest.PostAsync("tables/cadas",
                new StringContent(cadJson, System.Text.Encoding.UTF8, "application/json"));

            if (response.IsSuccessStatusCode) return true;

            return false;
        }

        public  async Task<bool> DeleteCadastroSqlAzureAsync(string cadId)
        {
            if (string.IsNullOrWhiteSpace(cadId)) return false;

            var httpRequest = new HttpClient();
            httpRequest.BaseAddress = new Uri("http://fiapcadastros.azurewebsites.net/");
            httpRequest.DefaultRequestHeaders.Accept.Clear();
            httpRequest.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            var response = await httpRequest.DeleteAsync(string.Format("tables/cadas/{0}", cadId));

            if (response.IsSuccessStatusCode) return true;

            return false;
        }
    }
}
