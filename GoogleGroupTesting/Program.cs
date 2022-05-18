using Google.Apis.Auth.OAuth2;
using Google.Apis.Admin.Directory.directory_v1;
using Google.Apis.Admin.Directory.directory_v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;

namespace AdminSDKDirectoryQuickstart
{
    class Program
    {
        // If modifying these scopes, delete your previously saved credentials
        // at ~/.credentials/admin-directory_v1-dotnet-quickstart.json
        static string[] Scopes = { DirectoryService.Scope.AdminDirectoryUserReadonly };
        static string ApplicationName = "Directory API .NET Quickstart";
        const string Google_Test_Token_Env_Var = "Google_Test_Token";

        static void Main(string[] args)
        {
            
            var token_file = Environment.GetEnvironmentVariable(Google_Test_Token_Env_Var);
            if (token_file == null)
                throw new System.Exception($"The environment variable: {Google_Test_Token_Env_Var} is not set");

            var credential = GoogleCredential.FromFile(token_file).CreateScoped(Scopes).CreateWithHttpClientFactory(null);

            // Create Directory API service.
            var service = new DirectoryService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            // Define parameters of request.
            UsersResource.ListRequest request = service.Users.List();
            request.Customer = "my_customer";
            request.MaxResults = 10;
            request.OrderBy = UsersResource.ListRequest.OrderByEnum.Email;

            // List users.
            IEnumerable<User> users = request.Execute().UsersValue;
            Console.WriteLine("Users:");
            if (users.Any())
            {
                foreach (var userItem in users)
                {
                    Console.WriteLine("{0} ({1})", userItem.PrimaryEmail,
                        userItem.Name.FullName);
                }
            }
            else
            {
                Console.WriteLine("No users found.");
            }
            Console.Read();
        }
    }
}