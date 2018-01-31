using Atividade6.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Atividade6.Model
{
    public class LoginRepository
    {
        public List<UserViewModel> GetUsers()
        {
            var lstUsers = new List<UserViewModel>();
            XmlTextReader reader = new XmlTextReader("http://wopek.com.spiraea.arvixe.com/xml/usuarios.xml");
            XmlDocument doc = new XmlDocument();
            doc.Load(reader);
            ArrayList list = new ArrayList();
            XmlNode idNodes = doc.SelectSingleNode("usuarios");

            UserViewModel user = new UserViewModel();
            foreach (XmlNode node in idNodes.ChildNodes)
            {
                foreach (XmlNode item in node.ChildNodes)
                {
                    if (item.Name == "nome")
                        user.nome = item.InnerText;
                    else if (item.Name == "username")
                        user.username = item.InnerText;
                    else if (item.Name == "password")
                        user.password = item.InnerText;

                    if (user.nome != null && user.username != null && user.password != null)
                    {
                        lstUsers.Add(user);
                        user = new UserViewModel();
                    }
                }
            }
            return lstUsers;
        }
    }
}
