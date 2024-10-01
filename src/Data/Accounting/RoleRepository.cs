using Microsoft.EntityFrameworkCore;
using TelegramBot.Data.Engine;
using TelegramBot.Domain.Accounting.Roles;
using TelegramBot.Framework.EntityFramework.Storage;

namespace TelegramBot.Data.Accounting;

internal sealed class RoleRepository :
    DefaultRepository<DataContext, Role, RoleId>,
    IRoleRepository
{
    public RoleRepository(DataContext context)
        : base(context)
    {
    }

    public async Task<Role?> GetByName(string name, CancellationToken cancellationToken)
    {
        var normalizedName = name.ToUpper();
        return await Context.Roles
            .FirstOrDefaultAsync(x => x.NormalizedName == normalizedName,
                cancellationToken);
    }
}
