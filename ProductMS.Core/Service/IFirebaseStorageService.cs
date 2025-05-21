using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ProductMS.Core.Service
{
    public interface IFirebaseStorageService
    {
        /// <summary>
        /// Uploads an image to Firebase Storage
        /// </summary>
        /// <param name="fileStream">The file stream of the image</param>
        /// <param name="fileName">Name of the file</param>
        /// <returns>URL of the uploaded image</returns>
        Task<string> UploadImageAsync(Stream fileStream, string fileName);
        
        /// <summary>
        /// Uploads multiple images to Firebase Storage
        /// </summary>
        /// <param name="files">Dictionary containing file streams and their names</param>
        /// <returns>List of URLs of the uploaded images</returns>
        Task<List<string>> UploadMultipleImagesAsync(Dictionary<string, Stream> files);
        
        /// <summary>
        /// Deletes an image from Firebase Storage
        /// </summary>
        /// <param name="fileUrl">URL of the file to delete</param>
        /// <returns>True if deletion was successful</returns>
        Task<bool> DeleteImageAsync(string fileUrl);
    }
}
