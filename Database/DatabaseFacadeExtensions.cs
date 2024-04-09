using Microsoft.EntityFrameworkCore;
using Project_CS412.Database.Models;

namespace Project_CS412.Database;

public static class DatabaseFacadeExtensions
{
    public static async Task Upsert<T>(this DbSet<T> dbSet, T newT)
        where T : AbstractItem
    {
        var existingEntity = await dbSet.FindAsync(newT.Id);
        if (existingEntity == null)
        {
            dbSet.Add(newT);
            return;
        }

        dbSet.Update(newT);
    }
}