using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Orange_Bay.DTOs;
using Orange_Bay.Utils;

namespace Orange.EF.Services;

public class ImageSaver
{
    private readonly AppSettings _appSettings;

    public ImageSaver(IOptions<AppSettings> appSettings)
    {
        _appSettings = appSettings.Value;
    }

    public async Task<string> GenerateImageUrl(IFormFile imageFile, string path)
    {
        var imageTitle = Guid.NewGuid().ToString();
        var directoryPath = Path.Combine("images", path);
        var filePath = Path.Combine(directoryPath, imageTitle);

        var extension = imageFile.FileName.Split(".").Last();

        if (!Directory.Exists(Path.Combine("wwwroot", directoryPath)))
        {
            Directory.CreateDirectory(Path.Combine("wwwroot", directoryPath));
        }

        await using var stream =
            new FileStream(Path.Combine(AppUtils.RootPath, filePath).Replace(" ", "") + "." + extension,
                FileMode.Create);
        await imageFile.CopyToAsync(stream);

        return Path.Combine(_appSettings.ImageBaseUrl, filePath)
            .Replace("\\", "/")
            .Replace(" ", "") + "." + extension;
    }
}