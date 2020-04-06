namespace RestaurantsPlatform.Services.Data.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public interface IImageService
    {
        string UploadImageToCloudinary(string imageUrl);
    }
}
