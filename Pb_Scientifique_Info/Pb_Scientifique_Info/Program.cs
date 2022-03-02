using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Media;
using System.ComponentModel;
using System.IO;

namespace Pb_Scientifique_Info
{
    class Program
    {     

        

        static void Main(string[] args)
        {
            MyImage test = new MyImage("tigre.bmp");

            if (test.TypeImage == "BitMap")
            {

                Console.WriteLine("type d'image : " + test.TypeImage);
                Console.WriteLine("taille du fichier : " + test.TailleFichier);
                Console.WriteLine("taille offset : " + test.TailleOffset);
                Console.WriteLine("hauteur de l'image : " + test.HauteurImage);
                Console.WriteLine("largeur de l'image : " + test.LargeurImage);
                Console.WriteLine("nombre de bits couleur : " + test.NbBitsCouleur);

                test.From_Image_To_File("verif_image.bmp");
                //Process.Start("verif_image.bmp");

                MyImage testNoirEtBlanc = test.NoirEtBlanc();
                testNoirEtBlanc.From_Image_To_File("testNoirEtBlanc.bmp");

                MyImage testNuanceDeGris = test.NuanceDeGris();
                testNuanceDeGris.From_Image_To_File("testNuanceDeGris.bmp");

                MyImage testInverse = test.Inverse();
                testInverse.From_Image_To_File("testInverse.bmp");

                MyImage testMiroirHorizontal = test.MiroirHorizontal();
                testMiroirHorizontal.From_Image_To_File("testMiroir.bmp");

                MyImage testMiroirVertical = test.MiroirVertical();
                testMiroirVertical.From_Image_To_File("testMiroir2.bmp");

                Console.WriteLine("Veuillez choisir un coefficient d'agrandissement (hauteur) :");
                int coefHauteurAgrandi = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Veuillez choisir un coefficient d'agrandissement (largeur) :");
                int coefLargeurAgrandi = Convert.ToInt32(Console.ReadLine());
                MyImage testAgrandi = test.Agrandir(coefHauteurAgrandi, coefLargeurAgrandi);
                testAgrandi.From_Image_To_File("testAgrandi.bmp");

                Console.WriteLine("Veuillez choisir un coefficient de réduction (hauteur) :");
                int coefHauteurReduit = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Veuillez choisir un coefficient de réduction (largeur) :");
                int coefLargeurReduit = Convert.ToInt32(Console.ReadLine());
                MyImage testReduit = test.Reduire(coefHauteurReduit, coefLargeurReduit);
                if (testReduit != null) testReduit.From_Image_To_File("testReduit.bmp");
                else Console.WriteLine("coefficient de réduction impossible");

                /*Console.WriteLine("Veuillez choisir un angle de rotation vers la droite :");        //rotation
                int angle = Convert.ToInt32(Console.ReadLine());
                MyImage testRotation = test.Rotation(angle);
                testRotation.From_Image_To_File("testRotation.bmp");*/

                MyImage testDetectionContours = test.DetectionContours();
                testDetectionContours.From_Image_To_File("testDetectionContours.bmp");

                MyImage testFlou = test.Flou();
                testFlou.From_Image_To_File("testFlou.bmp");

                MyImage testPsychedelique = test.Psychedelique();
                testPsychedelique.From_Image_To_File("testPsychedelique.bmp");

                MyImage testAugmentationContraste = test.AugmentationContraste();
                testAugmentationContraste.From_Image_To_File("testAugmentationContraste.bmp");

                MyImage testRenforcementBordsVertical = test.RenforcementBordsVertical();
                testRenforcementBordsVertical.From_Image_To_File("testRenforcementBords.bmp");

                MyImage testRenforcementBordsHorizontal = test.RenforcementBordsHorizontal();
                testRenforcementBordsHorizontal.From_Image_To_File("testRenforcementBords2.bmp");

                MyImage testRepoussage = test.Repoussage();
                testRepoussage.From_Image_To_File("testRepoussage.bmp");

                MyImage testFiltreSobel = test.FiltreSobel();
                testFiltreSobel.From_Image_To_File("testFiltreSobel.bmp");

                MyImage testJSP = test.ConvolutionAleatoire();
                testJSP.From_Image_To_File("testConvolutionAleatoire.bmp");

                /*Console.WriteLine("Header :");
                for (int i = 0; i < 14; i++) Console.Write(test.FichierComplet[i] + " ");
                Console.WriteLine("\n\nHeader Info :");
                for (int i = 14; i < 54; i++) Console.Write(test.FichierComplet[i] + " ");
                Console.WriteLine("\n\nImage :");
                for (int i = 54; i < test.FichierComplet.Length; i += test.LargeurImage*3)
                {
                    for (int j = i; j < i + test.LargeurImage*3; j++)
                        Console.Write(test.FichierComplet[j] + " ");
                    Console.WriteLine();
                }*/
            }
            else
            {
                Console.WriteLine("L'image fournie n'est pas un BitMap");
            }
        }
    }
}
