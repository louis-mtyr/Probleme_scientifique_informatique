using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Pb_Scientifique_Info
{
    class MyImage
    {
        private string myfile;
        private string typeImage;
        private int tailleFichier;
        private int tailleOffset;
        private int hauteurImage;
        private int largeurImage;
        private int nbBitsCouleur;
        private byte[] image;

        public string Myfile
        {
            get { return this.myfile; }
        }

        public string TypeImage
        {
            get { return this.typeImage; }
        }

        public int TailleFichier
        {
            get { return this.tailleFichier; }
        }

        public int TailleOffset
        {
            get { return this.tailleOffset; }
        }

        public int HauteurImage
        {
            get { return this.hauteurImage; }
        }

        public int LargeurImage
        {
            get { return this.largeurImage; }
        }

        public int NbBitsCouleur
        {
            get { return this.nbBitsCouleur; }
        }

        public byte[] Image
        {
            get { return this.image; }
        }

        public MyImage(string typeImage, int tailleFichier, int tailleOffset, int hauteurImage, int largeurImage, int nbBitsCouleur, byte[] image)
        {
            this.typeImage = typeImage;
            this.tailleFichier = tailleFichier;
            this.tailleOffset = tailleOffset;
            this.hauteurImage = hauteurImage;
            this.largeurImage = largeurImage;
            this.nbBitsCouleur = nbBitsCouleur;
            this.image = image;
        }

        public MyImage(string myfile)
        {
            byte[] tab = File.ReadAllBytes(myfile);
            this.typeImage = "Pas BitMap";
            if (tab[0] == (byte)66) if (tab[1] == (byte)77) this.typeImage = "BitMap";

            byte[] tailleFichierEndian = new byte[4];
            for (int i = 2; i < 6; i++) tailleFichierEndian[i-2] = tab[i];
            this.tailleFichier = Convert_Endian_To_Int(tailleFichierEndian);

            byte[] tailleOffsetEndian = new byte[4];
            for (int i = 14; i < 18; i++) tailleOffsetEndian[i - 14] = tab[i];
            this.tailleOffset = Convert_Endian_To_Int(tailleOffsetEndian);

            byte[] hauteurImageEndian = new byte[4];
            for (int i = 18; i < 22; i++) hauteurImageEndian[i - 18] = tab[i];
            this.hauteurImage = Convert_Endian_To_Int(hauteurImageEndian);

            byte[] largeurImageEndian = new byte[4];
            for (int i = 22; i < 26; i++) largeurImageEndian[i - 22] = tab[i];
            this.largeurImage = Convert_Endian_To_Int(largeurImageEndian);

            byte[] nbBitsCouleurEndian = new byte[2];
            for (int i = 28; i < 30; i++) nbBitsCouleurEndian[i - 28] = tab[i];
            this.nbBitsCouleur = Convert_Endian_To_Int(nbBitsCouleurEndian);

            byte[] limage = new byte[hauteurImage * largeurImage * 3];
            for (int i = 54; i < tab.Length; i++) limage[i - 54] = tab[i];
            this.image = limage;
        }

        public void From_Image_To_File(string file)
        {

        }

        public int Convert_Endian_To_Int(byte[] tab)
        {
            double res = 0;
            for (int i=0; i<tab.Length; i++)
            {
                res += tab[i]*Math.Pow(256, i);
            }
            return (int)res;
        }

        public byte[] Convert_Int_To_Endian(int val)
        {
            byte[] tab = new byte[4];
            tab[3] = (byte)(val % Math.Pow(256, 3));
            tab[2] = (byte)(val % Math.Pow(256, 2));
            tab[1] = (byte)(val % Math.Pow(256, 1));
            tab[0] = (byte)(val % Math.Pow(256, 0));
            return tab;
        }
    }
}
