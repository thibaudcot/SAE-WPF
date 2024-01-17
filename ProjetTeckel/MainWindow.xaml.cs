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
        int nombrecorps = 0;
        int score = 0;
        int _direction = 0; //variable qui permettra de savoir la derniere direction choisi par l'utilisateur
        Rectangle nourriture = new Rectangle();//rectangle correspondant à la nourriture, qui sera inséré dans la grille dynamiquement
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
        int MeilleureScore;

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
            timer.Interval = TimeSpan.FromMilliseconds(vitesse); //each 150 MilliSeconds the timer_Tick function will be executed
            timer.Tick += timer_Tick;
            timer.Start();
            


        }


        void timer_Tick(object sender, EventArgs e)
        {
            Score();
            //on récupère les coordonnées du snake sur la grille
            ligneRec = Grid.GetRow(teckel);
            colRec = Grid.GetColumn(teckel);
            Console.WriteLine("row : " + ligneRec + "\nligne : " + colRec);
            corpsTeckel.Insert(0, new Point(colRec, ligneRec));

            // Dessine les parties du corps du chien
            //foreach (var point in corpsTeckel)
            for (int i = 0;i<corpsTeckel.Count;i++)
            {
                Rectangle corpsPart = new Rectangle();
                corpsPart.Width = teckel.Width;
                corpsPart.Height = teckel.Height;
                ImageBrush imgCorps = new ImageBrush();
                imgCorps.ImageSource = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "image\\corps.png")); // Remplacez par le chemin de votre image de corps
                corpsPart.Fill = imgCorps;

                Grid.Children.Add(corpsPart);
                Grid.SetColumn(corpsPart, (int)corpsTeckel[i].Y);
                Grid.SetRow(corpsPart, (int)corpsTeckel[i].X);                                     
            }
            
            deplacement_corps();
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
                    } while (listeOs.Any(c => Grid.GetRow(c.Rectangle) == xOs && Grid.GetColumn(c.Rectangle) == yOs));

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
                    } while (xChocolat == Grid.GetRow(nourriture) && yChocolat == Grid.GetColumn(nourriture));

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


            if (score < 0)
            {
                GameOver();
            }

        }
    
            private void deplacement_corps()
        {
            foreach(Rectangle corps in Grid.Children.OfType<Rectangle>())
            {
            if (Grid.GetColumn(corps) == corpsTeckel[nombrecorps].X && Grid.GetRow(corps) == corpsTeckel[nombrecorps].Y) 
                {
                    trucASupprimer.Add(corps);  //regarde tout les rectangle de la grille et si rectangle dernier corps on le supprime 
                }
            }
            foreach(Rectangle x in trucASupprimer)
            {
                Grid.Children.Remove(x);
            }
            for (int i = 1; i < corpsTeckel.Count; i++)
            {
                corpsTeckel[i] = corpsTeckel[i-1]; //prend la place de l'element placé avant 
                
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
                        scoreacheck = 0;
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
                this.Scoretxt.Text = "Score : " + score;
            if (score > MeilleureScore)
            {
                MeilleureScore = score;
            }
        }

        
    }
}


