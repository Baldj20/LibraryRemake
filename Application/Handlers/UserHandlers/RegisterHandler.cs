using Application.DTO.Request;
using Application.DTO.Response;
using Application.Exceptions;
using Application.Interfaces.Handlers.UserHandlers;
using Application.Interfaces.Services;
using Application.UseCases.UserUseCases;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using FluentValidation;

namespace Application.Handlers.UserHandlers
{
    public class RegisterHandler : IRegisterHandler
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ITokenService tokenService;
        private readonly IMapper mapper;
        private readonly IHashPasswordService hashService;
        private readonly IValidator<UserRequest> validator;
        public RegisterHandler(IUnitOfWork unitOfWork,
            ITokenService tokenService, IMapper mapper,
            IHashPasswordService hashService,
            IValidator<UserRequest> validator)
        {
            this.unitOfWork = unitOfWork;
            this.tokenService = tokenService;
            this.mapper = mapper;
            this.hashService = hashService;
            this.validator = validator;
        }
        public async Task<TokenResponse> Handle(RegisterUseCase usecase, CancellationToken token)
        {
            var userRequest = usecase.UserRequest;

            var validationResult = validator.Validate(userRequest);
            if (!validationResult.IsValid) throw new BadRequestException(validationResult);

            token.ThrowIfCancellationRequested();

            var login = userRequest.UserLogin;
            var password = userRequest.Password;

            if (await unitOfWork.Users.GetByLogin(login) != null)
                throw new AlreadyExistException($"User with login {login} is already exists");

            var user = mapper.Map<User>(userRequest);

            token.ThrowIfCancellationRequested();

            var accessToken = tokenService.GenerateAccessToken(user);

            token.ThrowIfCancellationRequested();

            var refreshToken = tokenService.GenerateRefreshToken(user);

            token.ThrowIfCancellationRequested();

            user.RefreshTokens.Add(refreshToken);
            user.HashedPassword = hashService.HashPassword(password);

            await unitOfWork.Users.Add(user, token);

            await unitOfWork.CompleteAsync(token);

            return new TokenResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token,
            };
        }
    }
}
