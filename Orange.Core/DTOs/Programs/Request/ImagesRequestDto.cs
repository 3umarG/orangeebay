using Microsoft.AspNetCore.Http;

namespace Orange_Bay.DTOs.Programs.Request;

public record ImagesRequestDto(
    List<IFormFile> Images
);