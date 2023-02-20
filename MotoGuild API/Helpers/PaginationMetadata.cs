namespace MotoGuild_API.Helpers; //Nie twórz nigdy folderu/namespace Helpers bo to jest worek bez dna. Tam będą wszsycy wrzucać cokolwiek i to bedzie bez sensu. Lepiej zrobić osobne foldery dotyczące stronnicowania, mapowania / inicjalizacji, autoryzacji -> Refresh 

public class PaginationMetadata
{
    public PaginationMetadata(int totalCount, int currentPage, int itemsPerPage)
    {
        TotalCount = totalCount;
        CurrentPage = currentPage;
        TotalPages = (int) Math.Ceiling(totalCount / (double) itemsPerPage);
    }

    public int CurrentPage { get; }
    public int TotalCount { get; }
    public int TotalPages { get; }
    public bool HasPrevious => CurrentPage > 1;
    public bool HasNext => CurrentPage < TotalPages;
}
