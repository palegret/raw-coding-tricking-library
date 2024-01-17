using Microsoft.AspNetCore.StaticFiles;

namespace TrickingLibrary.Api.Helpers
{
    public class VideoHelper
    {
        // TODO: Move these to appsettings.json.
        public const string VideoDirectory = "videos";
        public const string TempFilePrefix = "temp_";
        public const string ConvertedFilePrefix = "conv_";
        public const string ConvertedFileExtension = ".mp4";
		public const string ThumbnailFilePrefix = "thumb_";
        public const string ThumbnailFileExtension = ".jpg";
		public const string FallbackMimeType = "video/*";

        private readonly IWebHostEnvironment _env;

        public VideoHelper(IWebHostEnvironment env)
        {
            _env = env;
        }

        public string WorkingDirectory => _env.WebRootPath;

        public string VideoWorkingDirectory =>
            _env.IsDevelopment() ? Path.Combine(WorkingDirectory, VideoDirectory) : string.Empty;

        public string VideoSavePath(string? fileName) => 
            _env.IsDevelopment() && !string.IsNullOrWhiteSpace(fileName) 
                ? Path.Combine(VideoWorkingDirectory, fileName) : string.Empty;

		public string FFmpegPath =>
			Path.Combine(_env.ContentRootPath, "Resources", "FFmpeg", "ffmpeg.exe");

        public bool IsTemporaryFile(string? fileName) =>
            fileName?.StartsWith(TempFilePrefix) ?? false;

		public bool IsConvertedFile(string? fileName) =>
			fileName?.StartsWith(ConvertedFilePrefix) ?? false;

		public bool IsThumbnailFile(string? fileName) =>
        	fileName?.StartsWith(ThumbnailFilePrefix) ?? false;

		public string GetTemporaryVideoFileName(string fileName) => 
            $"{TempFilePrefix}{DateTime.Now.Ticks}{Path.GetExtension(fileName)}";

		public string GetConvertedVideoFileName() =>
			$"{ConvertedFilePrefix}{DateTime.Now.Ticks}{ConvertedFileExtension}";

		public string GetThumbnailFileName() =>
	        $"{ThumbnailFilePrefix}{DateTime.Now.Ticks}{ThumbnailFileExtension}";

		public bool TemporaryVideoFileExists(string? fileName) =>
            IsTemporaryFile(fileName) && File.Exists(VideoSavePath(fileName));

        public bool ConvertedVideoFileExists(string? fileName) =>
            IsConvertedFile(fileName) && File.Exists(VideoSavePath(fileName));

		public bool ThumbnailFileExists(string? fileName) =>
			IsThumbnailFile(fileName) && File.Exists(VideoSavePath(fileName));

		public string GetVideoResourceMimeType(string fileName)
        {
            var fileExtensionContentTypeProvider = new FileExtensionContentTypeProvider();
            var gotMimeType = fileExtensionContentTypeProvider.TryGetContentType(fileName, out var mimeType);

            if (!gotMimeType || string.IsNullOrEmpty(mimeType))
                mimeType = FallbackMimeType;

            return mimeType;
        }

        public async Task<string> SaveTemporaryVideo(IFormFile video)
        {
            var temporaryFileName = GetTemporaryVideoFileName(video.FileName);
            var videoSavePath = VideoSavePath(temporaryFileName);

            if (string.IsNullOrWhiteSpace(videoSavePath))
                return string.Empty;

            await using var fileStream = new FileStream(videoSavePath, FileMode.Create, FileAccess.Write);
            await video.CopyToAsync(fileStream);

            return temporaryFileName;
        }

        public void DeleteTemporaryVideo(string? fileName)
        {
            if (!IsTemporaryFile(fileName) || !TemporaryVideoFileExists(fileName))
                return;

            var videoSavePath = VideoSavePath(fileName);
            File.Delete(videoSavePath);
        }
    }
}
