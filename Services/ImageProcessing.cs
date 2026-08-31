using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace TutorBridge.Services
{
    public static class ImageProcessing
    {
        private const int MaxDimension = 800;
        private const int JpegQuality = 80;

        public static async Task<byte[]> ResizeAndEncodeAsync(Stream inputStream)
        {
            using var image = await Image.LoadAsync(inputStream);

            image.Mutate(x => x.Resize(new ResizeOptions
            {
                Mode = ResizeMode.Max, // fits within the box, preserves aspect ratio, never upscales
                Size = new Size(MaxDimension, MaxDimension)
            }));

            using var outputStream = new MemoryStream();
            await image.SaveAsJpegAsync(outputStream, new JpegEncoder { Quality = JpegQuality });
            return outputStream.ToArray();
        }
    }
}