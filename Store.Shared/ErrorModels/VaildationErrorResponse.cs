using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Shared.ErrorModels
{
    public class VaildationErrorResponse
    {
        public int StatusCode { get; set; } = StatusCodes.Status400BadRequest;
        public string Message { get; set; } = "One or more validation errors occurred.";
        public IEnumerable<VaildationError> Errors { get; set; }
    }

    public class VaildationError
    {
        public string Field { get; set; }

        public IEnumerable<string> Errors { get; set; }
    }
}
