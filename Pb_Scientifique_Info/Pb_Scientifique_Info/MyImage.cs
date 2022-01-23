using System;
using System.Collections.Generic;
using System.Text;

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

        }

        public void From_Image_To_File(string file)
        {

        }

        public int Convert_Endian_To_Int(byte[] tab)
        {
            return 0;
        }

        public byte[] Convert_Int_To_Endian(int val)
        {
            return null;
        }
    }
}
