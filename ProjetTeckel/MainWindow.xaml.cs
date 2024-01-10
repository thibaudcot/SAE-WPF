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
using System.Windows.Media.Media3D;
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

        private void PrjTeckel_KeyDown(object sender, KeyEventArgs e)
        {
            int ligneRec = Grid.GetRow(teckel);
            int colRec = Grid.GetColumn(teckel);
            switch (e.Key.ToString())
            {
                case "Up":
                    if (ligneRec > 0)
                    {
                        ligneRec = ligneRec - 1;
                        Grid.SetRow(teckel, ligneRec);//déplace le rectangle sur ces nouvelles coordonnées
                    }
                    break;

                case "Down":
                    if (ligneRec < 15)
                    {
                        ligneRec = ligneRec + 1;
                        Grid.SetRow(teckel, ligneRec);
                    }
                    break;

                case "Right":
                    // Check if collided
                    if (colRec < 9)
                    {
                        colRec = colRec + 1;
                        Grid.SetColumn(teckel, colRec);
                    }
                    break;
                case "Left":
                    // Check if collided
                    if (colRec > 0)
                    {
                        colRec = colRec - 1;
                        Grid.SetColumn(teckel, colRec);
                    }
                    break;
            }
        }
    }
}
