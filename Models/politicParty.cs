using System;
using System.ComponentModel;

namespace ProyectoVotosBaseDartos.Models
{
    public class politicParty : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private String _name = "";
        private String _acromyn = "";
        private String _president = "";
        private int _votes = 0;
        private int _seats = 0;

        public String name{
            get { return _name; }
            set { 
                _name = value;
                OnPropertyChange("name");
            }
        }

        public int votes {
            get { return _votes; }
            set
            {
                _votes = value;
            }
        }
        public int seats
        {
            get { return _seats; }
            set
            {
                _seats = value;
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
    }
}
