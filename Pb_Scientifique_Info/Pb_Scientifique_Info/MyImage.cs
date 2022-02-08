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
        private byte[] fichierComplet;

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
            set { this.hauteurImage = value; }
        }

        public int LargeurImage
        {
            get { return this.largeurImage; }
            set { this.largeurImage = value; }
        }

        public int NbBitsCouleur
        {
            get { return this.nbBitsCouleur; }
        }

        public byte[] Image
        {
            get { return this.image; }
            set { this.image = value; }
        }

        public byte[] FichierComplet
        {
            get { return this.fichierComplet; }
        }

        public MyImage(string typeImage, int tailleFichier, int tailleOffset, int hauteurImage, int largeurImage, int nbBitsCouleur, byte[] image, byte[] fichierComplet)
        {
            this.typeImage = typeImage;
            this.tailleFichier = tailleFichier;
            this.tailleOffset = tailleOffset;
            this.hauteurImage = hauteurImage;
            this.largeurImage = largeurImage;
            this.nbBitsCouleur = nbBitsCouleur;
            this.image = image;
            this.fichierComplet = fichierComplet;
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

            this.fichierComplet = tab;
        }

        public void From_Image_To_File(string file)
        {
            byte[] nouveauFichier = new byte[this.tailleFichier];
            nouveauFichier[0] = Convert.ToByte(66);
            nouveauFichier[1] = Convert.ToByte(77);
            byte[] tailleFichierEndian = Convert_Int_To_Endian(this.tailleFichier);
            for (int i = 2; i < 6; i++) nouveauFichier[i] = tailleFichierEndian[i - 2];
            for (int i = 6; i < 14; i++) nouveauFichier[i] = Convert.ToByte(0);
            nouveauFichier[10] = Convert.ToByte(54);
            for (int i = 14; i < 18; i++) nouveauFichier[i] = Convert_Int_To_Endian(this.tailleOffset)[i - 14];
            for (int i = 18; i < 22; i++) nouveauFichier[i] = Convert_Int_To_Endian(this.hauteurImage)[i - 18];
            for (int i = 22; i < 26; i++) nouveauFichier[i] = Convert_Int_To_Endian(this.largeurImage)[i - 22];
            nouveauFichier[26] = Convert.ToByte(1);
            nouveauFichier[27] = Convert.ToByte(0);
            for (int i = 28; i < 30; i++) nouveauFichier[i] = Convert_Int_To_Endian(this.nbBitsCouleur)[i - 28];
            for (int i = 30; i < 34; i++) nouveauFichier[i] = Convert.ToByte(0);
            for (int i = 34; i < 38; i++) nouveauFichier[i] = Convert_Int_To_Endian(this.hauteurImage * this.largeurImage * 3)[i - 34];
            for (int i = 38; i < 54; i++) nouveauFichier[i] = Convert.ToByte(0);
            for (int i = 54; i < nouveauFichier.Length; i++) nouveauFichier[i] = this.Image[i - 54];
            File.WriteAllBytes(file, nouveauFichier);
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
            if (val > Math.Pow(256, 3))
            {
                tab[3] = Convert.ToByte(val / Math.Pow(256, 3));
                val = val % (int)Math.Pow(256, 3);
            }
            if (val > Math.Pow(256, 2))
            {
                tab[2] = Convert.ToByte(val / Math.Pow(256, 2));
                val = val % (int)Math.Pow(256, 2);
            }
            if (val > Math.Pow(256, 1))
            {
                tab[1] = Convert.ToByte(val / Math.Pow(256, 1));
                val = val % (int)Math.Pow(256, 1);
            }
            if (val >= 0) tab[0] = Convert.ToByte(val);
            return tab;
        }
    }
}
