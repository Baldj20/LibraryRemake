using Application.DTO.Request;
using Application.DTO.Response;
using Application.Exceptions;
using Application.Interfaces.Handlers.UserHandlers;
using Application.Interfaces.Services;
using Application.UseCases.UserUseCases;
using AutoMapper;
using Domain.Interfaces.Repositories;
using FluentValidation;

namespace Application.Handlers.UserHandlers
{
    public class UpdateUserHandler : IUpdateUserHandler
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IHashPasswordService hashService;
        private readonly IValidator<UpdateUserRequest> validator;

        public UpdateUserHandler(IUnitOfWork unitOfWork, IMapper mapper,
            IHashPasswordService hashService, IValidator<UpdateUserRequest> validator)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.hashService = hashService;
            this.validator = validator;
        }
        public async Task<ActionSuccessStatusResponse> Handle(UpdateUserUseCase usecase, CancellationToken token)
        {
            var userRequest = usecase.UserRequest;

            var validationResult = validator.Validate(userRequest);
            if (!validationResult.IsValid) throw new BadRequestException(validationResult);

            var oldlogin = userRequest.OldLogin;
            var newlogin = userRequest.NewLogin;
            var password = userRequest.NewPassword;

            var user = await unitOfWork.Users.GetByLogin(oldlogin);

            if (user == null) throw new NotFoundException("User to update not found");

            user.Login = newlogin;
            user.HashedPassword = hashService.HashPassword(password);

            await unitOfWork.Users.Update(user, token);

            await unitOfWork.CompleteAsync(token);

            return new ActionSuccessStatusResponse
            {
                Success = true,
                Message = "User updated successfully"
            };
        }
    }
}
