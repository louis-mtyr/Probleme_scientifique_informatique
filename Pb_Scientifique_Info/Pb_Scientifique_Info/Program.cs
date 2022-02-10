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

namespace Pb_Scientifique_Info
{
    class Program
    {     

        

        static void Main(string[] args)
        {
            MyImage test = new MyImage("tigre.bmp");

            Console.WriteLine("type d'image : " + test.TypeImage);
            Console.WriteLine("taille du fichier : " + test.TailleFichier);
            Console.WriteLine("taille offset : " + test.TailleOffset);
            Console.WriteLine("hauteur de l'image : " + test.HauteurImage);
            Console.WriteLine("largeur de l'image : " + test.LargeurImage);
            Console.WriteLine("nombre de bits couleur : " + test.NbBitsCouleur);

            test.From_Image_To_File("verif_image.bmp");
            //Process.Start("testingTD2.bmp");

            MyImage testNoirEtBlanc = test.NoirEtBlanc();
            testNoirEtBlanc.From_Image_To_File("testNoirEtBlanc.bmp");

            MyImage testInverse = test.Inverse();
            testInverse.From_Image_To_File("testInverse.bmp");

            MyImage testMiroir = test.Miroir();
            testMiroir.From_Image_To_File("testMiroir.bmp");

            Console.WriteLine("Veuillez choisir un coefficient d'agrandissement :");
            int coefAgrandi = Convert.ToInt32(Console.ReadLine());
            MyImage testAgrandi = test.Agrandir(coefAgrandi);
            testAgrandi.From_Image_To_File("testAgrandi.bmp");

            Console.WriteLine("Veuillez choisir un coefficient de réduction :");
            int coefReduit = Convert.ToInt32(Console.ReadLine());
            MyImage testReduit = test.Reduire(coefReduit);
            if (testReduit != null) testReduit.From_Image_To_File("testReduit.bmp");
            else Console.WriteLine("coefficient de réduction impossible");

            /*Console.WriteLine("Header :");
            for (int i = 0; i < 14; i++) Console.Write(test.Image[i] + " ");
            Console.WriteLine("\n\nHeader Info :");
            for (int i = 14; i < 54; i++) Console.Write(test.Image[i] + " ");
            Console.WriteLine("\n\nImage :");
            for (int i = 54; i < test.Image.Length; i += 60)
            {
                for (int j = i; j < i + 60; j++)
                    Console.Write(test.Image[j] + " ");
                Console.WriteLine();
            }*/
        }
    }
}
