﻿using System;
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
        static public MyImage NoirEtBlanc(MyImage image)
        {
            MyImage nouvelleImage = new MyImage(image.Myfile);
            for (int i=0; i<image.Image.Length; i++)
            {
                nouvelleImage.Image[i].R = (byte)(image.Image[i].B);
                nouvelleImage.Image[i].G = (byte)(image.Image[i].B);
                nouvelleImage.Image[i].B = (byte)(image.Image[i].B);
            }
            return nouvelleImage;
        }

        static public MyImage Inverse(MyImage image)
        {
            MyImage nouvelleImage = new MyImage(image.Myfile);
            for (int i = 0; i < image.Image.Length; i++)
            {
                nouvelleImage.Image[i].R = (byte)(255 - image.Image[i].R);
                nouvelleImage.Image[i].G = (byte)(255 - image.Image[i].G);
                nouvelleImage.Image[i].B = (byte)(255 - image.Image[i].B);
            }
            return nouvelleImage;
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

            /*byte[] tabTest = File.ReadAllBytes("tigre.bmp");
            Console.WriteLine("Header :");
            for (int i = 0; i < 14; i++) Console.Write(tabTest[i] + " ");
            Console.WriteLine("\n\nHeader Info :");
            for (int i = 14; i < 54; i++) Console.Write(tabTest[i] + " ");
            Console.WriteLine("\n\nImage :");
            for (int i = 54; i < tabTest.Length-59; i += 60)
            {
                for (int j = i; j < i + 60; j++)
                    Console.Write(tabTest[j] + " ");
                Console.WriteLine();
            }*/

            /*test.From_Image_To_File("testingTD2Inverse.bmp");

            for (int i = 0; i < 4; i++)
            {
                Console.Write(test.Convert_Int_To_Endian(800)[i] + " ");
            }*/

            test.From_Image_To_File("testingTD2.bmp");
            Console.WriteLine();
            Console.WriteLine();
            //Process.Start("testingTD2.bmp");
            MyImage testNoirEtBlanc = NoirEtBlanc(test);
            testNoirEtBlanc.From_Image_To_File("testingTD2_NoirEtBlanc.bmp");
            MyImage testInverse = Inverse(test);
            testInverse.From_Image_To_File("testingTD2Inverse.bmp");
        }
    }
}
