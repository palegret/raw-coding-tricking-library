﻿using System.Diagnostics;
using System.Threading.Channels;
using TrickingLibrary.Api.BackgroundServices.Messages;
using TrickingLibrary.Api.Helpers;
using TrickingLibrary.Data;

namespace TrickingLibrary.Api.BackgroundServices
{
    public class VideoEditingBackgroundService : BackgroundService
    {
        private readonly ILogger<VideoEditingBackgroundService> _logger;
        private readonly ChannelReader<EditVideoMessage> _channelReader;
        private readonly IServiceProvider _serviceProvider;
        private readonly VideoHelper _videoHelper;

        public VideoEditingBackgroundService(
            ILogger<VideoEditingBackgroundService> logger, 
            Channel<EditVideoMessage> channel,
            IServiceProvider serviceProvider,
            VideoHelper videoHelper
        ) 
        {
            _logger = logger;
            _channelReader = channel.Reader;
            _serviceProvider = serviceProvider;
            _videoHelper = videoHelper;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (await _channelReader.WaitToReadAsync(stoppingToken))
            {
                EditVideoMessage? editVideoMessage = null;

                try
                {
                    editVideoMessage = await _channelReader.ReadAsync(stoppingToken);
                    
                    var inputPath = _videoHelper.VideoSavePath(editVideoMessage.Input);
                    var outputPath = _videoHelper.VideoSavePath(editVideoMessage.Output);
                    var arguments = $"-y -i {inputPath} -an -vf scale=540x380 {outputPath}";

                    var processStartInfo = new ProcessStartInfo {
                        FileName = _videoHelper.FFmpegPath,
                        WorkingDirectory = _videoHelper.VideoWorkingDirectory,
                        Arguments = arguments,
                        CreateNoWindow = true,
                        UseShellExecute = false,
                    };

                    using var process = new Process { StartInfo = processStartInfo };
                    process.Start();
                    process.WaitForExit();

                    if (!_videoHelper.ConvertedVideoFileExists(editVideoMessage.Output))
                        throw new Exception("FFmpeg failed to generate converted video.");

                    using var serviceScope = _serviceProvider.CreateScope();
                    var appDbContext = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();

                    var submission = appDbContext.Submissions.FirstOrDefault(s => s.Id.Equals(editVideoMessage.SubmissionId))
                        ?? throw new Exception($"Submission not found for submission ID '{editVideoMessage.SubmissionId}'.");

                    submission.Video = editVideoMessage.Output;
                    submission.VideoProcessed = true;
                    await appDbContext.SaveChangesAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger?.LogError(ex, "Video processing failed for '{Input}'.", editVideoMessage?.Input);
                }
                finally
                {
                    _videoHelper.DeleteTemporaryVideo(editVideoMessage?.Input);
                }
            }
        }
    }
}
