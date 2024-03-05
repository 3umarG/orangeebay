using System.Collections;

namespace Orange_Bay.DTOs.Shared;

public class PaginatedResponseDto<T>
{
    public IEnumerable<T> Items { get; set; }
    public int CurrentPage { get; set; }
    public int Pages { get; set; }
}