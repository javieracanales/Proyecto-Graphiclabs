using System;
using Proyecto.Providers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.IO;
namespace Proyecto.Helpers
{
	public class UploadFilesHelper
	{
		private PathProvider _pathProvider;

		public UploadFilesHelper(PathProvider pathProvider)
		{
			_pathProvider = pathProvider;
		}

		public async Task<String> UploadFiles(IFormFile formFile, string image, Folders folder)
		{
			string path = _pathProvider.MapPath(image,folder);

			using (Stream stream = new FileStream(path, FileMode.Create))
			{
				await formFile.CopyToAsync(stream);
			}

			return path;
		}
	}
}

