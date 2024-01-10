<Query Kind="Program" />

void Main()
{
	// -------------------------------------------------------------------------------
	// FFmpeg Command-Line Argument Information
	// -------------------------------------------------------------------------------
	// 
	// Usage: 
	// ffmpeg [options] [[infile options] -i infile]... {[outfile options] outfile}...
	// 
	// *  -an: Disable audio.
	// *  -i: Specifies the input file. 
	//        Can be regular file, pipe, network stream, or grabbing device. 
	//        Can be of any format that FFmpeg supports.
	// *  -ss <time_off>: Set the start time offset.
	// *  -vf <filter_graph>: Set video filters.
	// *  -vframes <number>: Set the number of video frames to output.
	// *  -y: Overwrite output files.
	// -------------------------------------------------------------------------------

	// User-Specific Constants
	const string InputVideoFileName = "_test_video_01.mp4";
	const string ProjectRoot = @"C:\Users\phili\source\repos\TrickingLibrary\TrickingLibrary.Api";

	// FFmpeg Output File Constants
	const string ConvertedFileExtension = ".mp4";
	const string ConvertedFilePrefix = "conv_";
	const string ThumbnailFileExtension = ".png";
	const string ThumbnailFilePrefix = "thumb_";
	
	// ProcessStartInfo Values
	var ffmpegWorkingDirectory = $@"{ProjectRoot}\Resources\FFmpeg";
	var ffmpegFileName = $@"{ffmpegWorkingDirectory}\ffmpeg.exe";

	// FFmpeg Command-Line Values
	var videosDirectory = $@"{ProjectRoot}\wwwroot\videos";
	var inputPath = $@"{videosDirectory}\{InputVideoFileName}";
	var inputVideoFileNameWithoutExtension = Path.GetFileNameWithoutExtension(InputVideoFileName);
	var baseArguments = $"-y -i {inputPath}";
	
	string arguments;

	// File Conversion Process
	var convertedFileName = $"{ConvertedFilePrefix}{inputVideoFileNameWithoutExtension}{ConvertedFileExtension}";
	var convertedOutputPath = $@"{videosDirectory}\{convertedFileName}";
	arguments = $"{baseArguments} -an -vf scale=540x380 {convertedOutputPath}";

	using var fileConversionProcess = new Process {
		StartInfo = new ProcessStartInfo {
			FileName = ffmpegFileName,
			Arguments = arguments,
			CreateNoWindow = true,
			UseShellExecute = false,
			WorkingDirectory = ffmpegWorkingDirectory
		}
	};
	
	fileConversionProcess.Start();
	fileConversionProcess.WaitForExit();

	// Thumbnail Generation Process
	var thumbnailFileName = $"{ThumbnailFilePrefix}{inputVideoFileNameWithoutExtension}{ThumbnailFileExtension}";
	var thumbnailOutputPath = $@"{videosDirectory}\{thumbnailFileName}";
	arguments = $"{baseArguments} -ss 00:00:00 -vframes 1 {thumbnailOutputPath}";

	using var thumbnailGenerationProcess = new Process {
		StartInfo = new ProcessStartInfo {
			FileName = ffmpegFileName,
			Arguments = arguments,
			CreateNoWindow = true,
			UseShellExecute = false,
			WorkingDirectory = ffmpegWorkingDirectory
		}
	};

	thumbnailGenerationProcess.Start();
	thumbnailGenerationProcess.WaitForExit();
}
