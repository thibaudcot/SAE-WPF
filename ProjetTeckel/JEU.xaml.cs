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
using System.Windows.Shapes;

namespace ProjetTeckel
{
    /// <summary>
    /// Logique d'interaction pour JEU.xaml
    /// </summary>
    public partial class JEU : Window
    {
        public JEU()
        {
            InitializeComponent();
        }

        private void teckel_touche_appuiyer(object sender, KeyEventArgs e)
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

            
    
    }
}
