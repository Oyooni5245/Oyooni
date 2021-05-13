using Oyooni.Server.Constants;
using System;
using System.Net;

namespace Oyooni.Server.Exceptions
{
    /// <summary>
    /// Represents the base application exception
    /// </summary>
    public abstract class BaseException : Exception
    {
        /// <summary>
        /// The <see cref="HttpStatusCode"/> as an integer
        /// </summary>
        public int StatusCode { get; }

        /// <summary>
        /// Constructs a new instance of the <see cref="BaseException"/> class using the passed parameters
        /// </summary>
        public BaseException(string message, HttpStatusCode statusCode) : base(message)
        {
            // Set the status code
            StatusCode = (int)statusCode;
        }
    }

    /// <summary>
    /// Represents a not found exception
    /// </summary>
    public class NotFoundException : BaseException
    {
        /// <summary>
        /// Constructs a new instance of the <see cref="NotFoundException"/> class using the passed parameters
        /// </summary>
        public NotFoundException(string message) : base(message, HttpStatusCode.NotFound) { }
    }

    /// <summary>
    /// Represents an already exists exception
    /// </summary>
    public class AlreadyExistException : BaseException
    {
        /// <summary>
        /// Constructs a new instance of the <see cref="AlreadyExistException"/> class using the passed parameters
        /// </summary>
        public AlreadyExistException(string message) : base(message, HttpStatusCode.Conflict) { }
    }

    /// <summary>
    /// Represents an unauthorized exception
    /// </summary>
    public class UnAuthorizedException : BaseException
    {
        /// <summary>
        /// Constructs a new instance of the <see cref="UnAuthorizedException"/> class using the passed parameters
        /// </summary>
        public UnAuthorizedException(string message = Responses.General.UnAuthorizedAction) 
            : base(message, HttpStatusCode.Unauthorized) { }
    }

    /// <summary>
    /// Represents a bad request exception
    /// </summary>
    public class BadRequestException : BaseException
    {
        /// <summary>
        /// Constructs a new instance of the <see cref="BadRequestException"/> class using the passed parameters
        /// </summary>
        public BadRequestException(string message) : base(message, HttpStatusCode.BadRequest) { }
    }

    /// <summary>
    /// Represents a forbiddenexception
    /// </summary>
    public class ForbiddenException : BaseException
    {
        /// <summary>
        /// Constructs a new instance of the <see cref="ForbiddenException"/> class using the passed parameters
        /// </summary>
        public ForbiddenException(string message) : base(message, HttpStatusCode.Forbidden) { }
    }

    /// <summary>
    /// Represents a conflict exception
    /// </summary>
    public class ConflictException : BaseException
    {
        /// <summary>
        /// Constructs a new instance of the <see cref="ConflictException"/> class using the passed parameters
        /// </summary>
        public ConflictException(string message) : base(message, HttpStatusCode.Conflict) { }
    }
}
