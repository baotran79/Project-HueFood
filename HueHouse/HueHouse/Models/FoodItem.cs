namespace HueHouse.Models
{
    public class FoodItem
    {
        public int FooditemID { get; set; }             // Mã món ăn (Id)
        public string FoodName { get; set; }        // Tên món ăn
        public string ImageUrl { get; set; }    // Đường dẫn đến hình ảnh món ăn
        public decimal Price { get; set; }      // Giá món ăn
    }
}