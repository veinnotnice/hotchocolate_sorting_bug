
using HotChocolate.Data.Sorting;
using HotChocolate.Language;
using HotChocolate.Resolvers;
using hotchocolate_bug.DB;
using Microsoft.EntityFrameworkCore;

namespace hotchocolate_bug.Queries;
[QueryType]
public class Query
{
    [UseSorting]
    public async Task<IQueryable<Person>> GetPersons(IDbContextFactory<AppDbContext> factory, IResolverContext resolverContext)
    {
        var context = await factory.CreateDbContextAsync();
        var test = resolverContext.HasSortingRequested();

        // var def = sortingContext?.AsSortDefinition<Person>();
        return context.People;
    }



}

public static class IResolverContextExtensions
{
    public static bool HasSortingRequested(this IResolverContext context)
    {
        try
        {
            var orderByArgument = context.ArgumentLiteral<IValueNode>("order");
            if (orderByArgument != NullValueNode.Default && orderByArgument != null)
            {
                return true;
            }
        }
        catch
        {
            return false;
        }

        return false;
    }
}