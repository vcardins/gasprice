#region



#endregion

using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

// ReSharper disable CheckNamespace

namespace GasPrice.Api.Extensions
{
    #region

    

    #endregion

    /// <summary>
    /// 
    /// </summary>
    public class ForbiddenWithMessageResult : IHttpActionResult
    {
        private readonly string _message;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public ForbiddenWithMessageResult(string message)
        {
            _message = message;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage(HttpStatusCode.Forbidden)
            {
                Content = new ObjectContent<object>(new { Message = _message}, new JsonMediaTypeFormatter(), "application/json")
            };
            return Task.FromResult(response);
        }
    }
}

