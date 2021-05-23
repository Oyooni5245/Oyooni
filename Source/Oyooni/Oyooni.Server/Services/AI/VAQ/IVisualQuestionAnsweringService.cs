using System.Threading;
using System.Threading.Tasks;

namespace Oyooni.Server.Services.AI.VAQ
{
    /// <summary>
    /// Represents a visual question answering service contract
    /// </summary>
    public interface IVisualQuestionAnsweringService
    {
        /// <summary>
        /// Answers a question according to the visual content of the passed image
        /// </summary>
        /// <param name="question">Question to be answered</param>
        /// <param name="base64ImageData">The data of the image in the base64 format</param>
        /// <returns>The answer of the question related to the image</returns>
        Task<string> AnswerAsync(string question, string base64ImageData, CancellationToken token = default);
    }
}
