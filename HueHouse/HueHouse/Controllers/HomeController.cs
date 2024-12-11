using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HueHouse.Models;
using System.Data.SqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Menu;
using System.Data.Entity.Infrastructure;  // Thêm dòng này
using System.Diagnostics;
using System.Data.Entity.Validation; // Thêm thư viện này để ghi log


namespace HueHouse.Controllers
{
    public class HomeController : Controller

    {
        private readonly huehousemodel db; 

        public HomeController()
        {
            db = new huehousemodel();   // Khởi tạo database
        }



        // Trang chủ - Hiển thị danh sách sản phẩm nổi bật
        public ActionResult Index()
        {
            // Lấy danh sách sản phẩm nổi bật - Bán Chạy
            var topSellingProducts = (from hp in db.HighlightedProducts
                                      join p in db.Products on hp.ProductID equals p.ProductID
                                      where hp.HighlightType == "Bán Chạy"
                                      select p).Take(20).ToList();

            // Lấy danh sách sản phẩm mới
            var newProducts = (from hp in db.HighlightedProducts
                               join p in db.Products on hp.ProductID equals p.ProductID
                               where hp.HighlightType == "Sản Phẩm Mới"
                               select p).Take(20).ToList();

            // Lấy danh sách sản phẩm giảm giá
            var discountedProducts = (from hp in db.HighlightedProducts
                                      join p in db.Products on hp.ProductID equals p.ProductID
                                      where hp.HighlightType == "Giảm Giá"
                                      select p).Take(20).ToList();

            // Lấy danh sách sản phẩm hot nhất tháng
            var hotProducts = (from hp in db.HighlightedProducts
                               join p in db.Products on hp.ProductID equals p.ProductID
                               where hp.HighlightType == "Hot Nhất Tháng"
                               select p).Take(20).ToList();

            // Đưa dữ liệu vào ViewBag để sử dụng trong Index.cshtml
            ViewBag.TopSellingProducts = topSellingProducts;
            ViewBag.NewProducts = newProducts;
            ViewBag.DiscountedProducts = discountedProducts;
            ViewBag.HotProducts = hotProducts;

            return View();
        }



        // Action tìm kiếm món ăn
        public ActionResult Search(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                // Nếu từ khóa tìm kiếm trống, trả về danh sách rỗng
                return Json(new List<FoodItem>(), JsonRequestBehavior.AllowGet);
            }

            // Tìm kiếm trong bảng Products theo tên món ăn
            var result = db.Products
                           .Where(p => p.ProductName.Contains(query))
                           .Select(p => new FoodItem
                           {
                               FooditemID = p.ProductID,
                               FoodName = p.ProductName,
                               ImageUrl = p.ProductImage,
                               Price = p.Price
                           })
                           .ToList();

            // Trả về kết quả dưới dạng JSON
            return Json(result, JsonRequestBehavior.AllowGet);
        }



        // Action lấy danh sách sản phẩm
        public ActionResult Product()
        {
            using (var db = new huehousemodel())
            {
                // Lấy danh sách món ăn được nhóm theo danh mục
                var groupedProducts = db.Products
                                        .GroupBy(p => p.Category.CategoryName)
                                        .ToList();

                // Truyền dữ liệu vào View
                return View(groupedProducts);
            }
        }


        // Action xử lý thêm sản phẩm vào giỏ hàng
        [HttpPost]
        public ActionResult AddToCart(int productId, int quantity = 1)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Home", new { returnUrl = Request.Url.AbsoluteUri });
            }

            var product = db.Products.SingleOrDefault(p => p.ProductID == productId);
            if (product == null)
            {
                return HttpNotFound();
            }

            int userId = (int)Session["UserID"];

            var existingCartItem = db.Cart.SingleOrDefault(c => c.UserID == userId && c.ProductID == productId);

            if (existingCartItem == null)
            {
                var newCartItem = new Cart
                {
                    UserID = userId,
                    ProductID = product.ProductID,
                    ProductName = product.ProductName,
                    ProductImage = product.ProductImage,
                    Price = product.Price,
                    Quantity = quantity,
                    TotalAmount = product.Price * quantity,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                db.Cart.Add(newCartItem);
            }
            else
            {
                existingCartItem.Quantity += quantity;
                existingCartItem.TotalAmount = existingCartItem.Quantity * existingCartItem.Price;
                existingCartItem.UpdatedAt = DateTime.Now;
            }

            db.SaveChanges();

            return RedirectToAction("ShoppingCart");
        }


        // Action hiển thị giỏ hàng
        public ActionResult ShoppingCart()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            int userId = (int)Session["UserID"];

            var cartItems = db.Cart.Where(c => c.UserID == userId).ToList();

            return View(cartItems);
        }


        // Action xử lý xóa sản phẩm khỏi giỏ hàng
        [HttpPost]
        public ActionResult RemoveFromCart(int cartId)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            var cartItem = db.Cart.SingleOrDefault(c => c.CartID == cartId);

            if (cartItem != null)
            {
                db.Cart.Remove(cartItem);
                db.SaveChanges();
            }

            return RedirectToAction("ShoppingCart");
        }

        //trang ckeckout kiểm tra trước khi đặt hàng
        public ActionResult Checkout()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            int userId = (int)Session["UserID"];
            var cartItems = db.Cart.Where(c => c.UserID == userId).ToList();

            var user = db.Users.Find(userId);

            var checkoutViewModel = new CheckoutViewModel
            {
                CartItems = cartItems,
                Users = new Users
                {
                    Username = user.Username,
                    Address = user.Address,
                    Phone= user.Phone,
                }
            };

            return View(checkoutViewModel);
        }




        // HTTP GET: /Home/Register
        public ActionResult Register()
        {
            // Trả về View cho trang đăng ký người dùng
            // Phương thức GET này chỉ hiển thị giao diện để người dùng nhập thông tin đăng ký
            return View();
        }

        // HTTP POST: /Home/Register
        [HttpPost]
        public ActionResult Register(FormCollection form)
        {
            // Tạo một đối tượng mới của lớp Users
            // Đây là đối tượng dùng để lưu trữ thông tin người dùng khi họ đăng ký
            Users user = new Users();

            // Gán giá trị từ form vào đối tượng user
            // Sử dụng FormCollection để lấy dữ liệu từ form và gán vào các thuộc tính của đối tượng
            user.FullName = form["FullName"];      // Họ và tên
            user.Sex = form["Sex"];                // Giới tính
            user.Phone = form["Phone"];            // Số điện thoại
            user.Address = form["Address"];        // Địa chỉ
            user.Email = form["Email"];            // Email
            user.Username = form["Username"];      // Tên đăng nhập
            user.Password = form["Password"];      // Mật khẩu
            user.CreatedAt = DateTime.Now;         // Ngày tạo tài khoản (thời gian hiện tại)
            user.UpdatedAt = DateTime.Now;         // Ngày cập nhật tài khoản (thời gian hiện tại)

            // Thêm đối tượng user vào cơ sở dữ liệu
            // db.Users.Add(user); sẽ thêm người dùng vào bảng Users trong cơ sở dữ liệu
            db.Users.Add(user);

            // Lưu thay đổi vào cơ sở dữ liệu
            // db.SaveChanges() sẽ lưu tất cả thay đổi (trong trường hợp này là thêm mới người dùng) vào cơ sở dữ liệu
            db.SaveChanges();

            // Sau khi đăng ký thành công, chuyển hướng đến trang đăng nhập
            // return RedirectToAction("Login"); sẽ chuyển người dùng đến phương thức Login trong controller Home
            return RedirectToAction("Login");
        }



        // HTTP GET: /Home/LogIn
        public ActionResult LogIn()
        {
            // Trả về View cho trang đăng nhập
            // Phương thức GET sẽ hiển thị trang Login để người dùng nhập thông tin đăng nhập
            return View();
        }

        // HTTP POST /Home/LogIn
        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            // Kiểm tra trong bảng Users (người dùng thông thường)
            var users = db.Users.Where(u => u.Username == username && u.Password == password).ToList();
            // Dòng này thực hiện truy vấn tìm kiếm tất cả người dùng trong bảng `Users` với điều kiện tên đăng nhập và mật khẩu trùng khớp.
            // Kết quả trả về là một danh sách người dùng (dù chỉ có một người dùng hợp lệ, vì vậy dùng `ToList()` để lấy tất cả).

            // Kiểm tra trong bảng Admins (quản trị viên)
            var admin = db.Admin.FirstOrDefault(a => a.LoginName == username && a.Password == password);
            // Dòng này kiểm tra trong bảng `Admin` xem có tài khoản quản trị viên nào có tên đăng nhập và mật khẩu khớp với yêu cầu không.
            // `FirstOrDefault` sẽ trả về đối tượng quản trị viên đầu tiên tìm thấy hoặc `null` nếu không có.

            // Kiểm tra nếu tìm thấy người dùng thông thường
            if (users != null && users.Any()) // Nếu là người dùng thông thường
            {
                var user = users.First(); // Giả sử chỉ có 1 người dùng duy nhất trong trường hợp hợp lệ
                if (user != null)
                {
                    // Đăng nhập thành công cho người dùng
                    Session["UserID"] = user.UserID;        // Lưu ID người dùng vào session
                    Session["FullName"] = user.FullName;    // Lưu họ và tên vào session
                    Session["Sex"] = user.Sex;              // Lưu giới tính vào session
                    Session["Phone"] = user.Phone;          // Lưu số điện thoại vào session
                    Session["Address"] = user.Address;      // Lưu địa chỉ vào session
                    Session["Email"] = user.Email;          // Lưu email vào session
                    Session["Username"] = user.Username;    // Lưu tên đăng nhập vào session
                    Session["Password"] = user.Password;    // Lưu mật khẩu vào session

                    // Kiểm tra nếu có RedirectUrl trong Session
                    var redirectUrl = Session["RedirectUrl"];
                    if (redirectUrl != null)
                    {
                        // Reset lại RedirectUrl sau khi sử dụng
                        Session["RedirectUrl"] = null;    // Xóa giá trị RedirectUrl trong session
                        return Redirect(redirectUrl.ToString()); // Chuyển hướng đến trang giỏ hàng (nếu có)
                    }

                    // Nếu không có RedirectUrl, chuyển về trang chủ
                    return RedirectToAction("Index", new { id = user.UserID }); // Chuyển hướng đến trang chủ và truyền ID người dùng
                }
            }

            // Nếu người dùng không phải là người dùng thông thường, kiểm tra là quản trị viên
            else if (admin != null) // Nếu là quản trị viên
            {
                // Đăng nhập thành công cho quản trị viên
                Session["AdminID"] = admin.AdminID;          // Lưu ID quản trị viên vào session
                Session["FullName"] = admin.FullName;        // Lưu họ và tên của quản trị viên vào session
                Session["Username"] = admin.LoginName;        // Lưu tên đăng nhập của quản trị viên vào session
                Session["Password"] = admin.Password;        // Lưu mật khẩu của quản trị viên vào session

                // Chuyển hướng đến trang quản trị của admin
                return RedirectToAction("Index_Admin", new { id = admin.AdminID }); // Chuyển hướng đến trang quản lý của admin và truyền ID của admin
            }
            else
            {
                // Đăng nhập thất bại, hiển thị lỗi
                ViewBag.Message = "Tên đăng nhập hoặc mật khẩu không đúng hoặc không tồn tại."; // Gửi thông báo lỗi về View
            }

            return View(); // Trả về View Login nếu đăng nhập thất bại
        }



        // Hiển thị trang thông tin cá nhân của người dùng
        public ActionResult My_Account()
        {
            // Kiểm tra nếu người dùng đã đăng nhập (dựa trên Session)
            if (Session["UserID"] == null)
            {
                ViewBag.Message = "Session hết hạn. Vui lòng đăng nhập lại.";
                return RedirectToAction("LogIn", "Home");
            }

            int userId = (int)Session["UserID"]; // Lấy UserID từ Session

            // Lấy thông tin khách hàng từ database
            var user = db.Users.FirstOrDefault(c => c.UserID == userId);
            if (user == null)
            {
                ViewBag.Message = "Không tìm thấy khách hàng.";
                return View();
            }

            return View(user); // Truyền thông tin khách hàng sang view
        }



       // Xử lý khi người dùng cập nhật thông tin cá nhân
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult My_Account(Users model)
        {
            // Kiểm tra nếu người dùng đã đăng nhập (dựa trên Session)
            if (Session["UserID"] == null)
            {
                ViewBag.Message = "Session hết hạn. Vui lòng đăng nhập lại.";
                return RedirectToAction("LogIn", "Home");
            }

            int userId = (int)Session["UserID"]; // Lấy UserID từ Session

            // Lấy thông tin khách hàng từ database
            var user = db.Users.FirstOrDefault(c => c.UserID == userId);
            if (user == null)
            {
                ViewBag.Message = "Không tìm thấy khách hàng.";
                return View(model); // Nếu không tìm thấy khách hàng, trả về view với model hiện tại
            }

            // Cập nhật thông tin khách hàng
            user.FullName = model.FullName;
            user.Email = model.Email;
            user.Phone = model.Phone;
            user.Sex = model.Sex;
            user.Address = model.Address;
            user.Password = model.Password;

            try
            {
                db.SaveChanges(); // Lưu thay đổi vào database

                // Cập nhật Session với thông tin mới
                Session["FullName"] = model.FullName;
                Session["Email"] = model.Email;
                Session["Phone"] = model.Phone;
                Session["Sex"] = model.Sex;
                Session["Address"] = model.Address;
                Session["Password"] = model.Password;

                ViewBag.Message = "Cập nhật thông tin thành công."; // Hiển thị thông báo thành công
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Có lỗi xảy ra khi cập nhật thông tin. Vui lòng thử lại."; // Hiển thị thông báo lỗi
            }

            return View(user); // Trả về view với thông tin khách hàng
        }




        [HttpGet]
        public ActionResult Logout()
        {
            // Xoá thông tin người dùng
            Session["CustomerID"] = null;    // Xoá ID của người dùng
            Session["FullName"] = null;      // Xoá Họ và Tên của người dùng
            Session["Sex"] = null;           // Xoá Giới tính của người dùng
            Session["Phone"] = null;         // Xoá SĐT của người dùng
            Session["Address"] = null;       // Xoá Địa chỉ của người dùng
            Session["Email"] = null;         // Xoá Email của người dùng
            Session["Username"] = null;      // Xoá Tên đăng nhập của người dùng
            Session["Password"] = null;      // Xoá Mật khẩu của người dùng

            // Xoá thông tin admin
            Session["AdminID"] = null;       // Xoá ID của admin
            Session["FullName"] = null; // Xoá Họ và Tên của admin
            Session["Username"] = null; // Xoá Tên đăng nhập của admin
            Session["Password"] = null; // Xoá Mật khẩu của admin

            // Sau khi đăng xuất, chuyển hướng về trang chủ
            return RedirectToAction("Index");
        }



        public ActionResult Payment_Instructions()
        {
            return View();
        }

        public ActionResult Order_Instructions()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        // chi tiết sản phẩm
        public ActionResult Details(int id)
        {
            var product = db.Products.FirstOrDefault(p => p.ProductID == id);
            var productDetail = db.ProductDetails.FirstOrDefault(pd => pd.ProductID == id);

            if (product == null || productDetail == null)
            {
                return HttpNotFound();
            }

            var viewModel = new ProductDetailsViewModel
            {
                Products = product,
                ProductDetails = productDetail
            };

            return View(viewModel);
        }


        // Tăng giảm số lượng sản phẩm trong giỏ hàng
        public ActionResult UpdateQuantity(int cartId, string actionType)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            int userId = (int)Session["UserID"];

            var cartItem = db.Cart.SingleOrDefault(c => c.CartID == cartId && c.UserID == userId);

            if (cartItem == null)
            {
                return HttpNotFound();
            }

            if (actionType == "increase")
            {
                cartItem.Quantity += 1;
            }
            else if (actionType == "decrease")
            {
                if (cartItem.Quantity > 1)
                {
                    cartItem.Quantity -= 1;
                }
            }

            cartItem.TotalAmount = cartItem.Quantity * cartItem.Price;
            cartItem.UpdatedAt = DateTime.Now;

            db.SaveChanges();

            return RedirectToAction("ShoppingCart");
        }



        //Trang Order
        [HttpPost]
        public ActionResult ConfirmOrder(CheckoutViewModel model)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            int userId = (int)Session["UserID"];
            var cartItems = db.Cart.Where(c => c.UserID == userId).ToList();

            if (!cartItems.Any())
            {
                return RedirectToAction("ShoppingCart");
            }

            int totalAmount = cartItems.Sum(item => item.TotalAmount ?? 0);

            var newOrder = new Orders
            {
                UserID = userId,
                OrderDate = DateTime.Now,
                Status = "Đang Xử Lý",
                TotalAmount = (int)totalAmount,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            db.Orders.Add(newOrder);

            try
            {
                db.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var validationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Debug.WriteLine($"Property: {validationError.PropertyName} Error: {validationError.ErrorMessage}");
                    }
                }
                ModelState.AddModelError("", "Unable to save order. Validation failed.");
                return View("Error");
            }
            catch (DbUpdateException ex)
            {
                Debug.WriteLine($"Error saving order: {ex.Message}");
                Debug.WriteLine($"Inner exception: {ex.InnerException?.Message}");
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                return View("Error");
            }

            foreach (var item in cartItems)
            {
                var orderDetail = new OrderDetails
                {
                    OrderID = newOrder.OrderID,
                    ProductID = item.ProductID,
                    Quantity = item.Quantity,
                    Price = item.Price,
                    TotalAmount = item.TotalAmount,
                };
                db.OrderDetails.Add(orderDetail);
            }

            try
            {
                db.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var validationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Debug.WriteLine($"Property: {validationError.PropertyName} Error: {validationError.ErrorMessage}");
                    }
                }
                ModelState.AddModelError("", "Unable to save order details. Validation failed.");
                return View("Error");
            }
            catch (DbUpdateException ex)
            {
                Debug.WriteLine($"Error saving order details: {ex.Message}");
                Debug.WriteLine($"Inner exception: {ex.InnerException?.Message}");
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                return View("Error");
            }

            // Clear cart
            db.Cart.RemoveRange(cartItems);
            db.SaveChanges();

            return RedirectToAction("Order", new { orderId = newOrder.OrderID });
        }

        public ActionResult Order(int orderId)
        {
            var order = db.Orders.Include("OrderDetails.Products").FirstOrDefault(o => o.OrderID == orderId);
            if (order == null)
            {
                return HttpNotFound();
            }

            var model = new OrderViewModel
            {
                CartItems = order.OrderDetails.Select(od => new Cart
                {
                    ProductImage = od.Products.ProductImage,
                    ProductName = od.Products.ProductName,
                    Quantity = od.Quantity,
                    Price = od.Price,
                    TotalAmount = od.TotalAmount
                }).ToList(),
                TotalAmount = order.TotalAmount
            };

            return View(model);
        }




    }
}