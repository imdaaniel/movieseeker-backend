using System.Net;

namespace MovieSeeker.Application.Services
{
    public class ResponseService : IResponseService {
        private HttpStatusCode StatusCode = HttpStatusCode.OK;

        public bool Success { get; set; } = true;
        public List<string> Messages { get; set; } = new();
        
        public ResponseService AddError(string errorMessage, HttpStatusCode statusCode = HttpStatusCode.BadRequest) {
            Messages.Add(errorMessage);
            StatusCode = statusCode;
            Success = false;

            return this;
        }

        public ResponseService AddError(IEnumerable<string> errorMessages, HttpStatusCode statusCode = HttpStatusCode.BadRequest) {
            Messages.AddRange(errorMessages);
            StatusCode = statusCode;
            Success = false;

            return this;
        }

        public HttpStatusCode GetStatusCode() {
            return StatusCode;
        }
        
        public void SetStatusCode(HttpStatusCode statusCode) {
            StatusCode = statusCode;
        }
    }
    
    public class ResponseService<T> : ResponseService where T : class
    {
        public T? Data { get; set; }
    }
}