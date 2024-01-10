using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace ProjetTeckel
{

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Menu ChoixMenu = new Menu();
            ChoixMenu.Show();

            if (ChoixMenu.DialogResult == false) 
                Application.Current.Shutdown(); 
            InitializeComponent();
        }

        private void teckel_ToucheBas(object envoye, KeyEventArgs e)
        {
            int ligneRec = Grid.GetRow(teckel);
            int colRec = Grid.GetColumn(teckel);
            switch (e.Key.ToString())
            {
                case "Haut":
                    if (ligneRec > 0)
                    {
                        ligneRec = ligneRec - 1;
                        Grid.SetRow(teckel, ligneRec);//déplace le rectangle sur ces nouvelles coordonnées
                    }
                    break;
            }
        }

        private void teckel_ToucheHaut(object envoye, KeyEventArgs e)
        {
            int ligneRec = Grid.GetRow(teckel);
            int colRec = Grid.GetColumn(teckel);
            switch (e.Key.ToString())
            {
                case "Haut":
                    if (ligneRec > 0)
                    {
                        ligneRec = ligneRec + 1;
                        Grid.SetRow(teckel, ligneRec);//déplace le rectangle sur ces nouvelles coordonnées
                    }
                    break;
            }

            
        }
    }
}
