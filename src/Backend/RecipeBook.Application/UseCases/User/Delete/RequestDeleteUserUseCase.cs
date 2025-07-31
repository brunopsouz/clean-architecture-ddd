
using RecipeBook.Domain.Repositories;
using RecipeBook.Domain.Repositories.User;
using RecipeBook.Domain.Services.LoggedUser;
using RecipeBook.Domain.Services.ServiceBus;

namespace RecipeBook.Application.UseCases.User.Delete
{
    public class RequestDeleteUserUseCase : IRequestDeleteUserUseCase
    {
        private readonly IDeleteUserQueue _queue;
        private readonly IUserUpdateOnlyRepository _userUpdateRepository;
        private readonly ILoggedUser _loggedUser;
        private readonly IUnitOfWork _unitOfWork;

        public RequestDeleteUserUseCase(
            IDeleteUserQueue deleteUserQueue,
            IUserUpdateOnlyRepository userUpdateOnlyRepository,
            ILoggedUser loggedUser,
            IUnitOfWork unitOfWork)
        {
            _queue = deleteUserQueue;
            _userUpdateRepository = userUpdateOnlyRepository;
            _loggedUser = loggedUser;
            _unitOfWork = unitOfWork;
        }

        public async Task Execute()
        {
            var loggedUser = await _loggedUser.User();

            var user = await _userUpdateRepository.GetById(loggedUser.Id);

            user.Active = false;
            _userUpdateRepository.Update(user);

            await _unitOfWork.Commit();

            await _queue.SendMessage(loggedUser);
        }
    }
}
