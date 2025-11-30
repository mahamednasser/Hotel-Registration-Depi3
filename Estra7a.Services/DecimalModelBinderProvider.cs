using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

public class DecimalModelBinderProvider : IModelBinderProvider
{
    public IModelBinder? GetBinder(ModelBinderProviderContext context)
    {
        if (context == null)
            return null;
        
        if (context.Metadata.ModelType == typeof(decimal))
        {
            // Check if the DisplayFormat attribute is applied with a specific format string.
            // If so, we might want to let the default model binder handle it,
            // or enhance this binder to respect those formats.
            // For now, this custom binder will take precedence.
            // var displayFormatAttribute = context.Metadata.ValidatorMetadata.OfType<DisplayFormatAttribute>().FirstOrDefault();
            // if (displayFormatAttribute != null && !string.IsNullOrEmpty(displayFormatAttribute.DataFormatString))
            // {
            //     // Optionally, return null here to let another binder handle it if specific formatting is applied.
            //     // return null; 
            // }

            return new DecimalModelBinder();
        }
        
        return null;
    }
} 