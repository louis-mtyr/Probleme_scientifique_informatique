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

        public Pixel[,] Image
        {
            get { return this.image; }
            set { this.image = value; }
        }

        public byte[] FichierComplet
        {
            get { return this.fichierComplet; }
            set { this.fichierComplet = value; }
        }

        public MyImage(string typeImage, int tailleFichier, int tailleOffset, int hauteurImage, int largeurImage, int nbBitsCouleur, Pixel[,] image)
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

            if (this.typeImage == "BitMap")
            {

                byte[] tailleFichierEndian = new byte[4];                               //calcul de la taille totale du fichier (image + header)
                for (int i = 2; i < 6; i++) tailleFichierEndian[i - 2] = tab[i];
                this.tailleFichier = Convert_Endian_To_Int(tailleFichierEndian);

                byte[] tailleOffsetEndian = new byte[4];                                 //taille du header info
                for (int i = 34; i < 18; i++) tailleOffsetEndian[i - 34] = tab[i];
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

                Pixel[,] limage = new Pixel[hauteurImage, largeurImage];            //remplissage de l'attribut du tableau de pixel
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
                this.fichierComplet = tab;
            }
        }

        public void From_Image_To_File(string file)
        {
            if (this.tailleOffset % 4 == 3)
            {
                this.tailleOffset += 1;
                this.tailleFichier += 1;
            }
            if (this.tailleOffset % 4 == 2)
            {
                this.tailleOffset += 2;
                this.tailleFichier += 2;
            }
            if (this.tailleOffset % 4 == 1)
            {
                this.tailleOffset += 3;
                this.tailleFichier += 3;
            }

            byte[] nouveauFichier = new byte[this.tailleFichier];           //début recopiage header + header info
            nouveauFichier[0] = Convert.ToByte(66);
            nouveauFichier[1] = Convert.ToByte(77);
            byte[] tailleFichierEndian = Convert_Int_To_Endian(this.tailleFichier);
            for (int i = 2; i < 6; i++) nouveauFichier[i] = tailleFichierEndian[i - 2];
            for (int i = 6; i < 14; i++) nouveauFichier[i] = Convert.ToByte(0);
            nouveauFichier[10] = (byte)54;
            for (int i = 11; i < 14; i++) nouveauFichier[i] = (byte)0;
            nouveauFichier[14] = Convert.ToByte(40);
            for (int i = 15; i < 18; i++) nouveauFichier[i] = (byte)0;
            for (int i = 18; i < 22; i++) nouveauFichier[i] = Convert_Int_To_Endian(this.largeurImage)[i - 18];
            for (int i = 22; i < 26; i++) nouveauFichier[i] = Convert_Int_To_Endian(this.hauteurImage)[i - 22];
            nouveauFichier[26] = Convert.ToByte(1);
            nouveauFichier[27] = Convert.ToByte(0);
            for (int i = 28; i < 30; i++) nouveauFichier[i] = Convert_Int_To_Endian(this.nbBitsCouleur)[i - 28];
            for (int i = 30; i < 34; i++) nouveauFichier[i] = Convert.ToByte(0);
            for (int i = 34; i < 38; i++) nouveauFichier[i] = Convert_Int_To_Endian(this.tailleOffset)[i - 34];
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
            this.fichierComplet = nouveauFichier;
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
            if (val >= (256*256*256))
            {
                tab[3] = Convert.ToByte(val / (256*256*256));
                val = val % (256*256*256);
            }
            if (val >= (256*256))
            {
                tab[2] = Convert.ToByte(val / (256*256));
                val = val % (256*256);
            }
            if (val >= 256)
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

        public MyImage NuanceDeGris()            //return l'image en nuance de gris
        {
            MyImage nouvelleImage = new MyImage(this.Myfile);
            for (int i = 0; i < this.Image.GetLength(0); i++)
            {
                for (int j = 0; j < this.Image.GetLength(1); j++)
                {
                    nouvelleImage.Image[i,j].R = (byte)((this.Image[i, j].R + this.Image[i, j].G + this.Image[i, j].B) / 3);
                    nouvelleImage.Image[i,j].G = (byte)((this.Image[i, j].R + this.Image[i, j].G + this.Image[i, j].B) / 3);
                    nouvelleImage.Image[i,j].B = (byte)((this.Image[i, j].R + this.Image[i, j].G + this.Image[i, j].B) / 3);
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
                    if ((this.Image[i, j].R + this.Image[i, j].G + this.Image[i, j].B)/3 < 128)
                    {
                        nouvelleImage.Image[i, j].R = (byte)0;
                        nouvelleImage.Image[i, j].G = (byte)0;
                        nouvelleImage.Image[i, j].B = (byte)0;
                    }
                    else
                    {
                        nouvelleImage.Image[i, j].R = (byte)255;
                        nouvelleImage.Image[i, j].G = (byte)255;
                        nouvelleImage.Image[i, j].B = (byte)255;
                    }
                }
            }
            return nouvelleImage;
        }

        public MyImage MiroirHorizontal()                 //return l'image avec un effet miroir horizontal
        {
            MyImage nouvelleImage = new MyImage(this.myfile);
            for (int i=0; i<this.image.GetLength(0); i++)
            {
                for (int j=0; j<this.image.GetLength(1); j++)
                {
                    nouvelleImage.Image[i,j].R = (this.Image[i, this.largeurImage - j - 1].R);
                    nouvelleImage.Image[i,j].G = (this.Image[i, this.largeurImage - j - 1].G);
                    nouvelleImage.Image[i,j].B = (this.Image[i, this.largeurImage - j - 1].B);
                }
            }
            return nouvelleImage;
        }

        public MyImage MiroirVertical()                 //return l'image avec un effet miroir vertical
        {
            MyImage nouvelleImage = new MyImage(this.myfile);
            for (int i = 0; i < this.image.GetLength(0); i++)
            {
                for (int j = 0; j < this.image.GetLength(1); j++)
                {
                    nouvelleImage.Image[i, j].R = (this.Image[this.hauteurImage - i - 1, j].R);
                    nouvelleImage.Image[i, j].G = (this.Image[this.hauteurImage - i - 1, j].G);
                    nouvelleImage.Image[i, j].B = (this.Image[this.hauteurImage - i - 1, j].B);
                }
            }
            return nouvelleImage;
        }

        public MyImage Rotation(int angleDonne)                     //return une image tournée de -angleDonne- vers la droite
        {
            int angleDegre = angleDonne % 360;
            double angleRadian = (double)angleDegre * Math.PI / 180;
            int newHauteur = (int)(this.hauteurImage * Math.Cos(angleRadian) + this.largeurImage * Math.Sin(angleRadian));
            int newLargeur = (int)(this.largeurImage * Math.Cos(angleRadian) + this.hauteurImage * Math.Cos((Math.PI / 2) - angleRadian));  //ALEDDDDDDD
            int newTaille = newHauteur * newLargeur * 3 + 54;                                                                               //pour l'instant je cherche à créer les bords mais aled enfait
            Pixel[,] newImage = new Pixel[newHauteur, newLargeur];
            for (int i = 0; i < newHauteur; i++) for (int j = 0; j < newLargeur; j++) newImage[i, j] = new Pixel(0, 0, 0);

            for (int i=0; i < (int)(this.largeurImage * Math.Sin(angleRadian))-1; i++)
            {
                for (int j=0; j < (int)(this.largeurImage * Math.Cos(angleRadian))-i-1; j++)
                {
                    newImage[i, j].R = (byte)128;
                    newImage[i, j].G = (byte)128;
                    newImage[i, j].B = (byte)128;
                }
            }

            for (int i=0; i < (int)(this.hauteurImage*Math.Sin((Math.PI/2)-angleRadian)); i++)
            {
                for (int j = newLargeur - 1; j >= (int)(this.hauteurImage * Math.Cos((Math.PI / 2) - angleRadian) + 1) + i; j--)
                {
                    newImage[i, j].R = (byte)128;
                    newImage[i, j].G = (byte)128;
                    newImage[i, j].B = (byte)128;
                }
            }

            for (int i=newHauteur - 1; i >= (int)(this.hauteurImage * Math.Cos(angleRadian)); i--)
            {
                for (int j = 0; j < i - (int)(this.largeurImage * Math.Cos(angleRadian)); j++)
                {
                    newImage[i, j].R = (byte)128;
                    newImage[i, j].G = (byte)128;
                    newImage[i, j].B = (byte)128;
                }
            }

            for (int i = (int)(this.largeurImage * Math.Sin(angleRadian))+1; i < newHauteur; i++)
            {
                for (int j = newLargeur - 1; j >= - i + newHauteur + (int)(this.largeurImage * Math.Sin(angleRadian)); j--)
                {
                    newImage[i, j].R = (byte)128;
                    newImage[i, j].G = (byte)128;
                    newImage[i, j].B = (byte)128;
                }
            }

            MyImage nouvelleImage = new MyImage("BitMap", newTaille, this.TailleOffset, newHauteur, newLargeur, this.NbBitsCouleur, newImage);
            return nouvelleImage;
        }

        public MyImage Reduire(int coefHauteur, int coefLargeur)                    //return l'image réduite -coefHauteur * coefLargeur- fois
        {                                                   //Attention : ne return l'image que si -coef- est un diviseur commun de hauteurImage ou largeurImage
            if (hauteurImage % coefHauteur == 0 && largeurImage % coefLargeur == 0)
            {
                int newTaille = (this.hauteurImage / coefHauteur) * (this.LargeurImage / coefLargeur) * 3 + 54;
                int newHauteur = this.hauteurImage / coefHauteur;
                int newLargeur = this.largeurImage / coefLargeur;
                int newTailleOffset = newHauteur * newLargeur * 3;
                Pixel[,] newImage = new Pixel[newHauteur, newLargeur];
                for (int i = 0; i < newHauteur; i++) for (int j = 0; j < newLargeur; j++) newImage[i, j] = new Pixel(0, 0, 0);

                int x = 0;
                int y = 0;
                for (int i = 0; i < hauteurImage; i += coefHauteur)
                {
                    for (int j = 0; j < largeurImage; j += coefLargeur)
                    {
                        newImage[x, y] = new Pixel(image[i, j].R, image[i, j].G, image[i, j].B);
                        if (y < newLargeur - 1) y++;
                        else
                        {
                            y = 0;
                            x++;
                        }
                    }
                }
                MyImage imageReduite = new MyImage("BitMap", newTaille, newTailleOffset, newHauteur, newLargeur, this.NbBitsCouleur, newImage);
                return imageReduite;
            }
            else return null;
        }

        public MyImage Agrandir(int coefHauteur, int coefLargeur)                       //return l'image aggrandie -coefHauteur * coefLargeur- fois
        {
            int newTaille = this.hauteurImage * coefHauteur * this.LargeurImage * coefLargeur * 3 + 54;
            int newHauteur = this.hauteurImage * coefHauteur;
            int newLargeur = this.largeurImage * coefLargeur;
            int newTailleOffset = newHauteur * newLargeur * 3;
            Pixel[,] newImage = new Pixel[newHauteur, newLargeur];
            for (int i = 0; i < newHauteur; i++) for (int j = 0; j < newLargeur; j++) newImage[i, j] = new Pixel(0, 0, 0);

            int x = 0;
            int y = 0;
            for (int i = 0; i < newHauteur; i += coefHauteur)
            {
                for (int j = 0; j < newLargeur; j += coefLargeur)
                {
                    newImage[i, j] = new Pixel(image[x, y].R, image[x, y].G, image[x, y].B);
                    for (int n = 0; n < coefHauteur; n++)
                    {
                        for (int k = 0; k < coefLargeur; k++)
                        {
                            newImage[i + n, j + k] = new Pixel(image[x, y].R, image[x, y].G, image[x, y].B);
                        }
                    }
                    if (y < largeurImage - 1) y++;
                    else
                    {
                        y = 0;
                        x++;
                    }
                }
            }
            MyImage imageAgrandie = new MyImage("BitMap", newTaille, newTailleOffset, newHauteur, newLargeur, this.NbBitsCouleur, newImage);
            return imageAgrandie;
        }
    }
}
