using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChatRoom.Controllers
{
    public class ChatController : Controller
    {
        [Route("", Name = "ChatHome")]
        public ActionResult Home()
        {
            return View();
        }
    }
}