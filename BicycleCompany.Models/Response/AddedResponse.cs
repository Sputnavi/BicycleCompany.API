using System;
using System.Net;

namespace BicycleCompany.Models.Response
{
    public class AddedResponse : BaseResponseModel
    {
        public Guid Id { get; set; }
        public AddedResponse(Guid id)
        {
            Id = id;
            StatusCode = HttpStatusCode.Created;
        }
    }
}
