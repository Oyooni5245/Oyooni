namespace Oyooni.Server.Dtos.AI
{
    /// <summary>
    /// Represents an image captioning response dto
    /// </summary>
    public class ImageCaptioningDto
    {
        /// <summary>
        /// The caption generated
        /// </summary>
        public string Caption { get; set; }

        /// <summary>
        /// Constructs a new instance of the <see cref="ImageCaptioningDto"/> using the passed parameters
        /// </summary>
        public ImageCaptioningDto(string caption)
        {
            Caption = caption;
        }
    }
}
