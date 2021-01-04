using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using ImageMagick;
using Watermelon.NET.Commons;
using Watermelon.NET.Configurations;

namespace Watermelon.NET.Implementation
{
    public class WatermelonImages
    {
        private static readonly HttpClient Client = new HttpClient();
        
        private static readonly MagickColor Dark = MagickColor.FromRgb(36, 36, 36);

        private static readonly string RobotoLight =
            Path.Combine(Environment.CurrentDirectory, "Assets/Fonts/Roboto-Light.ttf");
        private static readonly string RobotoMedium =
            Path.Combine(Environment.CurrentDirectory, "Assets/Fonts/Roboto-Medium.ttf");

        private static readonly string RobotoRegular =
            Path.Combine(Environment.CurrentDirectory, "Assets/Fonts/Roboto-Regular.ttf");

        public static async Task<Stream> GenerateWelcomeImageAsync(DiscordMember member)
        {
            using var image = new MagickImage(Dark, 1100, 500);
            await AddAvatarAsync(image, member);
            
            var imageStream = new MemoryStream();
            await image.WriteAsync(imageStream, MagickFormat.Png);
            imageStream.Position = 0;
            return imageStream;
        }

        private static async Task AddAvatarAsync(MagickImage image, DiscordUser user)
        {
            await using var avatarStream = await Client.GetStreamAsync(user.GetAvatarUrl(ImageFormat.Auto));
            using var avatarImage = new MagickImage(avatarStream);
            avatarImage.Resize(new MagickGeometry
            {
                Width = 300,
                Height = 300
            });
            
            using var avatarLayer = new MagickImage(MagickColors.Transparent, 300, 300);
            avatarLayer.Draw(new Drawables()
                .RoundRectangle(0, 0, avatarLayer.Width, avatarLayer.Height, 500, 500)
                .FillColor(MagickColors.White));
            avatarLayer.Composite(avatarImage, CompositeOperator.Atop);

            image.Draw(new DrawableComposite(image.Width / 2 - 150, 50, CompositeOperator.Over, avatarLayer));
        }
    }
}