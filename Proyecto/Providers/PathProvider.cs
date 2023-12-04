using System;
using Microsoft.AspNetCore.Hosting;
using System.IO;
namespace Proyecto.Providers
{
	public enum Folders
	{
        images = 0
	}

	public class PathProvider
    {
		private IWebHostEnvironment _hostEnvironment;

		public PathProvider(IWebHostEnvironment hostEnvironment)
		{
			_hostEnvironment = hostEnvironment;
		}

		public string MapPath(string fileName, Folders folder)
		{
			string carpeta = "";
			if (folder == Folders.images)
			{
                carpeta = "images";
            }

			string path = Path.Combine(_hostEnvironment.WebRootPath, carpeta, fileName);

			return path;
        }
		
	}
}

