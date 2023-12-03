using ProyectoVotosBaseDartos.Models;
using ProyectoVotosBaseDartos.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProyectoVotosBaseDartos
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int POPULATION = 6921267; // Constante con la poblacion de madrid
        private politicPartyModelView model = new politicPartyModelView();
        private abtentionVotesModelView abtentionVotes = new abtentionVotesModelView();
        public MainWindow()
        {
            InitializeComponent();
            population.Text = POPULATION.ToString();
            DataContext = model;
            model.LoadParty();
            abtentionVotes.LoadAbstention();
            abstention.Text = abtentionVotes.totalVotes.ToString();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (abstention.Text.Length != 0 && nVotes.Text.Length != 0 && !nVotes.Text.Equals("Error") && !nVotes.Text.Equals("Error solo acepta numeros"))
            {
                parties.IsEnabled = true;
                abtentionVotes.totalVotes = int.Parse(abstention.Text);
                abtentionVotes.UpdateAbstention();
                parties.Focus();
                if (model.politicParties.Count == 10)
                {
                    simulation.IsEnabled = true;
                }
            }
            else
            {
                MessageBox.Show("No se han rellenado todos los campos");
            }
        }
        /// <summary>
        /// Se encarga de comprobar cuando se modifica la textBox de abstencion para calcular en tiempo real los votos nulos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int abtentionVotes;
            try
            {
                if (abstention.Text.Length == 0)
                {
                    nVotes.Text = "";

                }
                else
                {
                    abtentionVotes = int.Parse(abstention.Text.Trim());
                    if (abtentionVotes > POPULATION)
                    {
                        nVotes.Text = "Error";
                    }
                    else
                    {
                        nVotes.Text = Calcular(abtentionVotes).ToString();
                    }
                }
            }
            catch (FormatException)
            {
                nVotes.Text = "Error solo acepta numeros";
            }
        }
        /// <summary>
        /// Se encarga de calcular los votos nulos
        /// </summary>
        /// <param name="abstention"></param>
        /// <returns>devuelve el resultado</returns>
        private static int Calcular(int abstention)
        {
            int calculo;

            calculo = (POPULATION - abstention) / 20;

            return calculo;

        }
        /// <summary>
        /// Al seleccionar una opcion del dataGrid de los datos de los partidos activa el boton delete
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvPoliticParty_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            botonDelete.IsEnabled = true;
        }

        /// <summary>
        /// Al pulsar el boton new activa las textBox y los botones de guardar y cancelar 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void botonNew_Click(object sender, RoutedEventArgs e)
        {
            activarDesactivarCreacion(true);
        }
        /// <summary>
        /// Se encarga de eliminar los partidos politicos que han sido seleccionados en el dataGrid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void botonDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgvPoliticParty.SelectedItems != null)
            {
                var delete = dgvPoliticParty.SelectedItems.Cast<politicParty>().ToList();
                foreach (var p in delete) { 
                    model.DeleteParty(p);
                }
                botonDelete.IsEnabled = false;
                if (model.politicParties.Count == 10)
                {
                    simulation.IsEnabled = true;
                }
                else
                {
                    simulation.IsEnabled = false;
                }
            }
        }
        /// <summary>
        /// Al pulsar el boton save guarda el partido politico y refresca el dataGrid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void botonSave_Click(object sender, RoutedEventArgs e)
        {
            if (model.politicParties == null) model.politicParties = new ObservableCollection<politicParty>();
            //Si el registro no existe, procedemos a crearlo
            if (president.Text.Length != 0 && name.Text.Length != 0 && acronym.Text.Length != 0)
            {
                if (model.politicParties.Where(x => x.acromyn == model.acromyn).FirstOrDefault() == null)
                {
                    model.politicParties.Add(new politicParty
                    {
                        acromyn = model.acromyn,
                        name = model.name,
                        president = model.president
                    });
                    //una vez agregado el registro al modelo, lo agregamos a la BDD
                    model.NewParty();
                    dgvPoliticParty.Items.Refresh();
                    vaciarDatos();
                    activarDesactivarCreacion(false);
                    if (model.politicParties.Count == 10)
                    {
                        simulation.IsEnabled = true;
                    }
                    else
                    {
                        simulation.IsEnabled = false;
                    }
                }
                //Si el registro ya existe, debemos actualizarlo
                else
                {
                    MessageBox.Show("Se ha duplicado uno de los acronimos");
                }
            }
            else {
                MessageBox.Show("No se han rellenado todos los campos");
            }
        }
        /// <summary>
        /// Al pulsar fuera del data grid deselecciona los partidos que estuvieran seleccionados
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource != dgvPoliticParty)
            {
                dgvPoliticParty.SelectedItem = null;
                botonDelete.IsEnabled = false;
            }
        }
        /// <summary>
        /// Desactiva y vacia las textBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void botonCancelar_Click(object sender, RoutedEventArgs e)
        {
            activarDesactivarCreacion(false);
            vaciarDatos();
        }
        /// <summary>
        /// Se encarga de activar o desactivar los elementos de la pestaña parties management
        /// </summary>
        /// <param name="activar">booleana para comprobar si hay que activar o desactivar</param>
        private void activarDesactivarCreacion(bool activar)
        {
            botonCancelar.IsEnabled = activar;
            botonNew.IsEnabled = !activar;
            acronym.IsEnabled = activar;
            name.IsEnabled = activar;
            president.IsEnabled = activar;
        }
        /// <summary>
        /// Se encarga vaciar las textBox de la pestaña parties management
        /// </summary>
        private void vaciarDatos()
        {
            acronym.Text = string.Empty;
            name.Text = string.Empty;
            president.Text = string.Empty;
        }
        /// <summary>
        /// Comprueba cada vez que se cambia el texto de las textBox si todas contienen algo y activa el boton de save
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextChanged(object sender, TextChangedEventArgs e)
        {
            if (president.Text.Length != 0 && name.Text.Length != 0 && acronym.Text.Length != 0)
            {
                botonSave.IsEnabled = true;
            }
            else
            {
                botonSave.IsEnabled = false;
            }
        }
        /// <summary>
        /// Se encarga de iniciar la simulacion de los escaños
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void initSimulation_Click(object sender, RoutedEventArgs e)
        {
            List<int> votes;
            List<int> seats = new List<int>();

            List<politicParty> listPoliticParty = new List<politicParty>(model.politicParties);
            politicParty blankVotes = new politicParty { 
                acromyn = "blankvotes",
                name = "blankvotes",
                president = "blankvotes"
            };

            listPoliticParty.Add(blankVotes);
            votes = calculateVotes(listPoliticParty);
            if (votes.Count != 0)
            {
                seats = calculateSeats(votes);
            }
            for (int i = 0; i < listPoliticParty.Count; i++)
            {
                if (seats.Count > i)
                 {
                     listPoliticParty[i].seats = seats[i];
                 }
                 else
                 {
                     listPoliticParty[i].seats = 0;
                 }
            }

           dgvPoliticPartySimulation.ItemsSource = listPoliticParty;
        }
        /// <summary>
        /// Calcula los votos de cada partido (porcentajes entregados por la practica)
        /// </summary>
        /// <param name="listPoliticParty"> lista de partidos politicos</param>
        /// <returns>lista con los votos de los partidos que tienen mas del 3% de los votos</returns>
        private List<int> calculateVotes(List<politicParty> listPoliticParty)
        {
            List<double> percent = new List<double> { 35.25, 24.75, 15.75, 14.25, 3.75, 3.25, 1.5, 0.5, 0.25, 0.25, 0.5 };
            List<int> votes = new List<int>();
            int totalVotes = POPULATION - int.Parse(abstention.Text.Trim()) - int.Parse(nVotes.Text.Trim());
            int cont = 0;
            foreach (politicParty p in listPoliticParty)
            {
            p.votes = (int)(totalVotes * percent[cont] / 100);
                if (percent[cont] >= 3 && p.votes != 0)
                {
                    votes.Add(p.votes);
                }
             cont++;
            }

        return votes;
    }
    /// <summary>
    /// Se encarga de calcular los escaños de cada partido
    /// </summary>
    /// <param name="votes">reciba una lista con los votos de los partidos que han superado el 3%</param>
    /// <returns>devuelve los escaños que reciben cada grupo</returns>
    private List<int> calculateSeats(List<int> votes)
     {
        List<int> dividendo = new List<int>();
        int totalSeats = 37;
        int cont = 0;
        List<int> resul;
        for (int i = 0; i < votes.Count; i++)
        {
            dividendo.Add(1);
        }

        for (int i = 0; i < totalSeats; i++)
        {
            resul = new List<int>();
            foreach (int v in votes)
            {
                resul.Add(v / dividendo[cont]);
                cont++;
            }
            cont = 0;
            dividendo[resul.IndexOf(resul.Max())]++;
        }

        for (int i = 0; i < votes.Count; i++)
        {
            dividendo[i]--;
        }
        return dividendo;
        }
     }
}
