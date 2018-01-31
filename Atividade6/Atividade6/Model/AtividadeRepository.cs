using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace Atividade6.Model
{
    public class Atividade
    {
        //public int Id { get; set; }
        public DateTime dtCadastro { get; set; }
        public DateTime dtEntrega { get; set; }
        public string tipoAvaliacao { get; set; }
        public string descricao { get; set; }
        public int valor { get; set; }
    }

    public class AtividadeRepository
    {
        public static async Task<ObservableCollection<Atividade>> GetAtividadesSqlAzureAsync()
        {
            var httpRequest = new HttpClient();
            var stream = await httpRequest.GetStreamAsync(
                "http://fiapcadastros.azurewebsites.net/tables/cadas?ZUMO-API-VERSION=2.0.0");

            var atividadeSerializer = new DataContractJsonSerializer(typeof(List<Atividade>));

            StreamReader srStream = new StreamReader(stream);

            string result = null;

            while (!srStream.EndOfStream)
            {
                result = srStream.ReadLine();
            }

            List<Atividade> lstAtividades = new List<Atividade>();
            var a = JsonConvert.DeserializeObject<List<Atividade>>(result);

            return new ObservableCollection<Atividade>(a);
        }

        public static async Task<bool> PostAtividadeSqlAzureAsync(Atividade ativAdd)
        {
            if (ativAdd == null) return false;

            var httpRequest = new HttpClient();
            httpRequest.BaseAddress = new Uri("http://fiapcadastros.azurewebsites.net/");
            httpRequest.DefaultRequestHeaders.Accept.Clear();
            httpRequest.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            string ativjson = Newtonsoft.Json.JsonConvert.SerializeObject(ativAdd);
            var response = await httpRequest.PostAsync("tables/cadas",
                new StringContent(ativjson, System.Text.Encoding.UTF8, "application/json"));

            if (response.IsSuccessStatusCode) return true;

            return false;
        }

        public static async Task<bool> DeleteAtividadeSqlAzureAsync(string ativId)
        {
            if (string.IsNullOrWhiteSpace(ativId)) return false;

            var httpRequest = new HttpClient();
            httpRequest.BaseAddress = new Uri("http://fiapcadastros.azurewebsites.net/");
            httpRequest.DefaultRequestHeaders.Accept.Clear();
            httpRequest.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            var response = await httpRequest.DeleteAsync(string.Format("tables/cadas/descricao={0}", ativId));

            if (response.IsSuccessStatusCode) return true;

            return false;
        }
    }
}
