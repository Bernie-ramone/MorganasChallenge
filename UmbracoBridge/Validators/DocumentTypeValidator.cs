using UmbracoBridge.Models;

namespace UmbracoBridge.Validators
{
    public static class DocumentTypeValidator
    {
        public static (bool IsValid, List<string> Errors) Validate(DocumentTypeCreateRequest model)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(model.Alias))
                errors.Add("Alias is required.");

            if (string.IsNullOrWhiteSpace(model.Name))
                errors.Add("Name is required.");

            if (string.IsNullOrWhiteSpace(model.Description))
                errors.Add("Description is required.");

            if (string.IsNullOrWhiteSpace(model.Icon) || !model.Icon.StartsWith("icon-"))
                errors.Add("Icon must start with 'icon-'.");

            return (!errors.Any(), errors);
        }
    }
}
