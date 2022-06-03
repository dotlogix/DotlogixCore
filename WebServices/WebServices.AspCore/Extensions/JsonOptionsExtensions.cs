using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DotLogix.WebServices.AspCore.Extensions
{
    public static class JsonOptionsExtensions
    {
        /// <summary>
        ///     Applies the settings of one setting object to another
        /// </summary>
        public static void UseSerializerSettings(this MvcNewtonsoftJsonOptions options, JsonSerializerSettings serializerSettings)
        {
            var target = options.SerializerSettings;
            // date formatting
            target.DateFormatHandling = serializerSettings.DateFormatHandling;
            target.DateFormatString = serializerSettings.DateFormatString;
            target.DateParseHandling = serializerSettings.DateParseHandling;
            target.DateTimeZoneHandling = serializerSettings.DateTimeZoneHandling;

            // number formatting
            target.FloatFormatHandling = serializerSettings.FloatFormatHandling;
            target.FloatParseHandling = serializerSettings.FloatParseHandling;

            // string formatting
            target.StringEscapeHandling = serializerSettings.StringEscapeHandling;

            // default handling
            target.NullValueHandling = serializerSettings.NullValueHandling;
            target.DefaultValueHandling = serializerSettings.DefaultValueHandling;

            // value handling
            target.TypeNameHandling = serializerSettings.TypeNameHandling;
            target.TypeNameAssemblyFormatHandling = serializerSettings.TypeNameAssemblyFormatHandling;
            target.ConstructorHandling = serializerSettings.ConstructorHandling;
            target.ObjectCreationHandling = serializerSettings.ObjectCreationHandling;
            target.MetadataPropertyHandling = serializerSettings.MetadataPropertyHandling;
            target.MissingMemberHandling = serializerSettings.MissingMemberHandling;

            // reference handling
            target.ReferenceLoopHandling = serializerSettings.ReferenceLoopHandling;
            target.PreserveReferencesHandling = serializerSettings.PreserveReferencesHandling;

            // other options
            target.Culture = serializerSettings.Culture;
            target.Formatting = serializerSettings.Formatting;
            target.Context = serializerSettings.Context;
            target.CheckAdditionalContent = serializerSettings.CheckAdditionalContent;
            target.ContractResolver = serializerSettings.ContractResolver;
            target.EqualityComparer = serializerSettings.EqualityComparer;
            target.MaxDepth = serializerSettings.MaxDepth;
        }
    }
}