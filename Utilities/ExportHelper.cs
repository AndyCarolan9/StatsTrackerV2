#if ANDROID
using Android.Content;
using Android.Provider;
#endif

using SkiaSharp;

namespace StatsTrackerV2.Utilities
{
    public static class ExportHelper
    {
        public static async void ExportImage(string fileName, IScreenshotResult screenshotResult)
        {
            using var stream = await screenshotResult.OpenReadAsync();
            fileName = string.Concat(fileName, ".png");

#if WINDOWS
			var path = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
			var file = Path.Combine(path, fileName);

			await using var fileStream = File.Create(file);
			await stream.CopyToAsync(fileStream);

#elif ANDROID
			var resolver = Android.App.Application.Context.ContentResolver;

			var values = new ContentValues();
			values.Put(MediaStore.IMediaColumns.DisplayName, fileName);
			values.Put(MediaStore.IMediaColumns.MimeType, "image/png");
			values.Put(
			    MediaStore.IMediaColumns.RelativePath,
			    $"{Android.OS.Environment.DirectoryPictures}/YourApp");

			var uri = resolver.Insert(
			    MediaStore.Images.Media.ExternalContentUri,
			    values);

			if (uri != null)
			{
			    await using var output = resolver.OpenOutputStream(uri);
			    if (output != null)
			    {
			        await stream.CopyToAsync(output);
			    }
			}
#endif
        }

		public static async void ExportImage(string fileName, SKImage imageData)
		{
            using var data = imageData.Encode(SKEncodedImageFormat.Png, 100);
            if (data == null)
            {
                return;
            }

            fileName = string.Concat(fileName, ".png");

#if WINDOWS
		    var path = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
		    var file = Path.Combine(path, fileName);

		    await using var fileStream = File.OpenWrite(file);
		    data.SaveTo(fileStream);

#elif ANDROID
            var resolver = Android.App.Application.Context.ContentResolver;

            var values = new ContentValues();
            values.Put(MediaStore.IMediaColumns.DisplayName, fileName);
            values.Put(MediaStore.IMediaColumns.MimeType, "image/png");
            values.Put(
                MediaStore.IMediaColumns.RelativePath,
                $"{Android.OS.Environment.DirectoryPictures}/YourApp");

            var uri = resolver.Insert(
                MediaStore.Images.Media.ExternalContentUri,
                values);

            if (uri != null)
            {
                await using var output = resolver.OpenOutputStream(uri);
                if (output != null)
                {
                    data.SaveTo(output);
                }
            }
#endif
        }
    }
}
