using System;
using System.Collections.Generic;
using System.Text;

namespace Atividade6.ViewModel
{

    public class CadastroViewModel
    {
        public int id { get; set; }
        public DateTime dtCadastro { get; set; }
        public DateTime dtEntrega { get; set; }
        public string tipoAvaliacao { get; set; }
        public string descricao { get; set; }
        public int valor { get; set; }
    }
}
