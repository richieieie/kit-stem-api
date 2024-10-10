using System.Globalization;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace KST.Api.Configs
{
    public class KebabCaseModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
                throw new ArgumentNullException(nameof(bindingContext));


            if (bindingContext.ModelMetadata.IsComplexType)
            {
                var model = Activator.CreateInstance(bindingContext.ModelType);
                foreach (var property in bindingContext.ModelType.GetProperties())
                {
                    var kebabCaseName = ConvertToKebabCase(property.Name);
                    var valueProviderResult = bindingContext.ValueProvider.GetValue(kebabCaseName);

                    if (valueProviderResult != ValueProviderResult.None)
                    {
                        var convertedValue = ConvertValue(valueProviderResult.FirstValue, property.PropertyType, bindingContext, kebabCaseName);
                        if (convertedValue != null)
                        {
                            property.SetValue(model, convertedValue);
                        }
                    }
                }

                bindingContext.Result = ModelBindingResult.Success(model);
            }
            else
            {
                var kebabCaseName = ConvertToKebabCase(bindingContext.FieldName);
                var valueProviderResult = bindingContext.ValueProvider.GetValue(kebabCaseName);

                if (valueProviderResult != ValueProviderResult.None)
                {
                    var convertedValue = ConvertValue(valueProviderResult.FirstValue, bindingContext.ModelType, bindingContext, kebabCaseName);
                    if (convertedValue != null)
                    {
                        bindingContext.Result = ModelBindingResult.Success(convertedValue);
                    }
                }
            }

            return Task.CompletedTask;
        }

        private string ConvertToKebabCase(string name)
        {
            return string.Concat(name.Select((x, i) =>
                i > 0 && char.IsUpper(x) ? "-" + char.ToLower(x) : char.ToLower(x).ToString()));
        }

        private object? ConvertValue(string? value, Type targetType, ModelBindingContext bindingContext, string parameterName)
        {
            if (value == null)
            {
                if (targetType.IsValueType)
                {
                    bindingContext.ModelState.AddModelError(parameterName, $"{parameterName} cannot be null.");
                    return null; // Skip setting this value
                }
                return null; // Nullable types can accept null
            }
            try
            {
                // Handle specific types
                if (targetType == typeof(DateTime))
                {
                    if (DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out DateTime dateTimeValue))
                    {
                        return dateTimeValue;
                    }
                    else
                    {
                        bindingContext.ModelState.AddModelError(parameterName, $"The value '{value}' is not a valid DateTime.");
                    }
                }
                else if (targetType == typeof(DateTimeOffset))
                {
                    if (DateTimeOffset.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out DateTimeOffset dateTimeOffsetValue))
                    {
                        return dateTimeOffsetValue;
                    }
                    else
                    {
                        bindingContext.ModelState.AddModelError(parameterName, $"The value '{value}' is not a valid DateTimeOffset.");
                    }
                }
                else
                {
                    // Handle other types using Convert.ChangeType
                    return Convert.ChangeType(value, targetType);
                }
            }
            catch (InvalidCastException)
            {
                bindingContext.ModelState.AddModelError(parameterName, $"Invalid conversion for '{value}' to type '{targetType.Name}'.");
            }
            return null; // In case of error, return null
        }
    }
}