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
using System.Windows.Threading;
using System.Xml.Linq;

namespace ProjetTeckel
{

    public partial class MainWindow : Window
    {
            int score = 0;
            int _direction = 0; //variable qui permettra de savoir la derniere direction choisi par l'utilisateur
            Rectangle nourriture = new Rectangle();//rectangle correspondant à la nourriture, qui sera inséré dans la grille dynamiquement
            Random randomN = new Random();
            Rectangle chocolat = new Rectangle();//rectangle correspondant à la nourriture, qui sera inséré dans la grille dynamiquement
            Random randomC = new Random();
        public MainWindow()
        {
            InitializeComponent();
            ImageBrush imgTeckel = new ImageBrush();
            imgTeckel.ImageSource = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "image\\tete.png"));
            teckel.Fill = imgTeckel;
            Menu ChoixMenu = new Menu();
            ChoixMenu.ShowDialog();

            if (ChoixMenu.DialogResult == false) 
                Application.Current.Shutdown();
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(150); //each 150 MilliSeconds the timer_Tick function will be executed
            timer.Tick += timer_Tick;
            timer.Start();
        }
        void timer_Tick(object sender, EventArgs e)
        {
            //on récupère les coordonnées du snake sur la grille
            int ligneRec = Grid.GetRow(teckel);
            int colRec = Grid.GetColumn(teckel);

            //on choisit des coordonnées aléatoire pour la nourriture
            int xNourriture = randomN.Next(0, Grid.RowDefinitions.Count);
            int yNourriture = randomN.Next(0, Grid.ColumnDefinitions.Count);
            int xChocolat = randomN.Next(0, Grid.RowDefinitions.Count);
            int yChocolat = randomN.Next(0, Grid.ColumnDefinitions.Count);

            //Si aucune nourriture n'est présente alors on l'ajoute 
            if (!Grid.Children.Contains(nourriture) && _direction != 0)
            {
                //Le rectangle nourriture aura les meme dimensions que le snake
                nourriture.Width = teckel.Width;
                nourriture.Height = teckel.Height;
                //Os pour la nourriture
                ImageBrush imgNourriture = new ImageBrush();
                imgNourriture.ImageSource = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "image\\os.png"));
                nourriture.Fill = imgNourriture;
                //On l'ajoute à la grille
                Grid.Children.Add(nourriture);
                Grid.SetColumn(nourriture, yNourriture);
                Grid.SetRow(nourriture, xNourriture);

            }
            if (!Grid.Children.Contains(chocolat) && _direction != 0)
            {
                chocolat.Width = teckel.Width;
                chocolat.Height = teckel.Height;
                //Chocolat pour le malus
                ImageBrush imgChocolat = new ImageBrush();
                imgChocolat.ImageSource = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "image\\chocolat.jpg"));
                chocolat.Fill = imgChocolat;
                //On l'ajoute à la grille
                Grid.Children.Add(chocolat);
                Grid.SetColumn(chocolat, yChocolat);
                Grid.SetRow(chocolat, xChocolat);

            }
            switch (_direction)
            {

                case 1://up
                    if (ligneRec > 1)
                    {
                        ligneRec = ligneRec - 1;
                        Grid.SetRow(teckel, ligneRec);//déplace le rectangle sur ces nouvelles coordonnées
                    }
                    else
                    {
                        GameOver();
                    }
                    break;
                case 2://left
                    if (colRec > 0)
                    {
                        colRec = colRec - 1;
                        Grid.SetColumn(teckel, colRec);
                    }
                    else
                    {
                        GameOver();
                    }
                    break;
                case 3: //right
                    if (colRec < 18)
                    {
                        colRec = colRec + 1;
                        Grid.SetColumn(teckel, colRec);
                    }
                    else
                    {
                        GameOver();
                    }
                    break;
                case 4: //down
                    if (ligneRec < 16)
                    {
                        ligneRec = ligneRec + 1;
                        Grid.SetRow(teckel,ligneRec);
                    }
                    else
                    {
                        GameOver();
                    }
                    break;
            }
            if (Grid.GetRow(teckel) == Grid.GetRow(nourriture) && Grid.GetColumn(teckel) == Grid.GetColumn(nourriture))
            {
                Grid.Children.Remove(nourriture);
                score++;
                Score();

            }
            if (Grid.GetRow(teckel) == Grid.GetRow(chocolat) && Grid.GetColumn(teckel) == Grid.GetColumn(chocolat))
            {
                Grid.Children.Remove(chocolat);
                score--;
                Score();

            }
            if(score < 0)
            {
                GameOver();
            }
        }
            private void PrjTeckel_KeyDown(object sender, KeyEventArgs e)
        {
            int ligneRec = Grid.GetRow(teckel);
            int colRec = Grid.GetColumn(teckel);
            switch (e.Key.ToString())
            {
                case "Up":
                    _direction = 1;
                    break;

                case "Down":
                    _direction = 4;
                    break;

                case "Right":
                    _direction = 3;
                    break;
                case "Left":
                    _direction = 2;
                    break;

            }
        }
        public void GameOver()
        {
            MessageBox.Show("Peux Mieux Faire xD" + "\n" + "Ton score : " + score);
            Grid.Children.Remove(nourriture);
            Grid.Children.Remove(chocolat);
            Grid.SetColumn(teckel, 2);
            Grid.SetRow(teckel, 2);
            _direction = 0;
            score = 0;
        }
        private void Score()
        {
            this.PrjTeckel.Title = "Teckel - Score : " + score;
        }
    }
}
