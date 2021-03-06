using System;
using System.Collections.Generic;
using System.Text;

namespace Pb_Scientifique_Info
{
    class Pixel
    {
        private byte r;     //un pixel est un mélange de red green et blue
        private byte g;
        private byte b;

        public Pixel(byte r, byte g, byte b)
        {
            this.r = r;
            this.g = g;
            this.b = b;
        }

        public byte R
        {
            get { return this.r; }
            set { this.r = value; }
        }

        public byte G
        {
            get { return this.g; }
            set { this.g = value; }
        }

        public byte B
        {
            get { return this.b; }
            set { this.b = value; }
        }
    }
}
