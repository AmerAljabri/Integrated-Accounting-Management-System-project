using Microsoft.EntityFrameworkCore;
using accountSystem.Domain.Entities;
using Application.Interfaces;
using Application.Dto;
using AutoMapper;

namespace Infrastructure.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public AccountRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AccountDto> GetAccountByIdAsync(int accountId)
        {
            var account = await _context.Accounts
                                        .Include(a => a.TransactionAccounts)
                                        .FirstOrDefaultAsync(a => a.AccountId == accountId);

            return _mapper.Map<AccountDto>(account); // Map the entity to a DTO
        }


        public async Task<IEnumerable<AccountDto>> GetAllAccountsAsync()
        {
            var accounts = await _context.Accounts.ToListAsync();
            return _mapper.Map<IEnumerable<AccountDto>>(accounts); // Use AutoMapper for mapping
        }

        public async Task AddAccountAsync(AccountDto accountDto)
        {
            try
            {
                var account = _mapper.Map<Account>(accountDto); // Map DTO to entity
                await _context.Accounts.AddAsync(account); // Add the entity
                await _context.SaveChangesAsync(); // Save changes
            }
            catch (Exception e)
            {
                throw; // Handle exceptions as needed
            }
        }

        public async Task UpdateAccountAsync(AccountDto accountDto)
        {
            var account = await GetAccountByIdAsync(accountDto.AccountId); // Get the entity
            if (account != null)
            {
                _mapper.Map(accountDto, account); // Map the DTO to the entity
                await _context.SaveChangesAsync(); // Save changes
            }
        }

        public async Task DeleteAccountAsync(int accountId)
        {
            // Fetch the Account entity instead of AccountDto
            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.AccountId == accountId);
            if (account != null)
            {
                _context.Accounts.Remove(account); // Remove the entity
                await _context.SaveChangesAsync(); // Save changes
            }
        }

    }
}