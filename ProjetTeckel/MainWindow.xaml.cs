﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using static ProjetTeckel.MainWindow;

namespace ProjetTeckel
{

    public partial class MainWindow : Window
    {
        //Rectangle chocolat = new Rectangle();
        List<ChocolatInfo> listeChocolats = new List<ChocolatInfo>();
        int score = 0;
        int _direction = 0; //variable qui permettra de savoir la derniere direction choisi par l'utilisateur
        Rectangle nourriture = new Rectangle();//rectangle correspondant à la nourriture, qui sera inséré dans la grille dynamiquement
        Random randomN = new Random();
        Random randomC = new Random();
        List<Rectangle> corpsChien = new List<Rectangle>(); // Nouvelle liste pour stocker les parties du corps du chien
        ImageBrush imgTeckelHaut;
        ImageBrush imgTeckelGauche;
        ImageBrush imgTeckelDroite;
        ImageBrush imgTeckelBas;

        public MainWindow()
        {
            InitializeComponent();
            music.Source = new Uri(AppDomain.CurrentDomain.BaseDirectory + "sons/uranus.mp3");
            music.Play();

            ImageBrush Map = new ImageBrush();
            Map.ImageSource = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "image\\map.png"));
            Grid.Background = Map;
            // Initialisation des images pour différentes directions
            imgTeckelHaut = new ImageBrush();
            imgTeckelHaut.ImageSource = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "image\\tete.png"));

            imgTeckelGauche = new ImageBrush();
            imgTeckelGauche.ImageSource = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "image\\tetegauche.png"));

            imgTeckelDroite = new ImageBrush();
            imgTeckelDroite.ImageSource = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "image\\tetedroite.png"));

            imgTeckelBas = new ImageBrush();
            imgTeckelBas.ImageSource = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "image\\tetebas.png"));

            teckel.Fill = imgTeckelHaut;

            Menu ChoixMenu = new Menu();
            ChoixMenu.ShowDialog();





            if (ChoixMenu.DialogResult == false)
                Application.Current.Shutdown();
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(100); //each 150 MilliSeconds the timer_Tick function will be executed
            timer.Tick += timer_Tick;
            timer.Start();
            score = 0;
            Score();

        }

        void timer_Tick(object sender, EventArgs e)
        {
            //on récupère les coordonnées du snake sur la grille
            int ligneRec = Grid.GetRow(teckel);
            int colRec = Grid.GetColumn(teckel);

            //on choisit des coordonnées aléatoire pour la nourriture
            int xNourriture = randomN.Next(0, 17);
            int yNourriture = randomN.Next(0, 20);


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
            if (Grid.Children.Count < 3)
            {
                for (int i = 0; i <= 10; i++)
                {
                    Rectangle chocolat = new Rectangle();
                    int xChocolat = randomC.Next(0, 17);
                    int yChocolat = randomC.Next(0, 20);

                    chocolat.Width = teckel.Width;
                    chocolat.Height = teckel.Height;

                    ImageBrush imgChocolat = new ImageBrush();
                    imgChocolat.ImageSource = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "image\\chocolat.png"));
                    chocolat.Fill = imgChocolat;

                    listeChocolats.Add(new ChocolatInfo { Rectangle = chocolat, Numero = i });

                    Grid.Children.Add(chocolat);
                    Grid.SetColumn(chocolat, yChocolat);
                    Grid.SetRow(chocolat, xChocolat);
                }

                if (Grid.GetRow(teckel) == Grid.GetRow(nourriture) && Grid.GetColumn(teckel) == Grid.GetColumn(nourriture))
                {
                    Grid.Children.Remove(nourriture);
                    score++;
                    Score();
                }

                foreach (ChocolatInfo chocolatInfo in listeChocolats.ToList())
                {
                    int rowChocolat = Grid.GetRow(chocolatInfo.Rectangle);
                    int columnChocolat = Grid.GetColumn(chocolatInfo.Rectangle);

                    Debug.WriteLine($"Chocolat {chocolatInfo.Numero} - Row: {rowChocolat}, Column: {columnChocolat}");

                    if (rowChocolat == Grid.GetRow(teckel) && columnChocolat == Grid.GetColumn(teckel))
                    {
                        Debug.WriteLine($"Collision avec le chocolat {chocolatInfo.Numero} !");
                        Grid.Children.Remove(chocolatInfo.Rectangle);
                        listeChocolats.Remove(chocolatInfo);
                    }
                }



            }




            switch (_direction)
            {

                case 1: // Haut
                    if (ligneRec > 0)
                    {
                        ligneRec = ligneRec - 1;
                        Grid.SetRow(teckel, ligneRec);
                        teckel.Fill = imgTeckelHaut;
                    }
                    else
                    {
                        GameOver();
                    }
                    break;

                case 2: // Gauche
                    if (colRec > 0)
                    {
                        colRec = colRec - 1;
                        Grid.SetColumn(teckel, colRec);
                        teckel.Fill = imgTeckelGauche;
                    }
                    else
                    {
                        GameOver();
                    }
                    break;

                case 3: // Droite
                    if (colRec < 19)
                    {
                        colRec = colRec + 1;
                        Grid.SetColumn(teckel, colRec);
                        teckel.Fill = imgTeckelDroite;
                    }
                    else
                    {
                        GameOver();
                    }
                    break;

                case 4: // Bas
                    if (ligneRec < 17)
                    {
                        ligneRec = ligneRec + 1;
                        Grid.SetRow(teckel, ligneRec);
                        teckel.Fill = imgTeckelBas;
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
                AjouterPartieCorpsChien();
            }

            foreach (ChocolatInfo chocolatInfo in listeChocolats.ToList())
            {
                // Obtenez les coordonnées du chocolat dans la grille
                int rowChocolat = Grid.GetRow(chocolatInfo.Rectangle);
                int columnChocolat = Grid.GetColumn(chocolatInfo.Rectangle);
                int rowTeckel = Grid.GetRow(teckel);
                int columnTeckel = Grid.GetColumn(teckel);

                // Vérifiez s'ils se trouvent dans la même cellule de la grille
                if (rowTeckel == rowChocolat && columnTeckel == columnChocolat)
                {
                    // Affichez les coordonnées pour déboguer
                    Debug.WriteLine($"Collision avec chocolat {chocolatInfo.Numero} - Row: {rowChocolat}, Column: {columnChocolat}");

                    // Supprimez le rectangle de la grille et de la liste
                    Grid.Children.Remove(chocolatInfo.Rectangle);
                    listeChocolats.Remove(chocolatInfo);
                    score--;
                    Score();
                }
            }



            // Mettre à jour la position de la première partie du corps à la position actuelle de la tête
            if (corpsChien.Count > 0)
            {
                Grid.SetColumn(corpsChien[0], Grid.GetColumn(teckel));
                Grid.SetRow(corpsChien[0], Grid.GetRow(teckel));
            }

            // Déplacer chaque partie du corps vers la position de la partie précédente
            for (int i = 1; i < corpsChien.Count; i++)
            {
                Grid.SetColumn(corpsChien[i], Grid.GetColumn(corpsChien[i - 1]));
                Grid.SetRow(corpsChien[i], Grid.GetRow(corpsChien[i - 1]));
            }


            if (score < 0)
            {
                GameOver();
            }

        }
    
            private void AjouterPartieCorpsChien()
            {
                Rectangle nouvellePartieCorps = new Rectangle();
                nouvellePartieCorps.Width = teckel.Width;
                nouvellePartieCorps.Height = teckel.Height;

                ImageBrush imgPartieCorps = new ImageBrush();
                imgPartieCorps.ImageSource = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "image\\corps.png"));
                nouvellePartieCorps.Fill = imgPartieCorps;

                // Ajouter la nouvelle partie du corps à la liste
                corpsChien.Insert(0, nouvellePartieCorps);

                // Ajouter la nouvelle partie du corps à la grille
                Grid.Children.Insert(1, nouvellePartieCorps);

                // Mettre à jour la position de chaque partie du corps
                for (int i = 0; i < corpsChien.Count; i++)
                {
                    Grid.SetColumn(corpsChien[i], Grid.GetColumn(teckel));
                    Grid.SetRow(corpsChien[i], Grid.GetRow(teckel));
                }
            }
            private void RetirerPartieCorpsChien()
            {
                if (corpsChien.Count > 0)
                {
                    // Retirer la dernière partie du corps de la grille et de la liste
                    Grid.Children.Remove(corpsChien.Last());
                    corpsChien.RemoveAt(corpsChien.Count - 1);
                }
            }

            private void PrjTeckel_KeyDown(object sender, KeyEventArgs e)
            {

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
                ImageBrush imgTeckel = new ImageBrush();
                imgTeckel.ImageSource = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "image\\tetemort.png"));
                teckel.Fill = imgTeckel;
                MessageBoxResult result = MessageBox.Show("Peux Mieux Faire xD" + "\n" + "Ton score : " + score + "\n" + "Tu veux recommencer ?", "Teckel", MessageBoxButton.YesNoCancel);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        Grid.Children.Remove(nourriture);
                    Grid.SetColumn(teckel, 2);
                        Grid.SetRow(teckel, 2);
                        _direction = 0;
                        score = 0;
                        imgTeckel.ImageSource = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "image\\tete.png"));
                        teckel.Fill = imgTeckel;
                        Menu ChoixMenu = new Menu();
                        ChoixMenu.ShowDialog();


                        if (ChoixMenu.DialogResult == false)
                            Application.Current.Shutdown();
                        break;
                    case MessageBoxResult.No:
                        Close();
                        break;
                    case MessageBoxResult.Cancel:
                        MessageBox.Show("Nevermind then...", "Teckel");
                        break;
                }

            }
            private void Score()
            {
                this.PrjTeckel.Title = "Teckel - Score : " + score;
            }

        public class ChocolatInfo
        {
            public int Numero { get; set; }
            public Rectangle Rectangle { get; set; }
        }
    }
}


