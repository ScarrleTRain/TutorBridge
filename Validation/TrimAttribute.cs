using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Threading.Tasks;

namespace TutorBridge.Validation
{
    public class TrimAttribute : ModelBinderAttribute
    {
        public TrimAttribute() : base(typeof(TrimmingModelBinder))
        {
        }
    }

    public class TrimmingModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if (valueProviderResult == ValueProviderResult.None)
            {
                return Task.CompletedTask;
            }

            bindingContext.ModelState.SetModelValue(bindingContext.ModelName, valueProviderResult);

            var submitted = valueProviderResult.FirstValue;
            var trimmed = string.IsNullOrEmpty(submitted) ? submitted : submitted.Trim();

            bindingContext.Result = ModelBindingResult.Success(trimmed);
            return Task.CompletedTask;
        }
    }
}
