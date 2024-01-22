using System;
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
using static ProjetTeckel.Moteur.Nourriture;

namespace ProjetTeckel
{

    public partial class MainWindow : Window
    {
        List<ChocolatInfo> listeChocolats = new List<ChocolatInfo>();
        List<OsInfo> listeOs = new List<OsInfo>();
        List<Point> corpsTeckel = new List<Point>();//liste point corps
        List<Rectangle> trucASupprimer = new List<Rectangle>();
        List<Rectangle> rectTeckel = new List<Rectangle>();
        int nombrecorps = 0;
        int score = 0;
        int _direction = 0; //variable qui permettra de savoir la derniere direction choisi par l'utilisateur
        Random randomN = new Random();
        Random randomC = new Random();
        ImageBrush imgChocolat = new ImageBrush();
        ImageBrush imgOs = new ImageBrush();
        ImageBrush imgTeckelHaut;
        ImageBrush imgTeckelGauche;
        ImageBrush imgTeckelDroite;
        ImageBrush imgTeckelBas;
        int ligneRec;
        int colRec;
        int scoreacheck;
        int vitesse = 150;
        int AugObjet = 5;
        int xOs, yOs;
        int xChocolat, yChocolat;
        int rowNourriture, columnNourriture;
        int rowTeckel, columnTeckel;
        int rowChocolat, columnChocolat;
        int meilleurScore;
        int tailleCorps = 0;
        int rowCorps, columnCorps;



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
            timer.Interval = TimeSpan.FromMilliseconds(vitesse); 
            timer.Tick += timer_Tick;
            timer.Start();
            


        }


        void timer_Tick(object sender, EventArgs e)
        {
            Score();
            //on récupère les coordonnées du snake sur la grille
            ligneRec = Grid.GetRow(teckel);
            colRec = Grid.GetColumn(teckel);
            //Console.WriteLine("row : " + ligneRec + "\nligne : " + colRec);
            corpsTeckel.Insert(0, new Point(colRec, ligneRec));
            
            
           
            if (scoreacheck == 3)
            {
                vitesse += 25;
                scoreacheck = 0;
                if (listeChocolats.Count > AugObjet)
                {
                    for (int i = 0; i <= 5; i++)
                    {
                       Rectangle chocolat = new Rectangle();
                        do
                        {
                            xChocolat = randomN.Next(0, 17);
                            yChocolat = randomN.Next(0, 20);
                        } while (listeChocolats.Any(c => Grid.GetRow(c.Rectangle) == xChocolat && Grid.GetColumn(c.Rectangle) == yChocolat));

                        chocolat.Width = teckel.Width;
                        chocolat.Height = teckel.Height;

                        
                        imgChocolat.ImageSource = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "image\\chocolat.png"));
                        chocolat.Fill = imgChocolat;

                        listeChocolats.Add(new ChocolatInfo { Rectangle = chocolat, Numero = i });

                        Grid.Children.Add(chocolat);
                        Grid.SetColumn(chocolat, yChocolat);
                        Grid.SetRow(chocolat, xChocolat);
                    }
                }
           }
            if(listeOs.Count == 0)
            {
                for (int i = 0; i <= 2; i++)
                {
                    Rectangle os = new Rectangle();
                    do
                    {
                        xOs = randomN.Next(0, 17);
                        yOs = randomN.Next(0, 20);
                    } while (xOs == xChocolat && yOs == yChocolat);

                    os.Width = teckel.Width;
                    os.Height = teckel.Height;

                    imgOs.ImageSource = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "image\\os.png"));
                    os.Fill = imgOs;

                    listeOs.Add(new OsInfo { Rectangle = os, Numero = i });

                    Grid.Children.Add(os);
                    Grid.SetColumn(os, yOs);
                    Grid.SetRow(os, xOs);
                }
                
            }
            foreach (OsInfo osInfo in listeOs.ToList())
            {
                // Obtenez les coordonnées du chocolat dans la grille
                rowNourriture = Grid.GetRow(osInfo.Rectangle);
                columnNourriture = Grid.GetColumn(osInfo.Rectangle);
                rowTeckel = Grid.GetRow(teckel);
                columnTeckel = Grid.GetColumn(teckel);

                // Vérifiez s'ils se trouvent dans la même cellule de la grille
                if (rowTeckel == rowNourriture && columnTeckel == columnNourriture)
                {
                    // Supprimez le rectangle de la grille et de la liste
                    Grid.Children.Remove(osInfo.Rectangle);
                    tailleCorps ++;
                    listeOs.Remove(osInfo);
                    score++;
                    scoreacheck++;
                    Score();
                    nombrecorps++;
                    corpsTeckel[nombrecorps] = new Point(corpsTeckel[nombrecorps - 1].X, corpsTeckel[nombrecorps - 1].Y); //rajouter un corps de teckel des qu'un os est mangé 
                }
            }

            if (listeChocolats.Count < 5)
            {
                for (int i = 0; i <= 15; i++)
                {
                    Rectangle chocolat = new Rectangle();
                    // Générer des coordonnées aléatoires pour le chocolat en évitant l'emplacement de l'os
                    do
                    {
                        xChocolat = randomC.Next(0, 17);
                        yChocolat = randomC.Next(0, 20);
                    } while (xChocolat == xOs && yChocolat == yOs);

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
                
            }

                foreach (ChocolatInfo chocolatInfo in listeChocolats.ToList())
                {
                    rowChocolat = Grid.GetRow(chocolatInfo.Rectangle);
                    columnChocolat = Grid.GetColumn(chocolatInfo.Rectangle);

                    if (rowChocolat == Grid.GetRow(teckel) && columnChocolat == Grid.GetColumn(teckel))
                    {
                        score--;
                        scoreacheck--;
                        Score();
                        nombrecorps--;
                    
                    
                    if (tailleCorps > 1)
                    {
                        tailleCorps--;
                    }

                    Grid.Children.Remove(chocolatInfo.Rectangle);
                        listeChocolats.Remove(chocolatInfo);
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
                        deplacement_corps();
                        Corps();
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
                        deplacement_corps();
                        Corps();
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
                        deplacement_corps();
                        Corps();
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
                        deplacement_corps();
                        Corps();
                    }
                    else
                    {
                        GameOver();
                    }
                    break;
            }


            if (score < 0)
            {
                GameOver();
            }

        }
    
           private void deplacement_corps()
           {
            Console.WriteLine(rectTeckel.Count);
            if (rectTeckel.Count > tailleCorps)
            {
                Grid.Children.Remove(rectTeckel[0]);
                rectTeckel.RemoveAt(0);
            }


            trucASupprimer.Clear();



            for (int i = 1; i < corpsTeckel.Count; i++)
            {
                corpsTeckel[i] = corpsTeckel[i - 1]; //prend la place de l'element placé avant 

            }
            /*foreach (Rectangle corps in Grid.Children.OfType<Rectangle>())
            {
                rowTeckel = Grid.GetRow(teckel);
                columnTeckel = Grid.GetColumn(teckel);

                rowCorps = Grid.GetRow(corps);
                columnCorps = Grid.GetColumn(corps);

                // Vérifiez s'ils se trouvent dans la même cellule de la grille que le teckel
                if (rowCorps == rowTeckel && columnCorps == columnTeckel)
                {
                    // Si le teckel touche son propre corps, c'est la fin du jeu
                    GameOver();
                    return;
                }
            }*/

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
            imgTeckelHaut.ImageSource = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "image\\tetemort.png"));
            teckel.Fill = imgTeckelHaut;

            MessageBoxResult result = MessageBox.Show("Peux Mieux Faire xD" + "\n" + "Ton score : " + score + "\n" + "Tu veux recommencer ?", "Teckel", MessageBoxButton.YesNo);

            switch (result)
            {
                case MessageBoxResult.Yes:
                    ReinitialiserJeu();
                    break;

                case MessageBoxResult.No:
                    Close();
                    break;
            }

        }
        private void ReinitialiserJeu()
        {
            SupprimerTousChocolatsEtOs();

            // Remettre la tête du teckel à sa position initiale
            Grid.SetColumn(teckel, 2);
            Grid.SetRow(teckel, 2);

            // Réinitialiser les variables d'état du jeu
            _direction = 0;
            score = 0;
            scoreacheck = 0;
            nombrecorps = 0;
            tailleCorps = 0;
            corpsTeckel.Clear();
            rectTeckel.Clear();
            listeChocolats.Clear();
            listeOs.Clear();

            // Remettre l'image de la tête du teckel à sa version normale
            imgTeckelHaut.ImageSource = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "image\\tete.png"));
            teckel.Fill = imgTeckelHaut;

            // Afficher à nouveau le menu
            Menu ChoixMenu = new Menu();
            ChoixMenu.ShowDialog();

            if (ChoixMenu.DialogResult == false)
                Application.Current.Shutdown();
        }
        private void Score()
            {
                this.Scoretxt.Text = "Score : " + score;
            if (score > meilleurScore)
            {
                meilleurScore = score;
            }
                this.MScoretxt.Text = "Meilleur Score : " + meilleurScore;
            }
        private void SupprimerTousChocolatsEtOs()
        {
            foreach (UIElement element in Grid.Children.OfType<Rectangle>().Where(e => e != teckel).ToList())
            {
                Grid.Children.Remove(element);
            }


        }
        private void Corps()
        {
            Rectangle corpsPart = new Rectangle();
            corpsPart.Width = teckel.Width;
            corpsPart.Height = teckel.Height;
            ImageBrush imgCorps = new ImageBrush();
            imgCorps.ImageSource = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "image\\corps.png"));
            corpsPart.Fill = imgCorps;
            rectTeckel.Add(corpsPart);
            Grid.Children.Add(corpsPart);
            Grid.SetColumn(corpsPart, Grid.GetColumn(teckel));
            Grid.SetRow(corpsPart, Grid.GetRow(teckel));
        }


    }
}


