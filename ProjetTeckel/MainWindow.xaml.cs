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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void teckel_KeyDown(object sender, KeyEventArgs e)
        {
            int rowRec = Grid.GetRow(teckel);
            int colRec = Grid.GetColumn(teckel);
            switch (e.Key.ToString())
            {
                case "Up":
                    if (rowRec > 0)
                    {
                        rowRec = rowRec - 1;
                        Grid.SetRow(teckel, rowRec);//déplace le rectangle sur ces nouvelles coordonnées
                    }
                    break;
                case "Down":
                    if (rowRec > 0)
                    {
                        rowRec = rowRec + 1;
                        Grid.SetRow(teckel, rowRec);//déplace le rectangle sur ces nouvelles coordonnées
                    }
                    break;
            }
        }

    }
}
