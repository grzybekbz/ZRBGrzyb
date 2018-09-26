using Microsoft.AspNetCore.Mvc;

namespace ZRBGrzyb.Controllers {

    public class ErrorController : Controller {

        public ViewResult Error() => View();
    }
}
