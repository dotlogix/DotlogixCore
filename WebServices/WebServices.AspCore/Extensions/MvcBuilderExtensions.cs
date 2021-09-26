using System;
using DotLogix.Core.Utils.Naming;
using DotLogix.WebServices.Core.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace DotLogix.WebServices.AspCore.Extensions {
    public static class MvcBuilderExtensions {
        public static IMvcBuilder AddGenericNewtonsoftJson(this IMvcBuilder builder, INamingStrategy namingStrategy = null, Action<MvcNewtonsoftJsonOptions> setupAction = null) {
            builder.AddNewtonsoftJson(options => {
                                          options.SerializerSettings.ContractResolver = new GenericContractResolver(namingStrategy ?? NamingStrategies.CamelCase);
                                          setupAction?.Invoke(options);
                                      }
                                     );
            return builder;
        }
    }
}