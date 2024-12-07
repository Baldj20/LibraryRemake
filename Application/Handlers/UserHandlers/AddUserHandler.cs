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
    public class AddUserHandler : IAddUserHandler
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IHashPasswordService hashService;
        private readonly IValidator<AddUserRequest> validator;
        public AddUserHandler(IUnitOfWork unitOfWork, IMapper mapper,
            IHashPasswordService hashService, IValidator<AddUserRequest> validator)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.hashService = hashService;
            this.validator = validator;
        }
        public async Task<ActionSuccessStatusResponse> Handle(AddUserUseCase usecase, CancellationToken token)
        {
            var userRequest = usecase.AddUserRequest;

            var validationResult = validator.Validate(userRequest);
            if (!validationResult.IsValid) throw new BadRequestException(validationResult);

            token.ThrowIfCancellationRequested();

            var login = userRequest.UserLogin;
            var password = userRequest.Password;

            var userFromDB = await unitOfWork.Users.GetByLogin(login);

            if (userFromDB != null) throw new AlreadyExistException("Cannot add user because user with this login already exists");

            token.ThrowIfCancellationRequested();

            userRequest.Password = hashService.HashPassword(password);
            var user = mapper.Map<User>(userRequest);

            await unitOfWork.Users.Add(user, token);

            await unitOfWork.CompleteAsync(token);

            return new ActionSuccessStatusResponse
            {
                Success = true,
                Message = "User added successfully"
            };
        }
    }
}
