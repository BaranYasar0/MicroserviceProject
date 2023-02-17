using FreeCourse.Shared.Dtos;
using FreeCourse.Web.Models.ViewModels.Catalog;
using FreeCourse.Web.Services.Interfaces;

namespace FreeCourse.Web.Services
{
    public class PhotoStockService : IPhotoStockService
    {
        private readonly HttpClient _httpClient;

        public PhotoStockService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> DeletePhoto(string photoUrl)
        {
        var response=await _httpClient.DeleteAsync($"photos?photoUrl={photoUrl}");
            return response.IsSuccessStatusCode;
        }

        public async Task<PhotoViewModel> UploadPhoto(IFormFile photo)
        {
            if (photo == null || photo.Length <= 0)
                return null;

            //dosya iismi:19827414.jpg gibi olucak extenseion methoduyla .jpg geliyor
            var randomFileName = $"{Guid.NewGuid().ToString()}{Path.GetExtension(photo.FileName)}";

            using var ms = new MemoryStream();

            await photo.CopyToAsync(ms);

            var multipartContent = new MultipartFormDataContent();
            
            //"photo" kısmı istek yapılan apideki IFormFile ismiyle aynı olmalıdır örneğin IFormFile photo
            multipartContent.Add(new ByteArrayContent(ms.ToArray()), "photo", randomFileName);

            var response = await _httpClient.PostAsync("photos", multipartContent);

            if (!response.IsSuccessStatusCode)
                return null;

            var responseSuccess= await response.Content.ReadFromJsonAsync<Response<PhotoViewModel>>();

            return responseSuccess.Data;
        }
    }
}
