﻿using System;
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
    /// Logique d'interaction pour Menu.xaml
    /// </summary>
    public partial class Menu : Window
    {
        public Menu()
        {
            InitializeComponent();
            ImageBrush imgMenu = new ImageBrush();
            imgMenu.ImageSource = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "image\\menu.jpg"));
            rMenu.Fill = imgMenu;

        }

        private void Jouer_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void Annuler_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        public void Musique(object sender, RoutedPropertyChangedEventArgs<double> e) 
        {
            ((MainWindow)Application.Current.MainWindow).volumeMusic = (double)this.Sons.Value/100;
        }
    }
}
