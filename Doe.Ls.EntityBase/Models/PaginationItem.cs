namespace Doe.Ls.EntityBase.Models
{
    public class PaginationItem
    {
        public Pagination PaginationRef { get; set; }

        public int Start { get; private set; }

        public int Length { get; private set; }

        public PaginationItem(int start, int length, Pagination paginationRef)
        {
            Start = start;
            Length = length;
            PaginationRef = paginationRef;
        }

        public override string ToString()
        {
            return $"start: {Start}, length: {Length} ";
        }
    }
}