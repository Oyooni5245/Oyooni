using Oyooni.Server.Enumerations;
using System.Collections.Generic;
using System.Text.Json;

namespace Oyooni.Server.Common
{
    /// <summary>
    /// Represents an api responses
    /// </summary>
    public class ApiResponse
    {
        /// <summary>
        /// The message of the response
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public ApiResponse() { }

        /// <summary>
        /// Constructs a new instance of the <see cref="ApiResponse"/> class
        /// </summary>
        public ApiResponse(string message = null) => (Message) = (message);

        /// <summary>
        /// Serializate the <see cref="ApiResponse"/> to json
        /// </summary>
        /// <returns>Json representation of the response</returns>
        public string ToJson() => JsonSerializer.Serialize(this);
    }

    /// <summary>
    /// Represents an <see cref="ApiResponse"/> that accepts data
    /// </summary>
    /// <typeparam name="TData">The type of data to be sent with the response</typeparam>
    public class ApiResponse<TData> : ApiResponse
    {
        /// <summary>
        /// The data sent with the response
        /// </summary>
        public TData Data { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public ApiResponse() { }

        /// <summary>
        /// Constructs a new instance of the <see cref="ApiResponse"/> with <see cref="TData"/> data and message
        /// </summary>
        public ApiResponse(string message, TData data) : base(message) => Data = data;

        /// <summary>
        /// Constructs a new instance of the <see cref="ApiResponse"/> with data
        /// </summary>
        public ApiResponse(TData data) => Data = data;
    }

    /// <summary>
    /// Represents an api paged response with data
    /// </summary>
    /// <typeparam name="TData">The type to be sent with the response</typeparam>
    public class PagedApiResponse<TData> : ApiResponse
    {
        /// <summary>
        /// The number of entities per page
        /// </summary>
        public int EntitiesPerPage { get; set; }

        /// <summary>
        /// The page number
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// The total number of entities
        /// </summary>
        public int TotalEntities { get; set; }

        /// <summary>
        /// The data sent with the response
        /// </summary>
        public ICollection<TData> Data { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public PagedApiResponse() { }

        /// <summary>
        /// Constructs a new instance of the <see cref="PagedApiResponse{TData}"/> with <see cref="TData"/> data
        /// </summary>
        public PagedApiResponse(string message, int entitiesPerPage, int pageNumber, int totalEntities, ICollection<TData> data)
            : base(message) => (EntitiesPerPage, PageNumber, TotalEntities, Data) = (entitiesPerPage, pageNumber, totalEntities, data);
    }

    /// <summary>
    /// Represents an api error response with detailed errors if needed
    /// </summary>
    public class ApiErrorResponse : ApiResponse
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public ApiErrorResponse() { }

        /// <summary>
        /// The details errors sent with the response
        /// </summary>
        public Dictionary<string, List<string>> Errors { get; set; }

        /// <summary>
        /// Constructs a new instance of the <see cref="ApiErrorResponse"/> with the message and the errors if needed
        /// </summary>
        public ApiErrorResponse(string message = null, Dictionary<string, List<string>> errors = null)
            : base(message) => Errors = errors ?? new Dictionary<string, List<string>>();
    }

    /// <summary>
    /// Represents an image captioning api response
    /// </summary>
    public class ImageCaptioningResponse : ApiResponse
    {
        /// <summary>
        /// The caption generated
        /// </summary>
        public string Caption { get; set; }

        /// <summary>
        /// Constructs a new instance of the <see cref="ImageCaptioningResponse"/> using the passed parameters
        /// </summary>
        public ImageCaptioningResponse(string message, string caption) : base(message)
        {
            Caption = caption;
        }
    }

    /// <summary>
    /// Represents a banknote detection api response
    /// </summary>
    public class BankNoteDetectionResponse : ApiResponse
    {
        /// <summary>
        /// The bank note that has been recognized
        /// </summary>
        public SyrianBankNoteTypes BankNoteType { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public BankNoteDetectionResponse() { }

        /// <summary>
        /// Constructs a new instance of the <see cref="BanknoteDetectionResponse"/> using the passed parameters
        /// </summary>
        public BankNoteDetectionResponse(string message, SyrianBankNoteTypes bankNoteType) : base(message)
        {
            BankNoteType = bankNoteType;
        }
    }

    /// <summary>
    /// Represents a color recognition api response
    /// </summary>
    public class ColorRecognitionResponse : ApiResponse
    {
        /// <summary>
        /// The recognized color
        /// </summary>
        public RecognizedColor RecognizedColor { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public ColorRecognitionResponse() { }

        /// <summary>
        /// Constructs a new instance of the <see cref="ColorRecognitionResponse"/> using the passed parameters
        /// </summary>
        public ColorRecognitionResponse(string message, RecognizedColor recognizedColor) : base(message)
        {
            RecognizedColor = recognizedColor;
        }
    }

    /// <summary>
    /// Represents a text recognition api response
    /// </summary>
    public class TextRecognitionResponse : ApiResponse
    {
        /// <summary>
        /// The recognized text
        /// </summary>
        public string[] RecognizedText { get; set; }

        /// <summary>
        /// The brand name or the main text recognized
        /// </summary>
        public string BrandName { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public TextRecognitionResponse() { }

        /// <summary>
        /// Constructs a new instance of the <see cref="TextRecognitionResponse"/> using the passed parameters
        /// </summary>
        public TextRecognitionResponse(string message, string[] recognizedText, string brandName) : base(message)
        {
            RecognizedText = recognizedText;
            BrandName = brandName;
        }
    }
}