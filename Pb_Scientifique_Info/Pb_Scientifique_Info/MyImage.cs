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
        private Pixel[] image;
        //private byte[] fichierComplet;

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

        public Pixel[] Image
        {
            get { return this.image; }
            set { this.image = value; }
        }

        /*public byte[] FichierComplet
        {
            get { return this.fichierComplet; }
            set { this.fichierComplet = value; }
        }*/

        public MyImage(string typeImage, int tailleFichier, int tailleOffset, int hauteurImage, int largeurImage, int nbBitsCouleur, Pixel[] image, byte[] fichierComplet)
        {
            this.typeImage = typeImage;
            this.tailleFichier = tailleFichier;
            this.tailleOffset = tailleOffset;
            this.hauteurImage = hauteurImage;
            this.largeurImage = largeurImage;
            this.nbBitsCouleur = nbBitsCouleur;
            this.image = image;
            //this.fichierComplet = fichierComplet;
        }

        public MyImage(string myfile)
        {
            byte[] tab = File.ReadAllBytes(myfile);
            this.typeImage = "Pas BitMap";
            if (tab[0] == (byte)66) if (tab[1] == (byte)77) this.typeImage = "BitMap";    //si les premiers nombres du header sont 66 et 77, alors c'est du .BMP

            byte[] tailleFichierEndian = new byte[4];                               //calcul de la taille totale du fichier (image + header)
            for (int i = 2; i < 6; i++) tailleFichierEndian[i-2] = tab[i];
            this.tailleFichier = Convert_Endian_To_Int(tailleFichierEndian);

            byte[] tailleOffsetEndian = new byte[4];                                 //taille du header info
            for (int i = 14; i < 18; i++) tailleOffsetEndian[i - 14] = tab[i];
            this.tailleOffset = Convert_Endian_To_Int(tailleOffsetEndian);

            byte[] largeurImageEndian = new byte[4];                                //taille de la largeur de l'image
            for (int i = 18; i < 22; i++) largeurImageEndian[i - 18] = tab[i];
            this.largeurImage = Convert_Endian_To_Int(largeurImageEndian);

            byte[] hauteurImageEndian = new byte[4];                                    //calcul taille de la hauteur de l'image
            for (int i = 22; i < 26; i++) hauteurImageEndian[i - 22] = tab[i];
            this.hauteurImage = Convert_Endian_To_Int(hauteurImageEndian);

            byte[] nbBitsCouleurEndian = new byte[2];                               //profondeur de codage de la couleur
            for (int i = 28; i < 30; i++) nbBitsCouleurEndian[i - 28] = tab[i];
            this.nbBitsCouleur = Convert_Endian_To_Int(nbBitsCouleurEndian);

            Pixel[] limage = new Pixel[hauteurImage * largeurImage];            //remplissage de l'attribut du tableau de pixel
            int k = 0;
            for (int i = 54; i < tab.Length-2; i+=3)
            {
                limage[k] = new Pixel(0, 0, 0);
                limage[k].B = tab[i];
                limage[k].G = tab[i + 1];
                limage[k].R = tab[i + 2];
                k++;
            }
            this.image = limage;
            this.myfile = myfile;
            //this.fichierComplet = tab;
        }

        public void From_Image_To_File(string file)
        {
            byte[] nouveauFichier = new byte[this.tailleFichier];           //début recopiage header + header info
            nouveauFichier[0] = Convert.ToByte(66);
            nouveauFichier[1] = Convert.ToByte(77);
            byte[] tailleFichierEndian = Convert_Int_To_Endian(this.tailleFichier);
            for (int i = 2; i < 6; i++) nouveauFichier[i] = tailleFichierEndian[i - 2];
            for (int i = 6; i < 14; i++) nouveauFichier[i] = Convert.ToByte(0);
            nouveauFichier[10] = Convert.ToByte(54);
            for (int i = 14; i < 18; i++) nouveauFichier[i] = Convert_Int_To_Endian(this.tailleOffset)[i - 14];
            for (int i = 18; i < 22; i++) nouveauFichier[i] = Convert_Int_To_Endian(this.largeurImage)[i - 18];
            for (int i = 22; i < 26; i++) nouveauFichier[i] = Convert_Int_To_Endian(this.hauteurImage)[i - 22];
            nouveauFichier[26] = Convert.ToByte(1);
            nouveauFichier[27] = Convert.ToByte(0);
            for (int i = 28; i < 30; i++) nouveauFichier[i] = Convert_Int_To_Endian(this.nbBitsCouleur)[i - 28];
            for (int i = 30; i < 34; i++) nouveauFichier[i] = Convert.ToByte(0);
            for (int i = 34; i < 38; i++) nouveauFichier[i] = Convert_Int_To_Endian(this.hauteurImage * this.largeurImage * 3)[i - 34];
            for (int i = 38; i < 54; i++) nouveauFichier[i] = Convert.ToByte(0); //fin recopiage header + header info
            int k = 0;
            for (int i = 54; i < nouveauFichier.Length-2; i+=3)
            {
                nouveauFichier[i] = this.image[k].B;
                nouveauFichier[i+1] = this.image[k].G;
                nouveauFichier[i+2] = this.image[k].R;
                k++;
            }
            File.WriteAllBytes(file, nouveauFichier);
        }

        public int Convert_Endian_To_Int(byte[] tab)        //Convertir du base 256 little endian en base 10
        {
            double res = 0;
            for (int i=0; i<tab.Length; i++)
            {
                res += tab[i]*Math.Pow(256, i);
            }
            return (int)res;
        }

        public byte[] Convert_Int_To_Endian(int val)        //Convertir du base 10 en base 256 en little endian
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
