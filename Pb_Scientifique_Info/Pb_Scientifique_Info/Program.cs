using System;
using System.Diagnostics;

namespace Pb_Scientifique_Info
{
    class Program
    {
        static public void NoirEtBlanc(MyImage image)
        {
            for (int i=0; i<image.Image.Length; i+=3)
            {
                image.Image[i] = Convert.ToByte(255 - (int)image.Image[i]);
                image.Image[i+1] = Convert.ToByte(255 - (int)image.Image[i]);
                image.Image[i+2] = Convert.ToByte(255 - (int)image.Image[i]);
            }
        }

        static public void Inverse(MyImage image)
        {
            for (int i = 0; i < image.Image.Length; i += 3)
            {
                image.Image[i] = Convert.ToByte(255 - (int)image.Image[i]);
                image.Image[i + 1] = Convert.ToByte(255 - (int)image.Image[i+1]);
                image.Image[i + 2] = Convert.ToByte(255 - (int)image.Image[i+2]);
            }
        }

        static void Main(string[] args)
        {
            MyImage test = new MyImage("Test001.bmp");
            Console.WriteLine("type d'image : " + test.TypeImage);
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
            test.From_Image_To_File("testingTD2.bmp");
            Console.WriteLine();
            Console.WriteLine();
            //Process.Start("testingTD2.bmp");
            Inverse(test);
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
            }*/
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
            }
        }
    }
}
