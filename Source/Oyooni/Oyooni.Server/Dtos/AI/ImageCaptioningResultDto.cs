namespace Oyooni.Server.Dtos.AI
{
    /// <summary>
    /// Represents a data transfer object containing captioning results
    /// </summary>
    public class ImageCaptioningResultDto
    {
        /// <summary>
        /// The caption of the image
        /// </summary>
        public string Caption { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public ImageCaptioningResultDto() { }

        /// <summary>
        /// Constructs a new instance of the <see cref="ImageCaptioningResultDto"/> class using the passed parameters
        /// </summary>
        public ImageCaptioningResultDto(string caption) => Caption = caption;
    }
}
