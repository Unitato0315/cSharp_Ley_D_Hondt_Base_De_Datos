using ProyectoVotosBaseDartos.Models;
using ProyectoVotosBaseDartos.DB;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;

namespace ProyectoVotosBaseDartos.ViewModel
{
    public class politicPartyModelView : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private const String cnstr = "server=localhost;uid = JoseVi;pwd=admin;database=parties";

        private ObservableCollection<politicParty> _politicParties;
        private String _name = "";
        private String _acromyn = "";
        private String _president = "";

        public ObservableCollection<politicParty> politicParties
        {
            get { return _politicParties; }
            set
            {
                _politicParties = value;
                OnPropertyChange("users");
            }
        }

        public String name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChange("name");
            }
        }

        public String acromyn
        {
            get { return _acromyn; }
            set
            {
                _acromyn = value;
                OnPropertyChange("acromyn");
            }
        }
        public String president
        {
            get { return _president; }
            set
            {
                _president = value;
                OnPropertyChange("president");
            }
        }

        private void OnPropertyChange(string propertyName)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public void NewParty()
        {
            String SQL = $"INSERT INTO politicparties (acrom, name, president) VALUES ('{acromyn}','{name}','{president}')";
            MySQLDataManagement.ExecuteNonQuery(SQL, cnstr);
        }
        public void LoadParty()
        {
            if (politicParties == null) _politicParties = new ObservableCollection<politicParty>();
            String SQL = $"SELECT acrom, name, president FROM politicparties;";
            DataTable dt = MySQLDataManagement.LoadData(SQL, cnstr);
            if (dt.Rows.Count > 0)
            {
                if (politicParties == null) _politicParties = new ObservableCollection<politicParty>();
                foreach (DataRow i in dt.Rows)
                {
                    politicParties.Add(new politicParty
                    {
                        acromyn = i[0].ToString(),
                        name = i[1].ToString(),
                        president = i[2].ToString()
                    });
                }
            }
            dt.Dispose();
        }

        public void DeleteParty(politicParty party) {
            String SQL = $"DELETE FROM politicparties WHERE acrom = '{party.acromyn}';";
            MySQLDataManagement.ExecuteNonQuery(SQL, cnstr);
            politicParties.Remove(party);
        }
    }
}
