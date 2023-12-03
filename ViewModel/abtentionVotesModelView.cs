using ProyectoVotosBaseDartos.DB;
using ProyectoVotosBaseDartos.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml.Linq;

namespace ProyectoVotosBaseDartos.ViewModel
{
    internal class abtentionVotesModelView
    {
        private const String cnstr = "server=localhost;uid = JoseVi;pwd=admin;database=parties";
        private int _votes = 0;
        private int _id = 0;
        public int totalVotes 
        {
            get {
                return _votes;
            }
            set {
                _votes = value;
            }
        }

        public void NewAbstention()
        {
            String SQL = $"INSERT INTO abstentionvotes (abstention) VALUES ('{totalVotes}')";
            MySQLDataManagement.ExecuteNonQuery(SQL, cnstr);
            LoadAbstention();
        }
        public void LoadAbstention()
        {
            String SQL = $"SELECT * FROM abstentionvotes;";
            DataTable dt = MySQLDataManagement.LoadData(SQL, cnstr);
            if (dt.Rows.Count == 1)
            {
                var aux = dt.Rows[0];
                _id = int.Parse(aux[0].ToString());
                totalVotes = int.Parse(aux[1].ToString());
            }
            else {
                NewAbstention();
            }
            dt.Dispose();
        }

        public void UpdateAbstention() {
            String SQL = $"UPDATE abstentionvotes SET abstention = '{totalVotes}' WHERE id = '{_id}';";
            MySQLDataManagement.ExecuteNonQuery(SQL, cnstr);
        }
    }
}
