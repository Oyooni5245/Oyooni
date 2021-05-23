namespace Oyooni.Server.Dtos.AI
{
    /// <summary>
    /// Represents a data transfer object containing the answer of a question on an image
    /// </summary>
    public class VQAResultDto
    {
        /// <summary>
        /// The answer to the question on the image
        /// </summary>
        public string Answer { get; set; }
        
        /// <summary>
        /// Default constructor
        /// </summary>
        public VQAResultDto() { }

        /// <summary>
        /// Constructs a new instance of the <see cref="VQAResultDto"/> class using the passed parameters
        /// </summary>
        public VQAResultDto(string answer) => Answer = answer;
    }
}
