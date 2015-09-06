using System;

namespace DGCLib_Presenter.VisualizationTypes
{
    public struct ColorF
    {
        public float A;
        public float R;
        public float G;
        public float B;

        public ColorF(byte a, byte r, byte g, byte b)
        {
            A = a / 255f;
            R = r / 255f;
            G = g / 255f;
            B = b / 255f;
        }

        public ColorF(float a, float r, float g, float b)
        {
            A = a;
            R = r;
            G = g;
            B = b;
            ClipAll();
        }

        private void ClipAll()
        {
            A = Clip(A);
            R = Clip(R);
            G = Clip(G);
            B = Clip(B);
        }

        private float Clip(float val)
        {
            return Math.Max(Math.Min(val, 1f), 0f);
        }

        public static ColorF Black
        {
            get
            {
                return new ColorF(1f, 0f, 0f, 0f);
            }
        }

        public static ColorF Gray
        {
            get
            {
                return new ColorF(1f, 0.5f, 0.5f, 0.5f);
            }
        }

        public static ColorF White
        {
            get
            {
                return new ColorF(1f, 1f, 1f, 1f);
            }
        }

        public static ColorF Red
        {
            get
            {
                return new ColorF(1f, 1f, 0f, 0f);
            }
        }

        public static ColorF Green
        {
            get
            {
                return new ColorF(1f, 0f, 1f, 0f);
            }
        }

        public static ColorF Blue
        {
            get
            {
                return new ColorF(1f, 0f, 0f, 1f);
            }
        }

        public ColorF SetAlpha(float alpha)
        {
            this.A = alpha;
            return this;
        }
    }
}