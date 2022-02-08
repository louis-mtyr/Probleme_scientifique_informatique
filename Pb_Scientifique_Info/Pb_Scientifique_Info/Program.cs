using System;

namespace Pb_Scientifique_Info
{
    class Program
    {
        static void Main(string[] args)
        {
            MyImage test = new MyImage("Test001.bmp");
            /*Console.WriteLine("Header :");
            for (int i = 0; i < 14; i++) Console.Write(test.Image[i] + " ");
            Console.WriteLine("\n\nHeader Info :");
            for (int i = 14; i < 54; i++) Console.Write(test.Image[i] + " ");
            Console.WriteLine("\n\nImage :");
            for (int i = 0; i < test.Image.Length; i += 60)
            {
                for (int j = i; j < i + 60; j++)
                    Console.Write(test.Image[j] + " ");
                Console.WriteLine();
            }*/
            Console.WriteLine(test.TypeImage);
            Console.WriteLine(test.TailleFichier);
            Console.WriteLine(test.TailleOffset);
            Console.WriteLine(test.HauteurImage);
            Console.WriteLine(test.LargeurImage);
            Console.WriteLine(test.NbBitsCouleur);
            for (int i=0; i<test.Image.Length; i++)
            {
                if ((i % (test.LargeurImage * 3) == 0) && i!=0) Console.WriteLine();
                Console.Write(test.Image[i] + " ");
            }
        }
    }
}
