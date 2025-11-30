using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Globalization;
using System.Threading.Tasks;

public class DecimalModelBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        if (bindingContext == null)
        {
            throw new ArgumentNullException(nameof(bindingContext));
        }

        var modelName = bindingContext.ModelName;
        var valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);

        if (valueProviderResult == ValueProviderResult.None)
        {
            // No value provided for the model name.
            // Let the default model binding behavior handle this (e.g. for nullable types).
            return Task.CompletedTask;
        }

        bindingContext.ModelState.SetModelValue(modelName, valueProviderResult);

        var value = valueProviderResult.FirstValue;

        if (string.IsNullOrEmpty(value))
        {
            // Value is null or empty.
            // Let the default model binding behavior handle this (e.g. for nullable types or [Required] attribute).
            return Task.CompletedTask;
        }

        // Try to parse using InvariantCulture (handles '.') and current culture (handles ',')
        // Allow number styles such as currency symbols, group separators etc.
        if (decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var decimalValue))
        {
            bindingContext.Result = ModelBindingResult.Success(decimalValue);
        }
        else if (decimal.TryParse(value, NumberStyles.Any, CultureInfo.CurrentCulture, out decimalValue))
        {
            // As a fallback, try with current culture, in case the format matches it.
            // This might be useful if InvariantCulture fails for a valid CurrentCulture format.
            bindingContext.Result = ModelBindingResult.Success(decimalValue);
        }
        else
        {
            // If parsing fails with both cultures, add a model error.
            // The message here should use modelName for better context.
            bindingContext.ModelState.TryAddModelError(
                modelName, $"The value '{value}' is not valid for {modelName}. (Error from Custom DecimalModelBinder)");
        }

        return Task.CompletedTask;
    }
} 