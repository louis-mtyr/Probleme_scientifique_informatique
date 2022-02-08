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
        static public void NoirEtBlanc(MyImage image)
        {
            for (int i=0; i<image.Image.Length; i++)
            {
                image.Image[i].R = (byte)(255 - image.Image[i].R);
                image.Image[i].G = (byte)(255 - image.Image[i].R);
                image.Image[i].B = (byte)(255 - image.Image[i].R);
            }
        }

        static public void Inverse(MyImage image)
        {
            for (int i = 0; i < image.Image.Length; i++)
            {
                image.Image[i].R = (byte)(255 - image.Image[i].R);
                image.Image[i].G = (byte)(255 - image.Image[i].G);
                image.Image[i].B = (byte)(255 - image.Image[i].B);
            }
        }

        static void Main(string[] args)
        {
            MyImage test = new MyImage("coco.bmp");
            Console.WriteLine("type d'image : " + test.TypeImage);
            Console.WriteLine("taille du fichier : " + test.TailleFichier);
            Console.WriteLine("taille offset : " + test.TailleOffset);
            Console.WriteLine("hauteur de l'image : " + test.HauteurImage);
            Console.WriteLine("largeur de l'image : " + test.LargeurImage);
            Console.WriteLine("nombre de bits couleur : " + test.NbBitsCouleur);
            /*for (int i=0; i<test.Image.Length; i++)
            {
                if ((i % (test.LargeurImage * 3) == 0) && i!=0) Console.WriteLine();
                Console.Write(test.Image[i] + " ");
            }*/
            test.From_Image_To_File("testingTD2.bmp");
            Console.WriteLine();
            Console.WriteLine();
            //Process.Start("testingTD2.bmp");
            NoirEtBlanc(test);
            test.From_Image_To_File("testingTD2_NoirEtBlanc.bmp");
            Inverse(test);
            test.From_Image_To_File("testingTD2Inverse.bmp");
            /*Console.WriteLine("type d'image : " + test.TypeImage);
            Console.WriteLine("taille du fichier : " + test.TailleFichier);
            Console.WriteLine("taille offset : " + test.TailleOffset);
            Console.WriteLine("hauteur de l'image : " + test.HauteurImage);
            Console.WriteLine("largeur de l'image : " + test.LargeurImage);
            Console.WriteLine("nombre de bits couleur : " + test.NbBitsCouleur);
            for (int i=0; i<test.Image.Length; i++)
            {
                if ((i % (test.LargeurImage * 3) == 0) && i!=0) Console.WriteLine();
                Console.Write(test.Image[i] + " ");
            }
            Console.WriteLine("Header :");
            for (int i = 0; i < 14; i++) Console.Write(test.Image[i] + " ");
            Console.WriteLine("\n\nHeader Info :");
            for (int i = 14; i < 54; i++) Console.Write(test.Image[i] + " ");
            Console.WriteLine("\n\nImage :");
            for (int i = 54; i < test.Image.Length; i += 60)
            {
                for (int j = i; j < i + 60; j++)
                    Console.Write(test.Image[j] + " ");
                Console.WriteLine();
            }
            test.From_Image_To_File("testingTD2Inverse.bmp");

            for (int i = 0; i < 4; i++)
            {
                Console.Write(test.Convert_Int_To_Endian(800)[i] + " ");
            }*/
        }
    }
}
