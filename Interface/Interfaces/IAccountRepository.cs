
using accountSystem.Domain.Entities;
using Application.Dto;

namespace Application.Interfaces
{
    public interface IAccountRepository
    {
        Task<AccountDto> GetAccountByIdAsync(int accountId);
        Task<IEnumerable<AccountDto>> GetAllAccountsAsync();
        Task AddAccountAsync(AccountDto account);
        Task UpdateAccountAsync(AccountDto account);
        Task DeleteAccountAsync(int accountId);
    }
}
