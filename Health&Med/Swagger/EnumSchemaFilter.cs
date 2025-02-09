namespace Health_Med.Swagger
{
    using Microsoft.OpenApi.Models;
    using Swashbuckle.AspNetCore.SwaggerGen;
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;

    namespace Health_Med.Swagger
    {
        public class EnumSchemaFilter : ISchemaFilter
        {
            public void Apply(OpenApiSchema schema, SchemaFilterContext context)
            {
                if (context.Type.IsEnum)
                {
                    var enumValues = Enum.GetValues(context.Type)
                        .Cast<Enum>()
                        .Select(e => new
                        {
                            Name = e.ToString(),
                            Value = Convert.ToInt32(e),
                            Description = e.GetType()
                                          .GetMember(e.ToString())
                                          .FirstOrDefault()?
                                          .GetCustomAttribute<DescriptionAttribute>()?
                                          .Description ?? e.ToString()
                        });

                    schema.Enum.Clear();
                    foreach (var enumValue in enumValues)
                    {
                        schema.Enum.Add(new Microsoft.OpenApi.Any.OpenApiString($"{enumValue.Name} ({enumValue.Value}): {enumValue.Description}"));
                    }
                }
            }
        }
    }
}
