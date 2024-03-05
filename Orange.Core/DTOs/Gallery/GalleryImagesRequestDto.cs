using Microsoft.AspNetCore.Http;

namespace Orange_Bay.DTOs.Gallery;

public record GalleryImagesRequestDto(
    List<IFormFile> Images,
    List<int> ImagesTypeIds);

// public record GalleryImageRequestDto
// (
//     IFormFile Data,
//     int ImageTypeId
// );