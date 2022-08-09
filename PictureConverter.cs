using System.Drawing;

namespace NeuralNetworks
{
    public class PictureConverter
    {
        public int Boundery { get; set; } = 128;
        public int Width { get; set; }
        public int Height { get; set; }
        public List<int> Convert(string path)
        {
            var result = new List<int>();
            var image = new Bitmap(path);
            Height = image.Height;
            Width = image.Width;

            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    var pixel = image.GetPixel(x, y);
                    var value = Brightness(pixel);
                    result.Add(value);
                }
            }
            return result;
        }
        public void Save(string path, List<int> pixels)
        {
            var image = new Bitmap(Width, Height);

            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    var color = pixels[y * Width + x] == 1 ? Color.White : Color.Black;
                    image.SetPixel(x, y, color);
                }
            }
            image.Save(path);
        }
        private int Brightness(Color pixel)
        {
            var result = 0.299 * pixel.R + 0.587 * pixel.G + 0.114 * pixel.B;

            return result < Boundery ? 0 : 1;
        }
    }
}
