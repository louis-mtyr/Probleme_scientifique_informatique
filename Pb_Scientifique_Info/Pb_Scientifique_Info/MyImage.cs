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
        private Pixel[,] image;
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

        public Pixel[,] Image
        {
            get { return this.image; }
            set { this.image = value; }
        }

        /*public byte[] FichierComplet
        {
            get { return this.fichierComplet; }
            set { this.fichierComplet = value; }
        }*/

        public MyImage(string typeImage, int tailleFichier, int tailleOffset, int hauteurImage, int largeurImage, int nbBitsCouleur, Pixel[,] image, byte[] fichierComplet)
        {
            this.typeImage = typeImage;
            this.tailleFichier = tailleFichier;
            this.tailleOffset = tailleOffset;       //constante
            this.hauteurImage = hauteurImage;
            this.largeurImage = largeurImage;
            this.nbBitsCouleur = nbBitsCouleur;     //cst
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

            Pixel[,] limage = new Pixel[hauteurImage,largeurImage];            //remplissage de l'attribut du tableau de pixel
            int x = 0;
            int y = 0;
            for (int i = 54; i < tab.Length - 2; i += 3)
            {
                limage[x, y] = new Pixel(0, 0, 0);
                limage[x, y].B = tab[i];
                limage[x, y].G = tab[i + 1];
                limage[x, y].R = tab[i + 2];
                if (y < largeurImage - 1) y++;
                else
                {
                    y = 0;
                    x++;
                }
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
            int x = 0;
            int y = 0;
            for (int i = 54; i < nouveauFichier.Length-2; i+=3)             //copie des octets s'occupant de la couleur et remplissage du tableau de pixel en fct
            {
                nouveauFichier[i] = this.image[x, y].B;
                nouveauFichier[i + 1] = this.image[x, y].G;
                nouveauFichier[i + 2] = this.image[x, y].R;
                if (y < largeurImage - 1) y++;
                else
                {
                    y = 0;
                    x++;
                }
            }
            File.WriteAllBytes(file, nouveauFichier);
        }

        public int Convert_Endian_To_Int(byte[] tab)        //Convertir du base 256 little endian en base 10
        {
            int res = 0;
            for (int i=0; i<tab.Length; i++)
            {
                if (i == 0) res += tab[i];
                if (i == 1) res += tab[i] * 256;
                if (i == 2) res += tab[i] * 256 * 256;
                if (i == 3) res += tab[i] * 256 * 256 * 256;
            }
            return res;
        }

        public byte[] Convert_Int_To_Endian(int val)        //Convertir du base 10 en base 256 en little endian
        {
            byte[] tab = new byte[4];
            if (val > (256*256*256))
            {
                tab[3] = Convert.ToByte(val / 256*256*256);
                val = val % (256*256*256);
            }
            if (val > (256*256))
            {
                tab[2] = Convert.ToByte(val / (256*256));
                val = val % (256*256);
            }
            if (val > 256)
            {
                tab[1] = Convert.ToByte(val / 256);
                val = val % 256;
            }
            if (val >= 0) tab[0] = Convert.ToByte(val);
            return tab;
        }
        public MyImage Inverse()                        //return l'image avec les coleurs inversés en fct du spectre
        {
            MyImage nouvelleImage = new MyImage(this.Myfile);
            for (int i = 0; i < this.Image.GetLength(0); i++)
            {
                for (int j = 0; j < this.Image.GetLength(1); j++)
                {
                    nouvelleImage.Image[i,j].R = (byte)(255 - this.Image[i,j].R);
                    nouvelleImage.Image[i,j].G = (byte)(255 - this.Image[i,j].G);
                    nouvelleImage.Image[i,j].B = (byte)(255 - this.Image[i,j].B);
                }
            }
            return nouvelleImage;
        }

        public MyImage NoirEtBlanc()            //return l'image en noir et blanc
        {
            MyImage nouvelleImage = new MyImage(this.Myfile);
            for (int i = 0; i < this.Image.GetLength(0); i++)
            {
                for (int j = 0; j < this.Image.GetLength(1); j++)
                {
                    nouvelleImage.Image[i,j].R = (byte)(this.Image[i,j].B);
                    nouvelleImage.Image[i,j].G = (byte)(this.Image[i,j].B);
                    nouvelleImage.Image[i,j].B = (byte)(this.Image[i,j].B);
                }
            }
            return nouvelleImage;
        }

        public MyImage Miroir()
        {
            MyImage nouvelleImage = new MyImage(this.myfile);
            int k = 1;
            for (int i=0; i<this.image.GetLength(0); i++)
            {
                for (int j=0; j<this.image.GetLength(1); j++)
                {
                    nouvelleImage.Image[i,j].R = (this.Image[i, this.largeurImage - j - 1].R);
                    nouvelleImage.Image[i,j].G = (this.Image[i, this.largeurImage - j - 1].G);
                    nouvelleImage.Image[i,j].B = (this.Image[i, this.largeurImage - j - 1].B);
                }
                k++;
            }
            return nouvelleImage;
        }

        public MyImage Rotation()
        {
            MyImage nouvelleImage = new MyImage(this.Myfile);



            return nouvelleImage;
        }





        public MyImage Agrandir(int coef)
        {
            int newTaille = this.hauteurImage * coef * this.LargeurImage *coef *3+ 54;
            int newHauteur = this.hauteurImage * coef;
            int newLargeur = this.largeurImage * coef;
            Pixel[] newImage = new Pixel[newHauteur * newLargeur];

            for (int i = 0; i < newImage.Length; i++) newImage[i] = new Pixel(0, 0, 0);
            int k = 0;
            for (int i = 0; i < newImage.Length; i+= coef)
            {
                
                
                for (int j = 0; j < coef; j++)
                {
                    newImage[i + j + newLargeur * j] = new Pixel(this.image[k].R, this.image[k].G, this.image[k].B);
                    for (int n = 0; n < coef; n++)
                    {
                        newImage[i + newLargeur * j + n ] = new Pixel(this.image[k].R, this.image[k].G, this.image[k].B);
                        newImage[i + j + newLargeur * n] = new Pixel(this.image[k].R, this.image[k].G, this.image[k].B);
                        
                    }
                    
                }
                k++;
            }
                  
            MyImage imageAgrandie = new MyImage("BM", newTaille, this.TailleOffset, newHauteur, newLargeur, this.NbBitsCouleur, newImage);



            return imageAgrandie;
        }

        public MyImage Agrandirv2(int coef)
        {
            int newTaille = this.hauteurImage * coef * this.LargeurImage * coef * 3 + 54;
            int newHauteur = this.hauteurImage * coef;
            int newLargeur = this.largeurImage * coef;
            Pixel[] newImage = new Pixel[newHauteur * newLargeur];
            for (int i = 0; i < newImage.Length; i++) newImage[i] = new Pixel(0, 0, 0);
            int n = 0;
            for(int i = 0; i < this.image.Length; i++)
            {
                for(int j = 0; j < coef; j++)
                {
                    for(int k = 0; k < coef; k++)
                    {
                        newImage[n*coef + (i + j + k * newLargeur)] = this.image[i];
                    }
                }
                n++;
            }

            MyImage imageAgrandie = new MyImage("BM", newTaille, this.TailleOffset, newHauteur, newLargeur, this.NbBitsCouleur, newImage);
            return imageAgrandie;
        }
        
        


    }
}
