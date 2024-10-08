using System.Text.RegularExpressions;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace kit_stem_api.Configs
{
    public class KebabCaseParameterFilter : IParameterFilter
    {
        public void Apply(OpenApiParameter parameter, ParameterFilterContext context)
        {
            // Convert the parameter name to kebab-case if it's a query parameter
            if (parameter.In == ParameterLocation.Query)
            {
                parameter.Name = ToKebabCase(parameter.Name);
            }
        }

        // Function to convert PascalCase to kebab-case
        private string ToKebabCase(string name)
        {
            return Regex.Replace(name, "(?<!^)([A-Z])", "-$1").ToLower();
        }
    }

}