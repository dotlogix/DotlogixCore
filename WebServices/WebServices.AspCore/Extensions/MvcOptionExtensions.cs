using DotLogix.WebServices.AspCore.Mvc;
using Microsoft.AspNetCore.Mvc;

namespace DotLogix.WebServices.AspCore.Extensions
{
    public static class MvcOptionExtensions
    {
        public static void UseUnitOfWork(this MvcOptions options)
        {
            options.Filters.Add<UnitOfWorkResultFilter>();
        }

        public static void UseErrorHandling(this MvcOptions options)
        {
            options.Filters.Add<ErrorHandlingExceptionFilter>();
        }
    }
}