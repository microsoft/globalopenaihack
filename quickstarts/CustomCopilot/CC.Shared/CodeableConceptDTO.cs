namespace CC.Shared
{
    public class CodeableConceptDTO
    {
        public List<CodingDTO> Coding { get; set; } = new List<CodingDTO>();

        public string Text { get; set; } = string.Empty;
    }
}
