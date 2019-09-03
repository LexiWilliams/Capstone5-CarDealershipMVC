using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Capstone5_CarDealershipMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Capstone5_CarDealershipMVC.Controllers
{

    [Authorize]
    public class CarController : Controller
    {
        private readonly CarDealershipDbContext _context;

        public CarController(CarDealershipDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> ListCars()
        {
            var result = await GetCarList();

            return View(result);
        }
        public async Task<List<Car>>  GetCarList()
        {
            
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44376");

            var response = await client.GetAsync($"api/car");
            var result = await response.Content.ReadAsAsync<List<Car>>();

            return result;
        }
        public IActionResult AddCarToFavorites(Car car)
        {
            AspNetUsers thisUser = _context.AspNetUsers.Where(u => u.UserName == User.Identity.Name).First();

            FavoriteCars newCar = new FavoriteCars();
            newCar.CarId = car.Id;
            newCar.Make = car.Make;
            newCar.Model = car.Model;
            newCar.Year = car.Year;
            newCar.Color = car.Color;
            newCar.UserId = thisUser.Id;
            newCar.User = thisUser;
            
            if (ModelState.IsValid)
            {
                _context.FavoriteCars.Add(newCar);
                _context.SaveChanges();
                return RedirectToAction("ListCars", "Car");
            }
            return View("ListCars");
        }
        public IActionResult GetFavoritesList()
        {
            AspNetUsers thisUser = _context.AspNetUsers.Where(u => u.UserName == User.Identity.Name).First();
            List<FavoriteCars> favoriteList = _context.FavoriteCars.Where(u => u.UserId == thisUser.Id).ToList();
            return View(favoriteList);
        }
        public IActionResult RemoveFromFavorites(FavoriteCars car)
        {
            _context.FavoriteCars.Remove(car);
            _context.SaveChanges();
            return RedirectToAction("GetFavoritesList");
        }
        public async Task<IActionResult> FilterColor(string Color)
        {
            List<Car> filteredList = new List<Car>();

            var carList = await GetCarList();

            foreach (Car car in carList)
            {
                if (car.Color.ToLower().Contains(Color.ToLower()))
                {
                    filteredList.Add(car);
                }
            }
            return View("ListCars", filteredList);
        }
        public async Task<IActionResult> FilterMake(string Make)
        {
            List<Car> filteredList = new List<Car>();

            var carList = await GetCarList();

            foreach (Car car in carList)
            {
                if (car.Make.ToLower().Contains(Make.ToLower()))
                {
                    filteredList.Add(car);
                }
            }
            return View("ListCars", filteredList);
        }
        public async Task<IActionResult> FilterModel(string Model)
        {
            List<Car> filteredList = new List<Car>();

            var carList = await GetCarList();

            foreach (Car car in carList)
            {
                if (car.Model.ToLower().Contains(Model.ToLower()))
                {
                    filteredList.Add(car);
                }
            }
            return View("ListCars", filteredList);
        }
        public async Task<IActionResult> FilterYear(int Year)
        {
            List<Car> filteredList = new List<Car>();

            var carList = await GetCarList();

            foreach (Car car in carList)
            {
                if (car.Year == Year)
                {
                    filteredList.Add(car);
                }
            }
            return View("ListCars", filteredList);
        }








        //public async Task<IActionResult> GetFavoriteCars()
        //{
        //    var client = new HttpClient();
        //    client.BaseAddress = new Uri("https://localhost:44376"); 

        //    var response = await client.GetAsync($"api/car");
        //    var result = await response.Content.ReadAsAsync<List<FavoriteCars>>();

        //    return View(result);
        //}
        //public async Task<IActionResult> GetFavoriteCarsById(int id)
        //{
        //    var client = new HttpClient();
        //    client.BaseAddress = new Uri("https://localhost:44376"); 

        //    var response = await client.GetAsync($"api/car/{id}");
        //    var result = await response.Content.ReadAsAsync<FavoriteCars>();

        //    return View(result);
        //}
        //public IActionResult AddFavoriteCars()
        //{
        //    return View();
        //}
        //[HttpPost]
        //public async Task<IActionResult> AddFavoriteCars(FavoriteCars favoriteCars)
        //{
        //    var client = new HttpClient();
        //    client.BaseAddress = new Uri("https://localhost:44376");

        //    var postFavoriteCar = await client.PostAsJsonAsync<FavoriteCars>("api/car", favoriteCars);

        //    if (postFavoriteCar.IsSuccessStatusCode)
        //    {
        //        return RedirectToAction("Index");
        //    }

        //    ModelState.AddModelError(string.Empty, "Server error. Did not add to Db.");
        //    return View(favoriteCars);
        //}
        //public async Task<IActionResult> EditFavoriteCar(int id)
        //{
        //    var client = new HttpClient();
        //    client.BaseAddress = new Uri("https://localhost:44376");

        //    var response = await client.GetAsync($"api/car/{id}");
        //    var result = await response.Content.ReadAsAsync<FavoriteCars>();
        //    return View(result);
        //}
        //[HttpPost]
        //public async Task<IActionResult> EditFavoriteCar(FavoriteCars updatedFavoriteCars)
        //{
        //    var client = new HttpClient();
        //    client.BaseAddress = new Uri("https://localhost:44376");

        //    var putTask = await client.PutAsJsonAsync<FavoriteCars>($"api/car/{updatedFavoriteCars.CarId}", updatedFavoriteCars);
        //    if (putTask.IsSuccessStatusCode)
        //    {
        //        return RedirectToAction("Index");
        //    }
        //    return RedirectToAction("EditFavoriteCars", updatedFavoriteCars.CarId);
        //}
        //public async Task<IActionResult> DeleteFavoriteCars(int id)
        //{
        //    var client = new HttpClient();
        //    client.BaseAddress = new Uri("https://localhost:44376");

        //    var deleteTask = await client.DeleteAsync($"api/car/{id}");
        //    return RedirectToAction("Index");
        //}
    }
}