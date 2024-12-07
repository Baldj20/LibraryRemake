using Application.DTO.Request;
using Application.DTO.Response;
using Application.Exceptions;
using Application.Interfaces.Handlers.UserHandlers;
using Application.Interfaces.Services;
using Application.UseCases.UserUseCases;
using Domain.Interfaces.Repositories;
using FluentValidation;

namespace Application.Handlers.UserHandlers
{
    public class AuthorizeHandler : IAuthorizeHandler
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ITokenService tokenService;
        private readonly IHashPasswordService hashService;
        private readonly IValidator<UserRequest> validator;
        public AuthorizeHandler(IUnitOfWork unitOfWork, ITokenService tokenService,
            IHashPasswordService hashService, IValidator<UserRequest> validator)
        {
            this.unitOfWork = unitOfWork;
            this.tokenService = tokenService;
            this.hashService = hashService;
            this.validator = validator;
        }

        public async Task<TokenResponse> Handle(AuthorizeUseCase usecase, CancellationToken token)
        {
            var userRequest = usecase.UserRequest;

            var validationResult = validator.Validate(userRequest);
            if (!validationResult.IsValid) throw new BadRequestException(validationResult);

            token.ThrowIfCancellationRequested();

            var login = userRequest.UserLogin;
            var password = userRequest.Password;

            var user = await unitOfWork.Users.GetByLogin(login);
            if (user == null)
                throw new NotFoundException($"User with login {login} not found");

            if (!hashService.VerifyPassword(password, user.HashedPassword))
                throw new InvalidPasswordException();

            token.ThrowIfCancellationRequested();

            var access = tokenService.GenerateAccessToken(user);
            var refresh = tokenService.GenerateRefreshToken(user);

            return new TokenResponse
            {
                AccessToken = access,
                RefreshToken = refresh.Token,
            };
        }
    }
}
