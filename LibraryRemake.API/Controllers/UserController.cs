using Application.DTO.Request;
using Application.DTO.Response;
using Application.Interfaces.Handlers.BookHandlers;
using Application.Interfaces.Handlers.UserHandlers;
using Application.UseCases.UserUseCases;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IAddUserHandler addUser;
        private readonly IDeleteUserHandler deleteUser;
        private readonly IGetAllUsersHandler getAllUsers;
        private readonly IGetUserByLoginHandler getUserByLogin;
        private readonly IUpdateUserHandler updateUser;
        private readonly IGetPagedUsersHandler getPagedUsers;
        private readonly IRegisterHandler registerUser;
        private readonly IAuthorizeHandler authorize;
        private readonly IRegisterBookForUserHandler registerBookForUser;
        private readonly IGetBookByISBNHandler getBookByISBN;
        private readonly IGetUserTakenBooksHandler getUserTakenBooks;
        public UserController(IAddUserHandler addUser, IDeleteUserHandler deleteUser,
            IGetAllUsersHandler getAllUsers, IGetUserByLoginHandler getUserByLogin,
            IUpdateUserHandler updateUser, IGetPagedUsersHandler getPagedUsers,
            IRegisterHandler registerUser, IAuthorizeHandler authorize,
            IRegisterBookForUserHandler registerBookForUser,
            IGetBookByISBNHandler getBookByISBN, IGetUserTakenBooksHandler getUserTakenBooks)
        {
            this.addUser = addUser;
            this.deleteUser = deleteUser;
            this.getAllUsers = getAllUsers;
            this.getUserByLogin = getUserByLogin;
            this.updateUser = updateUser;
            this.getPagedUsers = getPagedUsers;
            this.registerUser = registerUser;
            this.authorize = authorize;
            this.registerBookForUser = registerBookForUser;
            this.getBookByISBN = getBookByISBN;
            this.getUserTakenBooks = getUserTakenBooks;
        }

        [HttpPost("register")]
        public async Task<ActionResult<TokenResponse>> Register([FromBody] UserRequest request, CancellationToken token)
        {
            var useCase = new RegisterUseCase
            {
                UserRequest = request,
            };

            var response = await registerUser.Handle(useCase, token);

            return Ok(response);
        }

        [HttpPost("authorize")]
        public async Task<ActionResult<TokenResponse>> Authorize([FromBody] UserRequest request, CancellationToken token)
        {
            var useCase = new AuthorizeUseCase
            {
                UserRequest = request
            };

            var response = await authorize.Handle(useCase, token);

            return Ok(response);
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        public async Task<ActionResult<ActionSuccessStatusResponse>> Add(AddUserRequest request, CancellationToken token)
        {
            var useCase = new AddUserUseCase
            {
                AddUserRequest = request,
            };

            var response = await addUser.Handle(useCase, token);
            return Ok(response);
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpDelete("{login}")]
        public async Task<ActionResult<ActionSuccessStatusResponse>> Delete(string login, CancellationToken token)
        {
            var useCase = new DeleteUserUseCase
            {
                Login = login,
            };

            var response = await deleteUser.Handle(useCase, token);
            return Ok(response);
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpGet]
        public async Task<ActionResult<List<UserResponse>>> GetAll(CancellationToken token)
        {
            var useCase = new GetAllUsersUseCase
            {

            };

            var users = await getAllUsers.Handle(useCase, token);
            return Ok(users);
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpGet("{login}")]
        public async Task<ActionResult<UserResponse>> GetByLogin(string login, CancellationToken token)
        {
            var useCase = new GetUserByLoginUseCase
            {
                Login = login,
            };

            var user = await getUserByLogin.Handle(useCase, token);
            return Ok(user);
        }

        [Authorize]
        [HttpPut("update")]
        public async Task<ActionResult<ActionSuccessStatusResponse>> Update(UpdateUserRequest request, CancellationToken token)
        {
            var useCase = new UpdateUserUseCase
            {
                UserRequest = request,
            };

            var response = await updateUser.Handle(useCase, token);
            return Ok(response);
        }

        [HttpGet("{pageNumber}, {pageSize}")]
        public async Task<ActionResult<List<UserResponse>>> GetPaged(int pageNumber, int pageSize, CancellationToken token)
        {
            var useCase = new GetPagedUsersUseCase
            {
                PaginationParams = new PaginationParams { PageNumber = pageNumber, PageSize = pageSize },
            };

            var result = await getPagedUsers.Handle(useCase, token);
            return Ok(result);
        }

        [HttpPut("books/register")]
        public async Task<ActionResult<ActionSuccessStatusResponse>> RegisterBookForUser(RegisterBookForUserRequest request, CancellationToken token)
        {
            var useCase = new RegisterBookForUserUseCase
            {
                UserLogin = request.Login,
                BookISBN = request.BookISBN,
                ReceiptDate = DateTime.UtcNow,
                ReturnDate = DateTime.UtcNow.AddDays(30)
            };

            var response = await registerBookForUser.Handle(useCase, token);
            return Ok(response);
        }

        [HttpGet("{login}/books")]
        public async Task<ActionResult<List<UserBookResponse>>> GetUserTakenBooks(string login, CancellationToken token)
        {
            var useCase = new GetUserTakenBooksUseCase
            {
                Login = login,
            };

            var response = await getUserTakenBooks.Handle(useCase, token);
            return Ok(response);
        }

    }
}
