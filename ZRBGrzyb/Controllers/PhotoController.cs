using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ZRBGrzyb.Models;
using ZRBGrzyb.Models.ViewModels;
using ZRBGrzyb.Infrastructure;

namespace ZRBGrzyb.Controllers {

    [Authorize]
    public class PhotoController : Controller {

        private IRepository repository;
        private readonly IConfiguration configuration;
        private readonly IFileProvider fileProvider;
        private readonly SelectList categoryList;
        private readonly string photosDirectory;
        private readonly string photosViewDirectory;
        public int PageSize;

        public PhotoController(IRepository repo,
                               IFileProvider fileprov,
                               IConfiguration config) {

            repository = repo;
            fileProvider = fileprov;
            configuration = config;
            categoryList = new SelectList(repo.Categories
                    .OrderBy(p => p.Name)
                    .ToList(), "CategoryID", "Name");
            photosDirectory = config["Data:Grzyb:PhotosDirectory"];
            photosViewDirectory = config["Data:Grzyb:PhotosViewDirectory"];
            PageSize = Int32.Parse(config["Data:Grzyb:GalleryPageSize"]);
        }

        [AllowAnonymous]
        public ViewResult Gallery(string category, int photoPage = 1) {

            ViewBag.CurrentPage = "PhotoGallery";
            ViewBag.PhotosViewDirectory = photosViewDirectory;

            return View(new PhotosGalleryViewModel {
                Photos = repository.Photos
                .Where(p => category == null || p.Category.RouteName == category)
                .OrderByDescending(p => p.AddDate)
                .Skip((photoPage - 1) * PageSize)
                .Take(PageSize),
                PagingInfo = new PagingInfo {
                    CurrentPage = photoPage,
                    ItemsPerPage = PageSize,
                    TotalItems = category == null ?
                    repository.Photos.Count() :
                    repository.Photos.Where(e =>
                e.Category.RouteName == category).Count()
                },
                CurrentCategory = category
            });
        }

        public ViewResult Index() => View(repository.Photos);

        public ViewResult Edit(int photoId) {

            Photo photo = repository.Photos
                .FirstOrDefault(p => p.PhotoID == photoId);

            ViewBag.Categories = categoryList;
            ViewBag.Thumb = Url.Content(
                photosViewDirectory + "/thumbs/" + photo.FileName);

            return View(photo);
        }

        public ViewResult Create() {

            DateTime localDateTime = DateTime.Now;
            Photo photo = new Photo { AddDate = localDateTime };

            ViewBag.Categories = categoryList;
            return View(photo);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Photo photo) {

            if (photo.CategoryID < 0) {

                ModelState.AddModelError("CategoryID", "Wybierz kategorię");
            }

            if (ModelState.IsValid) {

                //add category
                photo.Category = repository.Categories
                    .Where(c => c.CategoryID == photo.CategoryID)
                    .FirstOrDefault();

                //check flename
                int i = 1;
                string tmpFileName = photo.FileName;
                while (repository.CheckFileExist(photo.FileName)) {
                    photo.FileName = "(" + i + ")" + tmpFileName;
                    ++i;
                }

                //remove spaces
                tmpFileName = photo.FileName.Replace(" ", "_");

                //upload file
                var filePath = Path.Combine(Directory.GetCurrentDirectory(),
                    photosDirectory, tmpFileName);
                using (var stream = new FileStream(filePath, FileMode.Create)) {
                    await photo.FileToUpload.CopyToAsync(stream);
                }

                photo.FileName = tmpFileName;

                //create thumbs
                Thumbs.Create(photo.FileName, configuration);

                //save to database
                repository.SavePhoto(photo);
                TempData["message"] = $"Plik {photo.FileName} został zapisany";
                return RedirectToAction("Index");

            } else {

                ViewBag.Categories = categoryList;
                return View(photo);
            }
        }

        [HttpPost]
        public IActionResult Edit(Photo photo) {

            ModelState.Remove("FileToUpload");

            if (ModelState.IsValid) {

                repository.SavePhoto(photo);
                TempData["message"] = $"Plik {photo.FileName} został zapisany";
                return RedirectToAction("Index");

            } else {

                ViewBag.Categories = categoryList;
                ViewBag.Thumb = Url.Content(
                    photosViewDirectory + "/thumbs/" + photo.FileName);
                return View(photo);
            }
        }

        [HttpPost]
        public IActionResult Delete(int photoId) {

            Photo deletedPhoto = repository.DeletePhoto(photoId);

            //delete photo
            var filePath = Path.Combine(Directory.GetCurrentDirectory(),
                photosDirectory, deletedPhoto.FileName);
            System.IO.File.Delete(filePath);

            //delete thumb
            filePath = Path.Combine(Directory.GetCurrentDirectory(),
                photosDirectory + "/thumbs", deletedPhoto.FileName);
            System.IO.File.Delete(filePath);

            if (deletedPhoto != null) {
                TempData["message"] = $"Plik {deletedPhoto.FileName} został usunięty";
            }

            return RedirectToAction("Index");
        }
    }
}
