using Grpc.Core;

namespace GrpcService.Services
{
    public class SecondService : Seconder.SeconderBase
    {
        public override Task<RegistrationReply> RegisterMe(RegistrationDto request, ServerCallContext context)
        {


            return !request.UserId.StartsWith("Va")
                ? Task.FromResult(new RegistrationReply()
                {
                    UserId = request.UserId,
                    IsSuccess = true,
                })
                : Task.FromResult(new RegistrationReply()
                {
                    UserId = request.UserId,
                    IsSuccess = false,
                });
        }
    }
}
