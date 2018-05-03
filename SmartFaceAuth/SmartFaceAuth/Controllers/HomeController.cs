using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartFaceAuth.FaceAuth;
using SmartFaceAuth.Models;
using System.IO;

namespace SmartFaceAuth.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}