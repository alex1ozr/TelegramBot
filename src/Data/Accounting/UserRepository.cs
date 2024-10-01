using Microsoft.EntityFrameworkCore;
using TelegramBot.Data.Engine;
using TelegramBot.Domain.Accounting.Users;
using TelegramBot.Framework.EntityFramework.Storage;

namespace TelegramBot.Data.Accounting;

internal sealed class UserRepository :
    DefaultRepository<DataContext, User, UserId>,
    IUserRepository
{
    public UserRepository(DataContext context)
        : base(context)
    {
    }

    public async Task<User?> GetByTelegramUserIdAsync(string telegramUserId, CancellationToken cancellationToken)
    {
        return await Context.Users
            .FirstOrDefaultAsync(x => x.TelegramUserId == telegramUserId,
                cancellationToken);
    }

    public async Task<IReadOnlyList<User>> GetLastRegisteredUsersAsync(DateTime from, CancellationToken cancellationToken)
    {
        return await Context.Users
            .Where(x => x.CreatedAt >= from)
            .OrderBy(x => x.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> GetTotalUsersAsync(CancellationToken cancellationToken)
    {
        return await Context.Users.CountAsync(cancellationToken);
    }
}
