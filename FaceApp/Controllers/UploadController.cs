using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FaceApp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FaceApp.Controllers
{
    public class UploadController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Upload()
        {
            string responseObject = "";
            Stream fs = Request.Form.Files[0].OpenReadStream();
            BinaryReader br = new BinaryReader(fs);
            byte[] bytes = br.ReadBytes((Int32)fs.Length);

            HttpContent metaDataContent = new ByteArrayContent(bytes);
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.PostAsync("http://poc1.tarento.ai/upload", metaDataContent))
                {
                    responseObject = await response.Content.ReadAsStringAsync();
                }
            }
            ViewBag.fileResult = responseObject;
            return View("Index");
        }


        [HttpPost]
        public async Task<IActionResult> MapImageVideo()
        {
            string responseObject = "";
            string body = "{\"video_file_id\":\"" + Request.Form["VideoId"].ToString() + "\"," + "\"image_file_id\":\"" + Request.Form["ImageId"].ToString() + "\"}";
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.PostAsync("http://poc1.tarento.ai/api/v1/face/verify", new StringContent(body, Encoding.UTF8, "application/json")))
                {
                    responseObject = await response.Content.ReadAsStringAsync();
                    
                }
            }
            ViewBag.mapId = responseObject;
            return View("Index");
        }
    }
}