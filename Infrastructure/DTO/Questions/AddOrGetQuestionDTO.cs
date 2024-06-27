namespace Infrastructure.DTO.Questions
{
    public class AddOrGetQuestionDTO
    {
        public int Id { get; set; }
        public string Text { get; set; }

        public List<optionInQuestionDTO> Options { get; set; }
    }
    public class optionInQuestionDTO
    {
        public int id { get; set; }
        public string Text { get; set; }
    }
}
