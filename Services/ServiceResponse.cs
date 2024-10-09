namespace kit_stem_api.Services
{
    public class ServiceResponse
    {
        public bool Succeeded { get; private set; }
        public string Status { get; private set; }
        public int StatusCode { get; private set; } = 500;
        public Dictionary<string, object>? Details { get; private set; }

        public ServiceResponse()
        {
            Succeeded = true;
            Status = "success";
        }

        public ServiceResponse SetSucceeded(bool status)
        {
            Status = status ? "success" : "fail";

            Succeeded = status;
            return this;
        }
        public ServiceResponse AddDetail(string key, object value)
        {
            if (Details == null)
            {
                Details = new Dictionary<string, object>();
            }
            Details.Add(ToKebabCase(key), value);
            return this;
        }

        // Dùng để add errors
        public ServiceResponse AddError(string key, string value)
        {
            if (Details == null)
            {
                Details = new Dictionary<string, object>();
            }
            if (!Details.ContainsKey("errors"))
            {
                Details.Add("errors", new Dictionary<string, string>());
            }

            var errors = (Dictionary<string, string>)Details["errors"];
            errors.Add(ToKebabCase(key), value);

            return this;
        }

        public ServiceResponse SetStatusCode(int code)
        {
            StatusCode = code;
            return this;
        }

        private static string ToKebabCase(string input)
        {
            return string.Concat(input.Select((c, i) =>
                i > 0 && char.IsUpper(c) ? "-" + char.ToLower(c).ToString() : char.ToLower(c).ToString()));
        }
    }
}