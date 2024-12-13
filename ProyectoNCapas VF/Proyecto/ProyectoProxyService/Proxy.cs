using Entities;
using Newtonsoft.Json;
using SLC;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoProxyService
{
    public class Proxy : IUserAccount, ICategory, IProduct, ILogin
    {
        private readonly string BaseAddress = "https://localhost:44396";

        private async Task<T> SendPost<T, U>(string requestURI, U data)
        {
            T result = default;
            using (var client = new HttpClient())
            {
                try
                {
                    requestURI = BaseAddress + requestURI;
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var jsonData = JsonConvert.SerializeObject(data);
                    var response = await client.PostAsync(requestURI, new StringContent(jsonData, Encoding.UTF8, "application/json"));

                    if (response.IsSuccessStatusCode)
                    {
                        var resultJson = await response.Content.ReadAsStringAsync();
                        result = JsonConvert.DeserializeObject<T>(resultJson);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error en SendPost: {ex.Message}");
                }
            }
            return result;
        }

        private async Task<T> SendGet<T>(string requestURI)
        {
            T result = default;
            using (var client = new HttpClient())
            {
                try
                {
                    requestURI = BaseAddress + requestURI;
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var response = await client.GetAsync(requestURI);
                    if (response.IsSuccessStatusCode)
                    {
                        var resultJson = await response.Content.ReadAsStringAsync();
                        result = JsonConvert.DeserializeObject<T>(resultJson);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error en SendGet: {ex.Message}");
                }
            }
            return result;
        }

        // UserAccount Methods
        public UserAccount CreateUserAccount(UserAccount newUserAccount)
        {
            return Task.Run(async () => await SendPost<UserAccount, UserAccount>($"/useraccount/create", newUserAccount)).Result;
        }

        public UserAccount GetUserAccountByID(int userId)
        {
            return Task.Run(async () => await SendGet<UserAccount>($"/useraccount/get/{userId}")).Result;
        }

        public bool UpdateUserAccount(UserAccount userAccountToUpdate)
        {
            return Task.Run(async () => await SendPost<bool, UserAccount>($"/useraccount/update", userAccountToUpdate)).Result;
        }

        public bool DeleteUserAccount(int userId)
        {
            return Task.Run(async () => await SendGet<bool>($"/useraccount/delete/{userId}")).Result;
        }

        public List<UserAccount> GetAllUserAccounts()
        {
            return Task.Run(async () => await SendGet<List<UserAccount>>($"/useraccount/all")).Result;
        }

        public UserAccount GetUserByEmail(string email)
        {
            return Task.Run(async () => await SendGet<UserAccount>($"/useraccount/email?email={Uri.EscapeDataString(email)}")).Result;
        }

        public bool UpdatePassword(int userId, string newPassword)
        {
            var request = new
            {
                UserID = userId,
                NewPassword = newPassword
            };

            return Task.Run(async () => await SendPost<bool, object>($"/useraccount/updatepassword", request)).Result;
        }

        // Category Methods
        public Category CreateCategory(Category newCategory)
        {
            return Task.Run(async () => await SendPost<Category, Category>($"/categories/create", newCategory)).Result;
        }

        public Category GetCategoryByID(int categoryId)
        {
            return Task.Run(async () => await SendGet<Category>($"/categories/get/{categoryId}")).Result;
        }

        public bool UpdateCategory(Category categoryToUpdate)
        {
            return Task.Run(async () => await SendPost<bool, Category>($"/categories/update", categoryToUpdate)).Result;
        }

        public bool DeleteCategory(int categoryId)
        {
            return Task.Run(async () => await SendGet<bool>($"/categories/delete/{categoryId}")).Result;
        }

        public List<Category> GetAllCategories()
        {
            return Task.Run(async () => await SendGet<List<Category>>($"/categories/all")).Result;
        }

        // Product Methods
        public Product CreateProduct(Product newProduct)
        {
            return Task.Run(async () => await SendPost<Product, Product>($"/product/create", newProduct)).Result;
        }

        public Product GetProductByID(int productId)
        {
            return Task.Run(async () => await SendGet<Product>($"/product/get/{productId}")).Result;
        }

        public bool UpdateProduct(Product productToUpdate)
        {
            return Task.Run(async () => await SendPost<bool, Product>($"/product/update", productToUpdate)).Result;
        }

        public bool DeleteProduct(int productId)
        {
            return Task.Run(async () => await SendGet<bool>($"/product/delete/{productId}")).Result;
        }

        public List<Product> GetProductsByCategoryID(int categoryId)
        {
            return Task.Run(async () => await SendGet<List<Product>>($"/product/category/{categoryId}")).Result;
        }

        public List<Product> GetAllProducts()
        {
            return Task.Run(async () => await SendGet<List<Product>>($"/product/all")).Result;
        }

        // Login Methods
        public UserAccount Authenticate(string email, string password)
        {
            var loginRequest = new
            {
                Email = email,
                Password = password
            };

            return Task.Run(async () => await SendPost<UserAccount, object>($"/login/authenticate", loginRequest)).Result;
        }
    }
}
