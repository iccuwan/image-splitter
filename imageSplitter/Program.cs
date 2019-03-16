using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text.RegularExpressions;

namespace imageSplitter
{
	class Program
	{
		private static Image img;
		private static int width,height;
		private static int xSize = 0;
		private static int ySize = 0;
		private static int splitType = 0; // 1 - from px, 2 - from numbers
		private static string saveFormat;

		static void Main(string[] args)
		{
			string input;
			bool quitNow = false;
			Console.WriteLine("Enter image filename:");
			while (!quitNow)
			{
				input = Console.ReadLine();
				if (img == null)
				{
					if (input.Length > 0 && File.Exists(input))
					{
						img = Image.FromFile(input);
						width = img.Size.Width;
						height = img.Size.Height;
						Console.WriteLine("Opening {0}\nWidth: {1}\nHeight: {2}", input, width, height);
						Console.WriteLine("Select split type:\n1 - enter pixels\n2 - enter number of squares");
					}
					else
					{
						Console.WriteLine(input + " not exists! Try again:");
					}
				}
				else if (splitType == 0 || splitType == 2)
				{
					try
					{
						splitType = Convert.ToInt16(input);
					}
					catch
					{
						Console.WriteLine("Wrong enter. Try again:");
					}
					switch (splitType)
					{
						case 1:
							Console.WriteLine("Type 1 selected.\n Enter width in px:");
							break;
						case 2:
							//Console.WriteLine("Type 2 selected.\n Enter width as number of squares:");
							Console.WriteLine("Type 2 selected. It is not available now. Please select type 1");
							break;
						default:
							Console.WriteLine("Wrong type. Try again:");
							splitType = 0;
							break;
					}
				}
				else if (xSize == 0)
				{
					try
					{
						xSize = Convert.ToInt16(input);
					}
					catch
					{
						Console.WriteLine("Wrong enter. Try again:");
					}
					if (xSize > 0)
					{
						Console.WriteLine("Enter height:");
					}
					else
					{
						Console.WriteLine("Wrong width. Try again:");
					}
				}
				else if (ySize == 0)
				{
					try
					{
						ySize = Convert.ToInt16(input);
					}
					catch
					{
						Console.WriteLine("Wrong enter. Try again:");
					}
					if (ySize > 0)
					{
						Console.WriteLine("Enter save format. Example: row-{row}-col-{col}");
					}
					else
					{
						Console.WriteLine("Wrong height. Try again:");
					}
				}
				else if (saveFormat == null)
				{
					saveFormat = input;
					switch (splitType)
					{
						case 1:
							int x = width / xSize;
							int y = height / ySize;
							int startX = xSize;
							int startY = ySize;
							Bitmap imgBitmap = new Bitmap(img);
							for (int i = 0; i < x; i++)
							{
								for (int j = 0; j < y; j++)
								{
									string filename = saveFormat;
									filename = Regex.Replace(filename, "{row}", i.ToString());
									filename = Regex.Replace(filename, "{col}", j.ToString());
									Rectangle rect = new Rectangle(startX - xSize, startY - ySize, xSize, ySize);
									Bitmap splittetImg = imgBitmap.Clone(rect, img.PixelFormat);
									startX += xSize;
									Directory.CreateDirectory("output");
									splittetImg.Save("output/" + filename + ".png", ImageFormat.Png);
									Console.WriteLine("output/{0}.png saved", filename);
									splittetImg.Dispose();
									
								}
								startY += ySize;
								startX = xSize;
							}
							Console.WriteLine("Grid {0}x{1} created. Generated {2} images", x, y, x * y);
							quitNow = true;
							break;
					}
				}
			}
		}
	}
}
