using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using System.Threading.Channels;
using TrickingLibrary.Api.BackgroundServices.Messages;
using TrickingLibrary.Data;

namespace TrickingLibrary.Api.BackgroundServices
{
    public class VideoEditingBackgroundService : BackgroundService
    {
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<VideoEditingBackgroundService> _logger;
        private readonly ChannelReader<EditVideoMessage> _channelReader;
        private readonly IServiceProvider _serviceProvider;

        public VideoEditingBackgroundService(
            IWebHostEnvironment env, 
            ILogger<VideoEditingBackgroundService> logger, 
            Channel<EditVideoMessage> channel,
            IServiceProvider serviceProvider
        ) 
        {
            _env = env;
            _logger = logger;
            _channelReader = channel.Reader;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (await _channelReader.WaitToReadAsync(stoppingToken))
            {
                EditVideoMessage? editVideoMessage = null;

                try
                {
                    editVideoMessage = await _channelReader.ReadAsync(stoppingToken);
                    
                    var workingDirectory = Path.Combine(_env.WebRootPath, "videos");
                    var inputPath = Path.Combine(workingDirectory, editVideoMessage.Input);
                    var outputPath = Path.Combine(workingDirectory, editVideoMessage.Output);
                    var processFilePath = Path.Combine(_env.ContentRootPath, "Resources", "FFmpeg", "ffmpeg.exe");
                    var arguments = $"-y -i {inputPath} -an -vf scale=540x380 {outputPath}";

                    var processStartInfo = new ProcessStartInfo {
                        FileName = processFilePath,
                        WorkingDirectory = workingDirectory,
                        Arguments = arguments,
                        CreateNoWindow = true,
                        UseShellExecute = false,
                    };

                    using var process = new Process { StartInfo = processStartInfo };
                    process.Start();
                    process.WaitForExit();

                    using var serviceScope = _serviceProvider.CreateScope();
                    var appDbContext = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();
                    var submission = appDbContext.Submissions.FirstOrDefault(s => s.Id.Equals(editVideoMessage.SubmissionId));

                    // TODO - Clean up on error.

                    if (submission != null)
                    {
                        submission.Video = editVideoMessage.Output;
                        submission.VideoProcessed = true;
                        await appDbContext.SaveChangesAsync(stoppingToken);
                    }

                    // TODO - Clean up on success.
                }
                catch (Exception ex)
                {
                    _logger?.LogError(ex, "Video processing failed for '{InputPath}'.", editVideoMessage?.Input);
                }
            }
        }
    }
}
