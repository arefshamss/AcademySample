using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using Exception = System.Exception;

namespace Academy.Domain.Shared;


public static class HttpExtensions
{
    
    public static async Task<ApiResponse<TSuccess>> PostAsync<TSuccess>(this HttpClient httpClient, string url, object data)
        where TSuccess : new()
    {
        try
        {
            var requestBody = JsonConvert.SerializeObject(data);

            using var response = await httpClient.PostAsync(
                url,
                new StringContent(requestBody, Encoding.UTF8, "application/json"));
            
            response.EnsureSuccessStatusCode();
            
            string stringResponse = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<TSuccess>(stringResponse ,  new JsonSerializerSettings()    
            {
                NullValueHandling = NullValueHandling.Ignore
            });
            return ApiResponse<TSuccess>.SuccessResponse(response.StatusCode , result ?? new TSuccess());
        }
        catch (Exception e)
        {
            return ApiResponse<TSuccess>.FailResponse(e.Message);
        }
    }
    
    public static async Task<ApiResponse<TSuccess , TError>> PostAsync<TSuccess , TError>(this HttpClient httpClient, string url, object data)
        where TSuccess : new()
        where TError : new()
    {
        try
        {
            var requestBody = JsonConvert.SerializeObject(data);

            using var response = await httpClient.PostAsync(
                url,
                new StringContent(requestBody, Encoding.UTF8, "application/json"));

          
            string stringResponse = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                var errorResult = JsonConvert.DeserializeObject<TError>(stringResponse,  new JsonSerializerSettings()    
                {
                    NullValueHandling = NullValueHandling.Ignore
                });  
                return ApiResponse<TSuccess, TError>.FailResponse(errorResult ?? throw new InvalidOperationException(), response.StatusCode);
            }

            
            var result = JsonConvert.DeserializeObject<TSuccess>(stringResponse ,  new JsonSerializerSettings()    
            {
                NullValueHandling = NullValueHandling.Ignore
            });
            return ApiResponse<TSuccess , TError>.SuccessResponse(result ?? throw new InvalidOperationException() ,  response.StatusCode);
        }
        catch (Exception e)
        {
            return ApiResponse<TSuccess , TError>.FailResponse(new TError() , e.Message);
        }
    }

    public static async Task<ApiResponse<TSuccess>> PutAsync<TSuccess>(this HttpClient httpClient, string url, object data)
        where TSuccess : new()
    {
        try
        {
            var requestBody = JsonConvert.SerializeObject(data);

            using var response = await httpClient.PutAsync(
                url,
                new StringContent(requestBody, Encoding.UTF8, "application/json"));
            
            response.EnsureSuccessStatusCode();
            
            string stringResponse = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<TSuccess>(stringResponse ,  new JsonSerializerSettings()    
            {
                NullValueHandling = NullValueHandling.Ignore
            });
            return ApiResponse<TSuccess>.SuccessResponse(response.StatusCode, result ?? new TSuccess());
        }
        catch (Exception e)
        {
            return ApiResponse<TSuccess>.FailResponse(e.Message);
        }
        
    }

    public static async Task<ApiResponse<TSuccess , TError>> PutAsync<TSuccess , TError>(this HttpClient httpClient, string url, object data)
        where TSuccess : new()
        where TError : new()
    {
        try
        {
            var requestBody = JsonConvert.SerializeObject(data);

            using var response = await httpClient.PutAsync(
                url,
                new StringContent(requestBody, Encoding.UTF8, "application/json"));
            
            string stringResponse = await response.Content.ReadAsStringAsync();
            
            if (!response.IsSuccessStatusCode)
            {
                var errorResult = JsonConvert.DeserializeObject<TError>(stringResponse,  new JsonSerializerSettings()    
                {
                    NullValueHandling = NullValueHandling.Ignore
                });  
                return ApiResponse<TSuccess, TError>.FailResponse(errorResult ?? throw new InvalidOperationException(), response.StatusCode);
            }


            var result = JsonConvert.DeserializeObject<TSuccess>(stringResponse ,  new JsonSerializerSettings()    
            {
                NullValueHandling = NullValueHandling.Ignore
            });
            return ApiResponse<TSuccess , TError>.SuccessResponse(result ?? throw new InvalidOperationException() ,  response.StatusCode);
        }
        catch (Exception e)
        {
            return ApiResponse<TSuccess , TError>.FailResponse(new TError() , e.Message);
        }
        
    }
    
    public static async Task<ApiResponse<TSuccess>> GetAsync<TSuccess>(this HttpClient httpClient, string url)
        where TSuccess : new()
    {
        try
        {
            using var response = await httpClient.GetAsync(url);
            
            response.EnsureSuccessStatusCode();

            string stringResponse = await response.Content.ReadAsStringAsync();
            
            var result = JsonConvert.DeserializeObject<TSuccess>(stringResponse ,  new JsonSerializerSettings()    
            {
                NullValueHandling = NullValueHandling.Ignore
            });

            return ApiResponse<TSuccess>.SuccessResponse(response.StatusCode , result ?? new TSuccess());
        }
        catch (Exception e)
        {
            return ApiResponse<TSuccess>.FailResponse(e.Message);
        }
    }

    public static async Task<ApiResponse<TSuccess , TError>> GetAsync<TSuccess , TError>(this HttpClient httpClient, string url)
        where TSuccess : new()
        where TError : new()
    {
        try
        {
            using var response = await httpClient.GetAsync(url);
            
            string stringResponse = await response.Content.ReadAsStringAsync();
            
            if (!response.IsSuccessStatusCode)
            {
                var errorResult = JsonConvert.DeserializeObject<TError>(stringResponse,  new JsonSerializerSettings()    
                {
                    NullValueHandling = NullValueHandling.Ignore
                });  
                return ApiResponse<TSuccess, TError>.FailResponse(errorResult ?? throw new InvalidOperationException(), response.StatusCode);
            }

            
            var result = JsonConvert.DeserializeObject<TSuccess>(stringResponse ,  new JsonSerializerSettings()    
            {
                NullValueHandling = NullValueHandling.Ignore
            });

            return ApiResponse<TSuccess , TError>.SuccessResponse(result ?? throw new InvalidOperationException() ,  response.StatusCode);
        }
        catch (Exception e)
        {
            return ApiResponse<TSuccess , TError>.FailResponse(new TError() , e.Message);
        }
    }

    
    public static async Task<ApiResponse<TSuccess>> GetAsync<TSuccess>(this HttpClient httpClient, string url, object data)
        where TSuccess : new()
    {
        try
        {
            if (data != null)
                url += "?";

            foreach (PropertyInfo propertyInfo in data.GetType().GetProperties())
                url += $"{propertyInfo.Name}={propertyInfo.GetValue(data)}&";

            using var response = await httpClient.GetAsync(url);
            
            response.EnsureSuccessStatusCode();

            string stringResponse = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<TSuccess>(stringResponse ,  new JsonSerializerSettings()    
            {
                NullValueHandling = NullValueHandling.Ignore
            });  
            return ApiResponse<TSuccess>.SuccessResponse(response.StatusCode , result ?? new TSuccess());
        }
        catch (Exception e)
        {
            return ApiResponse<TSuccess>.FailResponse(e.Message);
        }
    }
    
    
    public static async Task<ApiResponse<TSuccess , TError>> GetAsync<TSuccess , TError>(this HttpClient httpClient, string url, object data)
        where TSuccess : new()
        where TError : new()
    {
        try
        {
            if (data != null)
                url += "?";

            foreach (PropertyInfo propertyInfo in data.GetType().GetProperties())
                url += $"{propertyInfo.Name}={propertyInfo.GetValue(data)}&";

            using var response = await httpClient.GetAsync(url);

            string stringResponse = await response.Content.ReadAsStringAsync();
            
            if (!response.IsSuccessStatusCode)
            {
                var errorResult = JsonConvert.DeserializeObject<TError>(stringResponse,  new JsonSerializerSettings()    
                {
                    NullValueHandling = NullValueHandling.Ignore
                });  
                return ApiResponse<TSuccess, TError>.FailResponse(errorResult ?? throw new InvalidOperationException(), response.StatusCode);
            }

            var result = JsonConvert.DeserializeObject<TSuccess>(stringResponse ,  new JsonSerializerSettings()    
            {
                NullValueHandling = NullValueHandling.Ignore
            });  
            return ApiResponse<TSuccess , TError>.SuccessResponse(result ?? throw new InvalidOperationException() ,  response.StatusCode);
        }
        catch (Exception e)
        {
            return ApiResponse<TSuccess , TError>.FailResponse(new TError() , e.Message);
        }
    }
    
}