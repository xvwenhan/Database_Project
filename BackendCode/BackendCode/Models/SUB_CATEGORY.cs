namespace BackendCode.Models
{
    public class SUB_CATEGORY
    {
        public string SUBCATEGORY_ID { get; set; }
        public string CATEGORY_NAME { get; set; }
        public string SUBCATEGORY_NAME { get; set; }
        public CATEGORY CATEGORY { get; set; }
    }
}