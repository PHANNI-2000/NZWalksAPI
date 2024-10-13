using Microsoft.AspNetCore.Mvc;
using NZWalks.UI.Models;
using NZWalks.UI.Models.DTO;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NZWalks.UI.Controllers
{
    public class RegionsController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;
        // HttpClient เป็นคลาสที่ใช้สำหรับทำการร้องขอ HTTP requests เช่น GET, POST ไปยัง APIs หรือ endpoints ต่าง ๆ
        public RegionsController(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<RegionDto> response = new List<RegionDto>();
            try
            {
                // Get All Regions from Web API
                var client = httpClientFactory.CreateClient(); // สร้าง instance ของ HttpClient จาก HttpClientFactory

                var httpResponseMessage = await client.GetAsync("https://localhost:44350/api/regions");  // ทำการเรียก HTTP request

                httpResponseMessage.EnsureSuccessStatusCode(); // ตรวจสอบว่าการ request สำเร็จ (เช็ค HTTP status code)

                response.AddRange(await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<RegionDto>>()); // อ่านข้อมูลจาก HTTP response และแปลงจาก JSON เป็น IEnumerable<RegionDto>
            }
            catch (Exception ex)
            {
                // Log the exception
            }

            return View(response);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddRegionViewModel model)
        {
            var client = httpClientFactory.CreateClient(); // สร้าง instance ของ HttpClient จาก HttpClientFactory
            var httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://localhost:44350/api/regions"),
                Content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json")
            };

            var httpResponseMessage = await client.SendAsync(httpRequestMessage);
            httpResponseMessage.EnsureSuccessStatusCode(); // ตรวจสอบว่าการ request สำเร็จ (เช็ค HTTP status code)

            var response = await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<RegionDto>>();

            if (response is not null)
            {
                return RedirectToAction("Index", "Regions");
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var client = httpClientFactory.CreateClient(); // สร้าง instance ของ HttpClient จาก HttpClientFactory

            var response = await client.GetFromJsonAsync<RegionDto>($"https://localhost:44350/api/regions/{id.ToString()}");

            if (response is not null)
            {
                return View(response);
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(RegionDto request)
        {
            var client = httpClientFactory.CreateClient(); // สร้าง instance ของ HttpClient จาก HttpClientFactory
            var httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri($"https://localhost:44350/api/regions/{request.Id}"),
                Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json") // ส่งข้อมูล request ในรูปแบบ JSON พร้อมกับ request ด้วย ซึ่งจะถูกใช้ในการอัปเดตข้อมูลใน API นั้น
            };

            var httpResponseMessage = await client.SendAsync(httpRequestMessage);
            httpResponseMessage.EnsureSuccessStatusCode(); // ตรวจสอบว่าการ request สำเร็จ (เช็ค HTTP status code)

            var response = await httpResponseMessage.Content.ReadFromJsonAsync<RegionDto>();

            if (response is not null)
            {
                return RedirectToAction("Edit", "Regions");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(RegionDto request)
        {
            try
            {
                var client = httpClientFactory.CreateClient(); // สร้าง instance ของ HttpClient จาก HttpClientFactory

                var httpReponseMessage = await client.DeleteAsync($"https://localhost:44350/api/regions/{request.Id}");

                httpReponseMessage.EnsureSuccessStatusCode(); // ตรวจสอบว่าการ request สำเร็จ (เช็ค HTTP status code)

                return RedirectToAction("Index", "Regions");
            }
            catch (Exception ex)
            {
                // Console
            }

            return View("Edit");
        }
    }
}
