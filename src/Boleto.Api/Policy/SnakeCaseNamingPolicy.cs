using System.Text.Json;
using Boleto.Api.Configuration;

namespace Boleto.Api.Policy
{
     public class SnakeCaseNamingPolicy : JsonNamingPolicy
    {
        public override string ConvertName(string name) => name.ToSnakeCase();
    }
}